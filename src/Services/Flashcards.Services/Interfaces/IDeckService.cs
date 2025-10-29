using Flashcards.Infrastructure.Data;
using Flashcards.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Services.Interfaces;

public interface IDeckService
{
    Task<List<Deck>> GetAllAsync(CancellationToken ct = default);
    Task<Deck?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Deck> CreateAsync(Deck deck, CancellationToken ct = default);
    Task UpdateAsync(Deck deck, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
