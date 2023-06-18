using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using RPG.Identity.Domain.UserAggregate;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        // Obtiene el usuario basado en el nombre de usuario
        var user = await _userManager.FindByNameAsync(context.UserName);

        if (user != null && await _userManager.CheckPasswordAsync(user, context.Password))
        {
            // Las credenciales son válidas, establece el resultado en éxito y el subid del usuario
            context.Result = new GrantValidationResult(user.Id.ToString(), "password");
            return;
        }

        // Las credenciales son inválidas, establece el resultado en error
        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Credenciales inválidas");
    }
}