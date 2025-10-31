using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Flashcards.Infrastructure.Entities;

public class StudyResult
{
    public int Id { get; set; }

    [Required]
    public int DeckId { get; set; }
    public Deck? Deck { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }

    [Required]
    [StringLength(20)]
    public string Mode { get; set; } = "Review"; 

    public int Correct { get; set; }
    public int Total { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
}
