using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RPG.BuildingBlocks.Common.Constants;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace RPG.BuildingBlocks.Common.Extensions
{
    public static class HttpContextExtensions
    {
#nullable enable
        public static async Task<string?> GetBearerToken(this HttpContext context)
        {
            var token = await context.GetTokenAsync("access_token");
            return token;
        }

        public static Task<string?> GetHeaderUserId(this HttpContext context)
        {
            var userId = context.Request.Headers.FirstOrDefault(x => string.Equals(x.Key, "X-USER-ID", System.StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();
            return Task.FromResult(userId);
        }

        public static async Task<string?> GetUserId(this HttpContext context)
        {
            string? userId = null;

            try
            {
                if (context.User.Claims.Any(x => x.Type == JwtClaimTypes.ClientId && x.Value == ClientIds.Internal))
                {
                    userId = await GetHeaderUserId(context);
                }
                else if (context.User.Claims.Any(x => x.Type == ClaimTypes.NameIdentifier))
                {
                    userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                }
            }
            catch (Exception)
            {
                // cannot do logging here but we can safely ignore the error since it will trigger null value
            }


            return userId;
        }
    }
}