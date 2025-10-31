using Flashcards.Infrastructure.Entities;
using Flashcards.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Presentation.Controllers;

[Authorize]
public class FlashcardsController(IFlashcardService cardService, IDeckService deckService) : Controller
{
    private readonly IFlashcardService _cardService = cardService;
    private readonly IDeckService _deckService = deckService;

    public async Task<IActionResult> Index(int deckId)
    {
        var cards = await _cardService.GetByDeckAsync(deckId);
        ViewBag.DeckId = deckId;
        return View(cards);
    }

    public IActionResult Create(int deckId)
    {
        ViewBag.DeckId = deckId;
        return View(new Flashcard { DeckId = deckId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Flashcard card)
    {
        if (!ModelState.IsValid) return View(card);
        await _cardService.CreateAsync(card);
        return RedirectToAction(nameof(Index), new { deckId = card.DeckId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var card = await _cardService.GetByIdAsync(id);
        if (card is null) return NotFound();
        return View(card);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Flashcard card)
    {
        if (id != card.Id) return BadRequest();
        if (!ModelState.IsValid) return View(card);
        await _cardService.UpdateAsync(card);
        return RedirectToAction(nameof(Index), new { deckId = card.DeckId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int deckId)
    {
        await _cardService.DeleteAsync(id);
        return RedirectToAction(nameof(Index), new { deckId });
    }
}
