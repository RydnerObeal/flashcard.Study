using Flashcards.Infrastructure.Data;
using Flashcards.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Presentation.Controllers;

[Authorize]
public class StudyController(AppDbContext db, UserManager<IdentityUser> userManager) : Controller
{
    private readonly AppDbContext _db = db;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    public async Task<IActionResult> Review(int deckId)
    {
        var deck = await _db.Decks.Include(d => d.Flashcards).FirstOrDefaultAsync(d => d.Id == deckId);
        if (deck is null) return NotFound();
        ViewBag.Deck = deck;
        return View(deck.Flashcards.OrderBy(f => f.Id).ToList());
    }

    public async Task<IActionResult> Quiz(int deckId)
    {
        var deck = await _db.Decks.Include(d => d.Flashcards).FirstOrDefaultAsync(d => d.Id == deckId);
        if (deck is null) return NotFound();
        ViewBag.Deck = deck;
        return View(deck.Flashcards.OrderBy(f => f.Id).ToList());
    }

    public async Task<IActionResult> Scores(int deckId)
    {
        var deck = await _db.Decks.FindAsync(deckId);
        if (deck is null) return NotFound();
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Unauthorized();
        ViewBag.Deck = deck;
        var results = await _db.StudyResults
            .Where(r => r.DeckId == deckId && r.UserId == user.Id)
            .OrderByDescending(r => r.PlayedAt)
            .ToListAsync();
        return View(results);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveResult(int deckId, int correct, int total, string mode)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Unauthorized();
        
        var result = new StudyResult
        {
            DeckId = deckId,
            UserId = user.Id,
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
