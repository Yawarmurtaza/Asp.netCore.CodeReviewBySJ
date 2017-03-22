using Microsoft.AspNetCore.Http;

namespace WordCount.Web.Infrastructure
{
    public class SessionWrapper : ISessionWrapper
    {
        private readonly ISession session;
        public SessionWrapper(ISession session)
        {
            this.session = session;
        }
        public string GetString(string key)
        {
            return this.session.GetString(key);
        }

        public void SetString(string key, string value)
        {
            this.session.SetString(key, value);
        }
    }
}