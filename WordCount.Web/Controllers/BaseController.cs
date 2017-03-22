using Microsoft.AspNetCore.Mvc;
using WordCount.Web.Infrastructure;

namespace WordCount.Web.Controllers
{
    public class BaseController : Controller
    {
        public ISessionWrapper Session { get; set; }   
        public BaseController(IServiceProviderWrapper services)
        {
            this.Session = services.GetRequiredService();
        }
    }
}