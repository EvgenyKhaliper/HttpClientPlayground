﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        private static string URL = "http://localhost:5000/api/Test/1";

        static void Main(string[] args)
        {
            var start = 0;
            var end = 400;
            
            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    var startNew = Stopwatch.StartNew();
                    
                    if(input.Equals("n", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("new client go");
                        var tasks = Enumerable.Range(start, end).Select(i => HttpGet());
                        Task.WhenAll(tasks)
                            .GetAwaiter()
                            .GetResult();
                    }
                    else if(input.Equals("s", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("same client go");
                        using (var client = new HttpClient())
                        {
                            var tasks = Enumerable.Range(start, end).Select(i => HttpGet(client));
                            Task.WhenAll(tasks)
                                .GetAwaiter()
                                .GetResult();
                        }
                    }
                    startNew.Stop();
                    Console.WriteLine($"ended in: {startNew.Elapsed.TotalSeconds} " +
                                      $"\nrate is {end / startNew.Elapsed.TotalSeconds} req/sec");
                }
            }
        }
        
        private static async Task HttpGet()
        {
            using (var client = new HttpClient())
            {
                await HttpGet(client);
            }
        }

        private static async Task HttpGet(HttpClient httpClient)
        {
            var res = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, URL));

            if (res.StatusCode == HttpStatusCode.NotFound) return;

            var content = await res.Content.ReadAsStringAsync();

            throw new Exception($"{res.StatusCode}: {content}");
        }
    }
}