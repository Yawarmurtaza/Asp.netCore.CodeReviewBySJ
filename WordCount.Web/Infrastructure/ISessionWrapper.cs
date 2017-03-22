namespace WordCount.Web.Infrastructure
{
    public interface ISessionWrapper
    {
        string GetString(string key);
        void SetString(string key, string value);
    }
}