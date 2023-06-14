using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RPG.BuildingBlocks.Common.Exceptions;
using Dapr.Client;
using Microsoft.Extensions.Logging;


namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Extensions;

public static class FriendsExtension
{
    public static async Task<List<Guid>> GetFriends(this IServiceDiscovery serviceDiscovery, ILogger logger)
    {
        List<Guid> result = null;
        try
        {
            result = await serviceDiscovery.InvokeMethodAsync<List<Guid>>(HttpMethod.Get,
                "friends", $"/api/v1/Friends");
        }
        catch (InvocationException ex)
        {
            await ExceptionHelper.ErrorHandle(ex.Response, logger);
        }
        return result;
    }
    
    public static async Task<bool?> GetFriendshipStatus(this IServiceDiscovery serviceDiscovery, Guid userId, ILogger logger)
    {
        bool? result = null;
        try
        {
            result = await serviceDiscovery.InvokeMethodAsync<bool>(HttpMethod.Get,
                "friends", $"/api/v1/friends/{userId}");
        }
        catch (InvocationException ex)
        {
            await ExceptionHelper.ErrorHandle(ex.Response, logger);
        }
        return result;
    }
}