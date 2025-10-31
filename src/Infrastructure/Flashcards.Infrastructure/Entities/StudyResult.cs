using System;
using System.ComponentModel.DataAnnotations;

namespace Flashcards.Infrastructure.Entities;

public class StudyResult
{
    public int Id { get; set; }

    [Required]
    public int DeckId { get; set; }
    public Deck? Deck { get; set; }

    [Required]
    [StringLength(50)]
    public string PlayerName { get; set; } = "Guest";

    [Required]
    [StringLength(20)]
    public string Mode { get; set; } = "Review"; 

    public int Correct { get; set; }
    public int Total { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
}
