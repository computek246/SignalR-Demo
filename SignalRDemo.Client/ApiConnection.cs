using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SignalRDemo.Client
{
    public class ApiConnection
    {
        private const string ApiUrl = "https://localhost:44379/api";

        public string Token { get; set; }



        public async Task<AuthenticateResponse> LoginAsync()
        {
            using var client = new WebClient();
            client.Headers.Add("Content-Type:application/json");
            client.Headers.Add("Accept:application/json");

            var loginVm = new
            {
                Username = "test",
                Password = "test"
            };

            var result = await client.UploadStringTaskAsync($"{ApiUrl}/Users/Authenticate", JsonConvert.SerializeObject(loginVm));
            var resultObj = JsonConvert.DeserializeObject<AuthenticateResponse>(result);
            Token = resultObj?.Token;

            Console.WriteLine($"Token: {Token}");

            return resultObj;
        }

        public async Task AddNotificationAsync()
        {
            using var client = new WebClient();
            client.Headers.Add("Content-Type:application/json");
            client.Headers.Add("Accept:application/json");
            client.Headers.Add($"Authorization: {Token}");

            var result = await client.DownloadStringTaskAsync($"{ApiUrl}/Users/Private");

            Console.WriteLine(result);
        }
    }
}
