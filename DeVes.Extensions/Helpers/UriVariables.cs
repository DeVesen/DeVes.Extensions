using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeVes.Extensions.Helpers
{
    public class UriVariables : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly List<KeyValuePair<string, string>> m_elements = new List<KeyValuePair<string, string>>();

        public void Add(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            m_elements.Add(new KeyValuePair<string, string>(key, null));
        }
        public void Add(string key, object value)
        {
            Add(key, Convert.ToString(value));
        }
        public void Add(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (string.IsNullOrEmpty(value)) return;

            m_elements.Add(new KeyValuePair<string, string>(key, value));
        }

        public void Remove(KeyValuePair<string, string> item)
        {
            m_elements.Remove(item);
        }
        public void RemoveAt(int index)
        {
            if (index < 0) return;
            if (index >= m_elements.Count) return;

            m_elements.RemoveAt(index);
        }

        public string GetFinal()
        {
            return "?" + ToString();
        }

        public override string ToString()
        {
            var _elements =
                m_elements.Select(p => string.IsNullOrEmpty(p.Value) ? p.Key : $"{p.Key}={p.Value}");

            var _sb = new StringBuilder();
            foreach (var _item in _elements)
            {
                if (_sb.Length > 0)
                {
                    _sb.Append("&");
                }
                _sb.Append(_item);
            }
            return _sb.ToString();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return m_elements.GetEnumerator();
        }
    }
}
