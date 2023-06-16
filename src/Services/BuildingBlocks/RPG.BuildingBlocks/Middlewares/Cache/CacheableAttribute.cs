using System;
namespace RPG.BuildingBlocks.Common.Middlewares.Cache
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CacheableAttribute : Attribute, ICacheableData
    {
        public string StoreName { get; set; }
        public int TtlInSeconds { get; set; }

        public CacheableAttribute(string storeName, int ttlInSeconds)
        {
            if (storeName is null)
            {
                throw new ArgumentNullException(nameof(storeName));
            }
            StoreName = storeName;
            TtlInSeconds = ttlInSeconds;
        }

    }
}