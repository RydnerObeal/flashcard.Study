using Flashcards.Infrastructure.Entities;
using Flashcards.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Presentation.Controllers;

public class DecksController(IDeckService deckService) : Controller
{
    private readonly IDeckService _deckService = deckService;

    public async Task<IActionResult> Index()
    {
        var decks = await _deckService.GetAllAsync();
        return View(decks);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Deck deck)
    {
        if (!ModelState.IsValid) return View(deck);
        await _deckService.CreateAsync(deck);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var deck = await _deckService.GetByIdAsync(id);
        if (deck is null) return NotFound();
        return View(deck);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Deck deck)
    {
        if (id != deck.Id) return BadRequest();
        if (!ModelState.IsValid) return View(deck);
        await _deckService.UpdateAsync(deck);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _deckService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
