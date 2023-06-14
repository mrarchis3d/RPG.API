using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.StateManagement
{
    public interface IStateManagement
    {
        Task<T> GetStateAsync<T>(string storeName, string key);

        Task<T> UpdateStateAsync<T>(string storeName, string key, T state);

        Task DeleteStateAsync<T>(string storeName, string key);

        Task SaveStateAsync<T>(string storeName, string key, T state, int ttlInSeconds);
    }
}
