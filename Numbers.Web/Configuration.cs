using System;
using System.Html;

namespace Numbers.Web
{
    public interface IConfiguration
    {
        string GetValue(string key);
        void SetValue(string key, string value, DateTime expiration);
    }

    public static class ConfigurationExtensions
    {
        public static void SetValue(this IConfiguration configuration, string key, string value)
        {
            configuration.SetValue(key, value, DateTime.Today.AddYears(1));
        }
    }

    public class Configuration : IConfiguration
    {
        private const string GmtTimeFormat = "ddd, dd MMM yyyy HH:mm:ss 'GMT'";

        public string GetValue(string key)
        {
            foreach (string keyValue in Document.Cookie.Split(";"))
            {
                int index = keyValue.IndexOf("=");

                string currentKey = keyValue.Substring(0, index).Trim();

                if (currentKey == key)
                {
                    return keyValue.Substring(index + 1);
                }
            }

            return String.Empty;
        }

        public void SetValue(string key, string value, DateTime expiration)
        {
            Document.Cookie = String.Format("{0}={1}; expires={2}", key, value, expiration.ToString(GmtTimeFormat));
        }
    }
}
