using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WordCount.Model;
using WordCount.ServiceManagers.Interfaces;

namespace WordCount.ServiceManagers
{
    public abstract class BaseLoyalBooksWebApiManager : IWebApiManager
    {
        private readonly IOptions<MyConfig> config;
        private readonly IWebApiProcessor apiProcessor;
        protected readonly IMemoryCacheWrapper cache;
        protected readonly MemoryCacheEntryOptions cacheEntryOptions;        

        protected BaseLoyalBooksWebApiManager(IWebApiProcessor apiProcessor, IMemoryCacheWrapper cache, IOptions<MyConfig> config)
        {
            this.apiProcessor = apiProcessor;
            this.config = config;
            //this.apiProcessor.ApiPath = "download/text/";
            //this.apiProcessor.WebLocation = "http://www.loyalbooks.com/";
            this.apiProcessor.ApiPath = this.config.Value.ApiPath;
            this.apiProcessor.WebLocation = this.config.Value.HostServerUrl;


            this.cache = cache;
            this.cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
        }

        public abstract Task<IEnumerable<WordOccurance>> GetIndivisualWordsCount(string bookName);

        protected async Task<string> GetBookText(string bookName)
        {
            private string bookText;
            this.apiProcessor.ApiPath += bookName;

            if (!this.cache.TryGetValue(this.apiProcessor.ApiPath, out bookText))
            {
                this.bookText = await this.apiProcessor.GetStringAsync();
                this.cache.Set(this.apiProcessor.ApiPath, this.bookText, this.cacheEntryOptions);
            }

            return bookText;
        }
    }
}
