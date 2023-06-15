using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RPG.BuildingBlocks.Common.Constants;
using RPG.RPG.BuildingBlocks.Common.AuthorizationAttributes;

namespace RPG.BuildingBlocks.Common
{
    public static class CommonExtensions
    {

        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            //string serviceId = configuration["ServiceId"];
            //string authorityUrl = configuration["AUTHORITY_URL"];
            //bool requireHttpsMetadata = bool.Parse(configuration["AUTHORITY_REQUIRE_HTTPS_METADATA"]);
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "http://localhost:5274";
                options.Audience = "character";
                options.RequireHttpsMetadata = false;

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.HasValue && path.Value.Contains("/messaging"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(ctx =>
                    {
                        return ctx.User.HasClaim(claim => claim.Type == UserConstants.ACTIVE_CLAIM && claim.Value == "True")
                            || ctx.User.HasClaim(claim =>
                                claim.Type == JwtClaimTypes.ClientId && claim.Value == Constants.ClientIds.Internal);
                    });
                });

                options.AddPolicy(Authorization.INCREATION_USER, policy => policy.RequireAuthenticatedUser());

                options.AddPolicy(Authorization.SERVICE_DISCOVERY, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", SharedScopes.SCOPE_SERVICE_DISCOVERY);
                });

                options.AddPolicy(Authorization.EVENTBUS, policy =>
                {
                    policy.Requirements.Add(new ValidApiTokenRequirement());
                });

                options.DefaultPolicy = options.GetPolicy("Default")!;
            });
            services.AddAuthorization();
            return services;
        }
    }
}
