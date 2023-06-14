namespace RPG.BuildingBlocks.Common.Middlewares.Cache
{
    public interface ICacheableData
    {
        string StoreName { get; set; }
        int TtlInSeconds { get; set; }
    }
}
