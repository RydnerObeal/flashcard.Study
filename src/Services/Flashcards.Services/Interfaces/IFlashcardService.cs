using Flashcards.Infrastructure.Entities;

namespace Flashcards.Services.Interfaces;

public interface IFlashcardService
{
    Task<List<Flashcard>> GetByDeckAsync(int deckId, CancellationToken ct = default);
    Task<Flashcard?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Flashcard> CreateAsync(Flashcard card, CancellationToken ct = default);
    Task UpdateAsync(Flashcard card, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
