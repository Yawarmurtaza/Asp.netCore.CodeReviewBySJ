using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WordCount.Model;
using WordCount.ServiceManagers;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ConsoleRunner
{

    public class WebApiWrapper
    {
        public async Task<string> CallApi(int pageNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                // http://localhost:52779/api/LoyalBooksdata/4/bookName
                return await client.GetStringAsync($"http://localhost:52779/api/LoyalBooksdata/{pageNumber}/bookName");
            }
        }

        public IEnumerable<WordOccurance> ConvertIntoModelObject(string jsonString)
        {
            IEnumerable<WordOccurance> data = JsonConvert.DeserializeObject<IEnumerable<WordOccurance>>(jsonString);
            return data;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //ServiceCollection services = new ServiceCollection();

            //services.AddSingleton<IWebApiProcessor, WebApiProcessor>();
            //services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();
                                   
            //services.AddSingleton<IMemoryCacheWrapper, MemoryCacheWrapper>();
            //services.AddSingleton<ITextProcessor, TextProcessor>();
            //services.AddSingleton<IMemoryCacheWrapper, MemoryCacheWrapper>();

            //services.AddSingleton<IDependencyResolver, LoyalBookApiManagerResolver>();
            //services.AddSingleton<LoyalBooksWebApiParallelManager>();
            //services.AddSingleton<LoyalBooksWebApiManager>();


            //var serviceProvider = services.BuildServiceProvider();

            //IDependencyResolver resolver = serviceProvider.GetService<IDependencyResolver>();
            //IWebApiManager manager = resolver.GetWebApiManagerByName(typeof(LoyalBooksWebApiParallelManager));


            

            Task t = new Task(async () =>
            {
                WebApiWrapper api = new WebApiWrapper();
                int pageNumber = 1;
                while (Console.ReadLine() != "exit")
                {
                    string jsonString = await api.CallApi(pageNumber);
                    foreach (WordOccurance nextWord in api.ConvertIntoModelObject(jsonString))
                    {
                        Console.WriteLine($"{nextWord.Word}\t\t\t\t{nextWord.Count}");
                    }
                }
                
            });
            t.Start();
            t.Wait();

        }
    }

    public interface IDependencyResolver
    {
        IWebApiManager GetWebApiManagerByName(Type implementingType = null);
    }

    public class LoyalBookApiManagerResolver : IDependencyResolver
    {
        private readonly IServiceProvider services;

        public LoyalBookApiManagerResolver(IServiceProvider services)
        {
            this.services = services;
        }

        public IWebApiManager GetWebApiManagerByName(Type implementingType = null)
        {
            if (implementingType == null)
            {
                return this.services.GetService<LoyalBooksWebApiManager>();
            }

            if (string.Compare(implementingType.Name, "LoyalBooksWebApiParallelManager", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                return services.GetService<LoyalBooksWebApiParallelManager>();
            }

            return null;
        }
    }
}