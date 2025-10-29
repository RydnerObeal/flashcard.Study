namespace Flashcards.Infrastructure.Entities;

public class Flashcard
{
    public int Id { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;

    public int DeckId { get; set; }
    public Deck? Deck { get; set; }
}
