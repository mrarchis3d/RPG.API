using System;
using System.Collections.Generic;
using System.Linq;
using RPG.BuildingBlocks.Common.BaseContext;
using RPG.BuildingBlocks.Common.Enums;

namespace RPG.BuildingBlocks.Common.Repository.QueryableExtensions;

public static class PrivacyExtension
{
    public static IQueryable<T> ApplyPrivacyPolicy<T>(this IQueryable<T> query, TargetType? targetType, Guid? targetId,
        List<Guid> friendsIds, List<Guid> followedChannelsIds)
        where T : IBasePost
    {
        return query.Where(x => targetType == null || x.TargetType == targetType)
            .Where(x => targetId == null || x.TargetId == targetId)
            .Where(x => x.Privacy == Privacy.FRIENDS_AND_SUBSCRIBERS && (friendsIds.Contains(x.UserId) ||
                                                                         (targetType == TargetType.CHANNEL &&
                                                                          followedChannelsIds.Contains(x.TargetId))) ||
                        x.Privacy != Privacy.FRIENDS_AND_SUBSCRIBERS);
    }

    public static IQueryable<T> ApplyPrivacyPolicyByUser<T>(this IQueryable<T> query, TargetType? targetType,
        Guid userId, bool isFriend, List<Guid> followedChannelsIds)
        where T : IBasePost
    {
        return query.Where(x => x.UserId == userId)
            .Where(x => targetType == null || x.TargetType == targetType)
            .Where(x => x.Privacy == Privacy.FRIENDS_AND_SUBSCRIBERS && (isFriend ||
                                                                         (targetType == TargetType.CHANNEL &&
                                                                          followedChannelsIds.Contains(x.TargetId))) ||
                        x.Privacy != Privacy.FRIENDS_AND_SUBSCRIBERS);
    }

    public static IQueryable<T> ApplyPrivacyPolicyForSearch<T>(this IQueryable<T> query, List<Guid> friendsIds,
        List<Guid> followedChannelsIds)
        where T : IBasePost
    {
        return query.Where(x => x.TargetType == TargetType.CHANNEL)
            .Where(x => x.Privacy == Privacy.FRIENDS_AND_SUBSCRIBERS && 
                            (friendsIds.Contains(x.UserId) ||
                            (followedChannelsIds.Contains(x.TargetId))) ||
                            x.Privacy != Privacy.FRIENDS_AND_SUBSCRIBERS);
    }
}