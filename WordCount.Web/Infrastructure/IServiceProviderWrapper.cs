
namespace WordCount.Web.Infrastructure
{
    public interface IServiceProviderWrapper
    {
        ISessionWrapper GetRequiredService();
    }
}