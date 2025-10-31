using Flashcards.Infrastructure.Data;
using Flashcards.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Presentation.Controllers;

[Authorize]
public class StudyController(AppDbContext db) : Controller
{
    private readonly AppDbContext _db = db;

    // GET: /Study/Review?deckId=1
    public async Task<IActionResult> Review(int deckId)
    {
        var deck = await _db.Decks.Include(d => d.Flashcards).FirstOrDefaultAsync(d => d.Id == deckId);
        if (deck is null) return NotFound();
        ViewBag.Deck = deck;
        return View(deck.Flashcards.OrderBy(f => f.Id).ToList());
    }

    // GET: /Study/Quiz?deckId=1
    public async Task<IActionResult> Quiz(int deckId)
    {
        var deck = await _db.Decks.Include(d => d.Flashcards).FirstOrDefaultAsync(d => d.Id == deckId);
        if (deck is null) return NotFound();
        ViewBag.Deck = deck;
        return View(deck.Flashcards.OrderBy(f => f.Id).ToList());
    }

    // GET: /Study/Scores?deckId=1
    public async Task<IActionResult> Scores(int deckId)
    {
        var deck = await _db.Decks.FindAsync(deckId);
        if (deck is null) return NotFound();
        ViewBag.Deck = deck;
        var results = await _db.StudyResults
            .Where(r => r.DeckId == deckId)
            .OrderByDescending(r => r.PlayedAt)
            .ToListAsync();
        return View(results);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveResult(int deckId, string playerName, int correct, int total, string mode)
    {
        var result = new StudyResult
        {
            DeckId = deckId,
            PlayerName = string.IsNullOrWhiteSpace(playerName) ? "Guest" : playerName.Trim(),
            Correct = Math.Max(0, correct),
            Total = Math.Max(0, total),
            Mode = mode,
            PlayedAt = DateTime.UtcNow
        };
        _db.StudyResults.Add(result);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Scores), new { deckId });
    }
}
