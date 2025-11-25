using PriceFeedAPI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ODINMarketFeed.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var client = new ODINMarketFeedClient())
            {
                client.OnOpen += async () => {

                    List<string> tokenList = new List<string>();
                    tokenList.Add("1_22");
                    tokenList.Add("1_2885");
                    //String[] strTokenArr = T")
                    //Subscribe to TL Request
                    //await client.SubscribeTouchlineAsync(tokenList);
                    //await client.SubscribeTouchlineAsync(tokenList, "1");
                    await client.SubscribeTouchlineAsync(tokenList, "0", true);
                    //await client.SubscribeBestFiveAsync("22", 1);
                    //await client.SubscribeLTPTouchlineAsync(tokenList);
                    Thread.Sleep(15000);
                    await client.SubscribePauseResumeAsync(true);
                    Thread.Sleep(5000);
                    await client.SubscribePauseResumeAsync(false);


                };

                client.OnMessage += (msg) =>
                {
                    if (msg is MarketData md)
                    {
                        Console.WriteLine($"Market Data: Token={md.Token}, LTP={md.LTP}");
                    }
                    else if (msg is string str)
                    {
                        Console.WriteLine($"Message: {str}");
                    }
                };

                client.OnError += (err) => Console.WriteLine($"Error: {err}");
                client.OnClose += (code, msg) => Console.WriteLine($"Closed: {code} - {msg}");

                client.SetCompression(true);
                await client.ConnectAsync("172.25.100.43", 4509, false, "USER123", "");

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();

                await client.DisconnectAsync();
            }
        }
    }
}
