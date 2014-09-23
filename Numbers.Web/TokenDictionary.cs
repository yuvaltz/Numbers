using System;
using System.Collections.Generic;
using System.Linq;

namespace Numbers.Web
{
    public class TokenDictionary
    {
        private const char TokenSeparator = ',';
        private const char KeyValueSeparator = ' ';

        private Func<string> getRawList;
        private Action<string> setRawList;

        public TokenDictionary(Func<string> getRawList, Action<string> setRawList)
        {
            this.getRawList = getRawList;
            this.setRawList = setRawList;
        }

        public void Set(string key, string value)
        {
            IDictionary<string, string> dictionary = CreateDictionary(getRawList());
            dictionary[key] = value;
            setRawList(CreateRawList(dictionary));
        }

        public bool Contains(string key)
        {
            IDictionary<string, string> dictionary = CreateDictionary(getRawList());
            return dictionary.ContainsKey(key);
        }

        public void Clear(string key)
        {
            IDictionary<string, string> dictionary = CreateDictionary(getRawList());
            dictionary.Remove(key);
            setRawList(CreateRawList(dictionary));
        }

        private static IDictionary<string, string> CreateDictionary(string rawList)
        {
            return rawList.Split(TokenSeparator).Where(token => !String.IsNullOrEmpty(token)).ToDictionary(GetTokenKey, GetTokenValue);
        }

        private static string CreateRawList(IDictionary<string, string> dictionary)
        {
            return dictionary.Keys.
                Select(key => String.Format("{0}{1}{2}", key, KeyValueSeparator.ToString(), dictionary[key])).
                DefaultIfEmpty(String.Empty).Aggregate((s1, s2) => String.Format("{0}{1} {2}", s1, TokenSeparator.ToString(), s2));
        }

        private static string GetTokenKey(string token)
        {
            token = token.TrimStart();
            int index = token.IndexOf(KeyValueSeparator);
            return index == -1 ? token : token.Substring(0, index);
        }

        private static string GetTokenValue(string token)
        {
            token = token.TrimStart();
            int index = token.IndexOf(KeyValueSeparator);
            return index == -1 ? String.Empty : token.Substring(index + 1).TrimStart(KeyValueSeparator);
        }
    }
}
