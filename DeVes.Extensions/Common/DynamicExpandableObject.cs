using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace DeVes.Extensions.Common
{
    public class DynamicExpandableObject : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly dynamic m_dynCollItem;


        public object this[string propertyName]
        {
            get
            {
                return Get(propertyName);
            }
            set
            {
                Set(propertyName, value);
            }
        }

        public int PropertyCount
        {
            get
            {
                lock (this)
                {
                    var _dict = m_dynCollItem as IDictionary<string, object>;
                    if (_dict == null) return -1;
                    return _dict.Count;
                }
            }
        }


        public DynamicExpandableObject()
        {
            m_dynCollItem = new ExpandoObject();
        }
        public DynamicExpandableObject(ExpandoObject dynCollection)
        {
            m_dynCollItem = dynCollection;
        }



        public void Add(string propertyName, object value)
        {
            lock (m_dynCollItem)
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    return;
                }

                var _dict = m_dynCollItem as IDictionary<string, object>;

                if (_dict == null) return;

                _dict.Add(propertyName, value);
            }
        }
        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }
        public void AddRange(IEnumerable<KeyValuePair<string, object>> collection)
        {
            lock (m_dynCollItem)
            {
                foreach (var _item in collection)
                {
                    Add(_item);
                }
            }
        }

        public void Set(string propertyName, object value)
        {
            lock (m_dynCollItem)
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    return;
                }

                var _dict = m_dynCollItem as IDictionary<string, object>;

                if (_dict == null) return;

                _dict[propertyName] = value;
            }
        }
        public void Set(KeyValuePair<string, object> item)
        {
            Set(item.Key, item.Value);
        }
        public void SetRange(IEnumerable<KeyValuePair<string, object>> collection)
        {
            lock (m_dynCollItem)
            {
                foreach (var _item in collection)
                {
                    Set(_item);
                }
            }
        }

        public object Get(string propertyName)
        {
            lock (m_dynCollItem)
            {
                var _dict = m_dynCollItem as IDictionary<string, object>;

                if (_dict == null) return null;
                return !_dict.ContainsKey(propertyName) ? null : _dict[propertyName];
            }
        }
        public bool TryGet(string propertyName, out object value)
        {
            lock (m_dynCollItem)
            {
                var _dict = m_dynCollItem as IDictionary<string, object>;

                value = null;

                if (_dict == null) return false;
                if (!_dict.ContainsKey(propertyName)) return false;

                value = _dict[propertyName];

                return true;
            }
        }

        public bool Remove(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return false;
            }

            lock (m_dynCollItem)
            {
                var _dict = m_dynCollItem as IDictionary<string, object>;
                if (_dict == null) return false;

                if (!_dict.ContainsKey(propertyName))
                {
                    return false;
                }

                _dict.Remove(propertyName);

                return true;
            }
        }

        public dynamic GetItem()
        {
            lock (m_dynCollItem)
            {
                return m_dynCollItem;
            }
        }


        public override string ToString()
        {
            lock (m_dynCollItem)
            {
                return JsonConvert.SerializeObject(m_dynCollItem, Formatting.Indented);
            }
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            var _hash = base.GetHashCode();

            unchecked
            {
                lock (m_dynCollItem)
                {
                    var _dict = m_dynCollItem as IDictionary<string, object>;
                    if (_dict == null) return _hash;

                    foreach (var _property in _dict)
                    {
                        var _key = (int)_property.Key.CreateHashCode();
                        var _value = (int)_property.Value.CreateHashCode();

                        _hash ^= _key;
                        _hash ^= _value;
                    }
                }
            }

            return _hash;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (m_dynCollItem)
            {
                var _dict = m_dynCollItem as IDictionary<string, object>;

                return _dict?.GetEnumerator() ?? new KeyValuePair<string, object>[0].GetEnumerator();
            }
        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            lock (m_dynCollItem)
            {
                var _dict = m_dynCollItem as IDictionary<string, object>;

                if (_dict == null)
                {
                    throw new NotImplementedException("Not Supported");
                }

                return _dict.GetEnumerator();
            }
        }
    }


    public class ExpandableObjectCollection : LockSaveList<DynamicExpandableObject>
    {
    }
}
