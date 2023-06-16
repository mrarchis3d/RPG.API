using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RPG.BuildingBlocks.Common.BaseContext
{
    public class BaseUser
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    //Need this since Bson doesnt support hidden properties
    public class BaseUserRelational : BaseUser
    {
        public Guid UserId { get; set; }
    }

    public class BaseUserNoSql : BaseUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
    }
}
