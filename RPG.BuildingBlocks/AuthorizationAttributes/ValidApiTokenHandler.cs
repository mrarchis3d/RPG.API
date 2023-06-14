using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace RPG.RPG.BuildingBlocks.Common.AuthorizationAttributes
{
    public class ValidApiTokenRequirement: IAuthorizationRequirement
    {
    }

    public class ValidApiTokenHandler : AuthorizationHandler<ValidApiTokenRequirement>
    {
        private readonly IConfiguration _configuration;

        public ValidApiTokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidApiTokenRequirement requirement)
        {
            if (context.Resource is HttpContext httpContext)
            {
                var token = httpContext.Request.Headers["dapr-api-token"];

                if (token != _configuration["token"]) context.Fail();

                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
