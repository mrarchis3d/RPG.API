using IdentityServer4.Services;
using IdentityServer4.Validation;
using RPG.Identity.Infrastructure;
using System.Security.Claims;

namespace RPG.Identity.Services;

public class CustomClaimsService : DefaultClaimsService
{
    private readonly UserIdentityDbContext _context;

    public CustomClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger, UserIdentityDbContext context) : base(profile, logger)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, ResourceValidationResult resourceResult,
        ValidatedRequest request)
    {
        var baseResult = await base.GetAccessTokenClaimsAsync(subject, resourceResult, request);
        var outputClaims = baseResult.ToList();

        //if (request.ClientId == Authorization.IDENTITY_CLIENT)
        //{
        //    var userId = request.Subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        //    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        //    var isActiveUser =!user.UserName.StartsWith(UserConstants.TEMPORAL_PREFIX) ? "True" : "False";
        //    outputClaims.Add(new Claim(UserConstants.ACTIVE_CLAIM, isActiveUser));
        //}
        
        return outputClaims;
    }
}