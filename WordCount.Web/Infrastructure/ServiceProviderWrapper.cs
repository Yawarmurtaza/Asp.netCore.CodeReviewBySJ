using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WordCount.Web.Infrastructure;

namespace WordCount.Web.Controllers.Infrastructure
{
    public class ServiceProviderWrapper : IServiceProviderWrapper
    {
        private readonly IServiceProvider services;

        public ServiceProviderWrapper(IServiceProvider services)
        {
            this.services = services;
        }
        public ISessionWrapper GetRequiredService()
        {
            ISession session = this.services.GetRequiredService<IHttpContextAccessor>() == null ? null : services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
            ISessionWrapper sessionWrapper = new SessionWrapper(session); // need to avoid "new" keyword here.... can do with Setter injection.
            return sessionWrapper;
        }
    }
}