using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Flashcards.Presentation.Identity;

public class StrictPasswordValidator : IPasswordValidator<IdentityUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string? password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordEmpty",
                Description = "Password cannot be empty."
            }));
        }

        int uppercase = password.Count(char.IsUpper);
        int digits = password.Count(char.IsDigit);
        int symbols = password.Count(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c));

        var errors = new System.Collections.Generic.List<IdentityError>();
        if (uppercase < 2)
            errors.Add(new IdentityError { Code = "UppercaseRequirement", Description = "Password must contain at least 2 uppercase letters." });
        if (digits < 3)
            errors.Add(new IdentityError { Code = "DigitRequirement", Description = "Password must contain at least 3 numbers." });
        if (symbols < 3)
            errors.Add(new IdentityError { Code = "SymbolRequirement", Description = "Password must contain at least 3 symbols." });

        return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
    }
}
