# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Common commands

- Restore dependencies
  - dotnet restore
- Build (solution)
  - dotnet build FlashcardStudyTool.sln -c Debug
  - dotnet build FlashcardStudyTool.sln -c Release
- Run the web app (ASP.NET Core MVC)
  - dotnet run --project src/Presentation/Flashcards.Presentation -c Debug
  - Hot reload: dotnet watch --project src/Presentation/Flashcards.Presentation run
- Lint/format C# (code style and analyzers)
  - dotnet format FlashcardStudyTool.sln
- Tests (no test project present yet; when added):
  - Run all: dotnet test
  - Single test by name contains: dotnet test --filter "Name~Substring"
  - Single test by fully-qualified name: dotnet test --filter "FullyQualifiedName=Namespace.Class.Method"

## Architecture overview

- Solution layout (3 projects)
  - Infrastructure (src/Infrastructure/Flashcards.Infrastructure)
    - EF Core InMemory DbContext (AppDbContext) and entity models (Deck, Flashcard).
    - One-to-many: Deck has many Flashcards; cascade delete configured in OnModelCreating.
  - Services (src/Services/Flashcards.Services)
    - Application layer with interfaces (IDeckService, IFlashcardService) and implementations using AppDbContext.
    - Encapsulates CRUD and query logic; async throughout.
  - Presentation (src/Presentation/Flashcards.Presentation)
    - ASP.NET Core MVC app (net9.0). Controllers: DecksController and FlashcardsController for CRUD flows; HomeController for landing/error.
    - Razor Views under Views/, static assets under wwwroot/ (Bootstrap, jQuery, validation libs).

- Application wiring (src/Presentation/Flashcards.Presentation/Program.cs)
  - Services.AddControllersWithViews();
  - DI registrations: AppDbContext with UseInMemoryDatabase("FlashcardsDb"); scoped services for IDeckService and IFlashcardService.
  - Routing: default pattern "{controller=Home}/{action=Index}/{id?}"; static assets mapped.

- Data flow
  - MVC Controller → Service (interface) → EF Core DbContext → InMemory store.
  - Entities are simple POCOs; IDs are ints; no migrations since InMemory provider is used.

- Notes
  - Data is not persisted between runs due to InMemory provider.
  - To switch to a real database, replace UseInMemoryDatabase with a real provider and add migrations in the Infrastructure project.
