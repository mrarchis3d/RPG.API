using Microsoft.AspNetCore.Identity;

namespace RPG.Identity.Domain.UserAggregate
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
        public bool Active { get; set; }

    }
}
