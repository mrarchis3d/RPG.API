using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RPG.BuildingBlocks.Common.Exceptions;
using Dapr.Client;
using Microsoft.Extensions.Logging;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Extensions;

public static class ChannelExtension
{
    public static async Task<List<Guid>> GetFollowedChannelsIds(this IServiceDiscovery serviceDiscovery, ILogger logger)
    {
        List<Guid> result = null;
        try
        {
            result = await serviceDiscovery.InvokeMethodAsync<List<Guid>>(HttpMethod.Get,
                "channel", $"/api/v1/Channel/followed/ids");
        }
        catch (InvocationException ex)
        {
            await ExceptionHelper.ErrorHandle(ex.Response, logger);
        }
        return result;
    }
    
    public static async Task<List<Guid>> GetFollowedChannelsIdsByUser(this IServiceDiscovery serviceDiscovery, Guid userId, ILogger logger)
    {
        List<Guid> result = null;
        try
        {
            result = await serviceDiscovery.InvokeMethodAsync<List<Guid>>(HttpMethod.Get,
                "channel", $"/api/v1/Channel/followed/{userId}/ids");
        }
        catch (InvocationException ex)
        {
            await ExceptionHelper.ErrorHandle(ex.Response, logger);
        }
        return result;
    }
}