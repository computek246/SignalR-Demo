using System;
using System.Threading.Tasks;

namespace SignalRDemo.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            var hub = new SignalRConnection();
            var api = new ApiConnection();

            Task.Run(async () =>
            {
                // Do any async anything you need here without worry
                var response = await api.LoginAsync();

                if (!response.IsAuthenticated)
                    return;

                await hub.StartAsync(response.Token);

                await api.AddNotificationAsync();

            });

            Console.WriteLine(new string('-', 60));


            Console.Read();

        }
    }
}
