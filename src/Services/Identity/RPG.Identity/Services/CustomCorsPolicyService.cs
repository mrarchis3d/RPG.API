using IdentityServer4.Services;

namespace RPG.Identity.Services
{
    public class CustomCorsPolicyService : ICorsPolicyService
    {
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            if(origin !=null && origin != "http://localhost:7002")
                return Task.FromResult(false);

            return Task.FromResult(true); // Personaliza según tus necesidades
        }
    }
}
