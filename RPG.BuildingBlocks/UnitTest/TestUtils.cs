using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RPG.BuildingBlocks.Common.UnitTest
{
    public class TestUtils
    {
        public async Task<string> GetAccessTokenAsync(string scope)
        {
            var client = new HttpClient();
            var values = new Dictionary<string, string>
            {
               { "client_id", "flutter" },
               { "client_secret", "SuperSecretPassword" },
               { "grant_type", "password" },
               { "scope", scope },
               { "username", "bob" },
               { "password", "Pass123$" },
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://api.ourglass.cc/connect/token", content);

            var jsonContent = await response.Content.ReadAsStringAsync();
            Token token = JsonConvert.DeserializeObject<Token>(jsonContent);

            return token.AccessToken;
        }

        public async Task<string> GetInternalAccessTokenAsync(string scope)
        {
            var client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "client_id", "internal" },
                { "client_secret", "AnotherSuperSecretPassword" },
                { "grant_type", "client_credentials" },
                { "scope", scope },
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.ourglass.cc/connect/token", content);

            var jsonContent = await response.Content.ReadAsStringAsync();
            Token token = JsonConvert.DeserializeObject<Token>(jsonContent);

            return token.AccessToken;
        }
    }

    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
