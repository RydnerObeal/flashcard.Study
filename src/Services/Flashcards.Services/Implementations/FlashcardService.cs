using Flashcards.Infrastructure.Data;
using Flashcards.Infrastructure.Entities;
using Flashcards.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Services.Implementations;

public class FlashcardService : IFlashcardService
{
    private readonly AppDbContext _db;

    public FlashcardService(AppDbContext db) => _db = db;

    public async Task<List<Flashcard>> GetByDeckAsync(int deckId, CancellationToken ct = default)
        => await _db.Flashcards.Where(f => f.DeckId == deckId).ToListAsync(ct);

    public async Task<Flashcard?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Flashcards.FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task<Flashcard> CreateAsync(Flashcard card, CancellationToken ct = default)
    {
        _db.Flashcards.Add(card);
        await _db.SaveChangesAsync(ct);
        return card;
    }

    public async Task UpdateAsync(Flashcard card, CancellationToken ct = default)
    {
        _db.Flashcards.Update(card);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var card = await _db.Flashcards.FindAsync([id], ct);
        if (card is null) return;
        _db.Flashcards.Remove(card);
        await _db.SaveChangesAsync(ct);
    }
}
