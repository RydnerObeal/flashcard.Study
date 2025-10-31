using Flashcards.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Deck> Decks => Set<Deck>();
    public DbSet<Flashcard> Flashcards => Set<Flashcard>();
    public DbSet<StudyResult> StudyResults => Set<StudyResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deck>()
            .HasMany(d => d.Flashcards)
            .WithOne(f => f.Deck!)
            .HasForeignKey(f => f.DeckId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudyResult>()
            .HasOne(sr => sr.Deck!)
            .WithMany()
            .HasForeignKey(sr => sr.DeckId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
