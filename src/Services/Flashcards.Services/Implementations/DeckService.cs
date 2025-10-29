using Flashcards.Infrastructure.Data;
using Flashcards.Infrastructure.Entities;
using Flashcards.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Services.Implementations;

public class DeckService : IDeckService
{
    private readonly AppDbContext _db;

    public DeckService(AppDbContext db) => _db = db;

    public async Task<List<Deck>> GetAllAsync(CancellationToken ct = default)
        => await _db.Decks.Include(d => d.Flashcards).ToListAsync(ct);

    public async Task<Deck?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Decks.Include(d => d.Flashcards).FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<Deck> CreateAsync(Deck deck, CancellationToken ct = default)
    {
        _db.Decks.Add(deck);
        await _db.SaveChangesAsync(ct);
        return deck;
    }

    public async Task UpdateAsync(Deck deck, CancellationToken ct = default)
    {
        _db.Decks.Update(deck);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var deck = await _db.Decks.FindAsync([id], ct);
        if (deck is null) return;
        _db.Decks.Remove(deck);
        await _db.SaveChangesAsync(ct);
    }
}
