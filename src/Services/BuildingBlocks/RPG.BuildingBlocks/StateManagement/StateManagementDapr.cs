using Dapr.Client;

namespace RPG.BuildingBlocks.Common.StateManagement
{
    public class StateManagementDapr : IStateManagement
    {
        private DaprClient _daprClient;

        public StateManagementDapr(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<T> GetStateAsync<T>(string storeName, string key)
        {
            return await _daprClient.GetStateAsync<T>(storeName, key);
        }

        public async Task<T> UpdateStateAsync<T>(string storeName, string key, T state)
        {
            var currentState = await _daprClient.GetStateEntryAsync<T>(storeName, key);
            currentState.Value = state;

            await currentState.SaveAsync();

            return await GetStateAsync<T>(storeName, key);
        }

        public async Task SaveStateAsync<T>(string storeName, string key, T state, int ttlInSeconds)
        {
            await _daprClient.SaveStateAsync(storeName, key, state, metadata: new Dictionary<string, string> { { "ttlInSeconds", ttlInSeconds.ToString() } });

        }

        public async Task DeleteStateAsync<T>(string storeName, string key)
        {
            await _daprClient.DeleteStateAsync(storeName, key);
        }
    }
}
