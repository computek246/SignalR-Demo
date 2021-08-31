using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRDemo.Client
{
    public class SignalRConnection
    {
        public HubConnection Connection;

        public async Task StartAsync(string myAccessToken)
        {
            const string url = "https://localhost:44379/hubs/notifications";

            Connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    options.CloseTimeout = TimeSpan.MaxValue;
                    options.AccessTokenProvider = () => Task.FromResult(myAccessToken);
                    //options.UseDefaultCredentials = true;
                })
                .WithAutomaticReconnect()
                .ConfigureLogging(logging =>
                {
                    //logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                    //logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);

                    //// Log to the Console
                    //logging.AddConsole();

                    //// This will set ALL logging to Debug level
                    //logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();

            // receive a message from the hub
            Connection.On<string, string>("ReceiveMessage", OnReceiveMessage);

            
            await Connection.StartAsync();
            
            Console.WriteLine($"ConnectionId: {Connection.ConnectionId}");

            // send a message to the hub
            await Connection.InvokeAsync("SendMessage", "ConsoleApp", "Message from the console app");

        }


        private static void OnReceiveMessage(string user, string message)
        {
            Console.WriteLine($"{user}:");
            Console.WriteLine($" - {message}");
        }

    }
}
