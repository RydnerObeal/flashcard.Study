using Flashcards.Infrastructure.Data;
using Flashcards.Services.Implementations;
using Flashcards.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Flashcards.Infrastructure.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("FlashcardsDb"));

builder.Services
    .AddDefaultIdentity<Microsoft.AspNetCore.Identity.IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddPasswordValidator<Flashcards.Presentation.Identity.StrictPasswordValidator>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddScoped<IDeckService, DeckService>();
builder.Services.AddScoped<IFlashcardService, FlashcardService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    var invalidResults = db.StudyResults.Where(r => string.IsNullOrEmpty(r.UserId)).ToList();
    if (invalidResults.Any())
    {
        db.StudyResults.RemoveRange(invalidResults);
        db.SaveChanges();
    }
    
    if (!db.Decks.Any())
    {
        var decks = new List<Deck>
        {
            new Deck
            {
                Name = "Country",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Front = "Capital of France?", Back = "Paris" },
                    new Flashcard { Front = "Capital of Japan?", Back = "Tokyo" },
                    new Flashcard { Front = "Largest country by area?", Back = "Russia" },
                    new Flashcard { Front = "Country with city Cairo?", Back = "Egypt" },
                    new Flashcard { Front = "Currency of the USA?", Back = "US Dollar" },
                }
            },
            new Deck
            {
                Name = "Food",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Front = "Main ingredient in sushi?", Back = "Rice" },
                    new Flashcard { Front = "Dairy product used to make cheese?", Back = "Milk" },
                    new Flashcard { Front = "Spicy red sauce from Mexico?", Back = "Salsa" },
                    new Flashcard { Front = "Fruit high in potassium?", Back = "Banana" },
                    new Flashcard { Front = "Italian dish with pasta layers?", Back = "Lasagna" },
                }
            },
            new Deck
            {
                Name = "Math",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Front = "2 + 3 = ?", Back = "5" },
                    new Flashcard { Front = "9 - 4 = ?", Back = "5" },
                    new Flashcard { Front = "3 × 4 = ?", Back = "12" },
                    new Flashcard { Front = "16 ÷ 4 = ?", Back = "4" },
                    new Flashcard { Front = "√81 = ?", Back = "9" },
                }
            },
            new Deck
            {
                Name = "Animals",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Front = "Largest land animal?", Back = "Elephant" },
                    new Flashcard { Front = "Fastest land animal?", Back = "Cheetah" },
                    new Flashcard { Front = "Mammal that can fly?", Back = "Bat" },
                    new Flashcard { Front = "'King of the jungle'?", Back = "Lion" },
                    new Flashcard { Front = "Smart marine mammal?", Back = "Dolphin" },
                }
            },
            new Deck
            {
                Name = "Works",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Front = "Who teaches students?", Back = "Teacher" },
                    new Flashcard { Front = "Who treats sick people?", Back = "Doctor" },
                    new Flashcard { Front = "Who builds houses?", Back = "Carpenter" },
                    new Flashcard { Front = "Who fights fires?", Back = "Firefighter" },
                    new Flashcard { Front = "Who writes code?", Back = "Programmer" },
                }
            },
        };

        db.Decks.AddRange(decks);
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Decks}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
