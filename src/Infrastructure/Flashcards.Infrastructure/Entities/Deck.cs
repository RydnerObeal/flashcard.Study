using System.Collections.Generic;

namespace Flashcards.Infrastructure.Entities;

public class Deck
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}
