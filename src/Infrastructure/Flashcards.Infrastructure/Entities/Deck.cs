using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flashcards.Infrastructure.Entities;

public class Deck
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}
