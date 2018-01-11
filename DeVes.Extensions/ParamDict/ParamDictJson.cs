using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeVes.Extensions.ParamDict
{
    public class PdValue
    {
        public object Value { get; set; }

        public bool HasValue => Value != null;
        public bool IsParamDict => AsParamDict() != null;


        public PdValue(object value)
        {
            Value = value;
        }


        public ParamDict AsParamDict()
        {
            return (ParamDict) this;
        }


        public static PdValue Empty() { return new PdValue(null); }



        public static implicit operator PdValue(JToken value)
        {
            return new PdValue(value?.Value<object>());
        }
        
        public static implicit operator PdValue(Guid? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(short? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(ushort? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(uint? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(ulong? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(double value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(float value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(string value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(uint value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(ulong value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(byte[] value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(Uri value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(TimeSpan value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(TimeSpan? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(Guid value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(decimal value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(float? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(bool? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(DateTime value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(long? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(DateTimeOffset value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(byte value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(byte? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(sbyte value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(sbyte? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(long value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(bool value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(DateTimeOffset? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(decimal? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(double? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(short value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(ushort value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(int value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(int? value)
        {
            return new PdValue(value);
        }
        public static implicit operator PdValue(DateTime? value)
        {
            return new PdValue(value);
        }


        public static explicit operator ParamDict(PdValue value)
        {
            if (value == null) return null;
            if (!value.HasValue) return null;

            var _paramDict = value.Value as ParamDict;
            if (_paramDict != null) return _paramDict;

            var _jobject = value.Value as JObject;
            if (_jobject != null)
                return new ParamDict(_jobject);
            
            return null;
        }
    }

    public class PdToken : PdValue
    {
        public string Key { get; set; }

        internal PdToken(string key, object value)
            : base(value)
        {
            Key = key;
        }
    }


    public class ParamDict : IReadOnlyList<PdToken>
    {
        private JObject m_parameters;


        public JObject Parameters
        {
            get
            {
                lock (this)
                {
                    return m_parameters.DeepClone() as JObject;
                }
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                lock (this)
                {
                    m_parameters = value;
                }
            }
        }
        public int Count => ToArray().Length;
        public PdValue this[string path]
        {
            get { return GetKey(path); }
            set { SetKey(path, value);}
        }
        public PdToken this[int index]
        {
            get
            {
                var _array = ToArray();
                return index < 0 || _array.Length <= index ? null : _array[index];
            }
        }
        public string[] PathSeparator { get; set; }


        public ParamDict()
        {
            PathSeparator = new[] {".", "\\", "/"};
            m_parameters = new JObject();
        }
        public ParamDict(JObject jobject) : this()
        {
            m_parameters = jobject ?? m_parameters;
        }
        public ParamDict(object parameters) : this()
        {
            if (parameters is JObject)
                m_parameters = parameters as JObject;

            else if (parameters is string)
                ReadJson(parameters.ConvTo<string>());

            else if (parameters != null &&
                (parameters.GetType().IsTypeAnonymous() || parameters.GetType().IsAbstract || parameters.GetType().IsClass ||
                 parameters.GetType().IsAnsiClass || parameters.GetType().IsInterface || parameters.GetType().IsArray))
                m_parameters = JObject.Parse(JsonConvert.SerializeObject(parameters));
        }


        public bool ExistKey(string path)
        {
            return !string.IsNullOrEmpty(path) && ExistKey(PathToKeys(path));
        }
        public bool ExistKey(string[] keys)
        {
            throw new NotImplementedException();
        }

        public PdValue GetKey(string path)
        {
            return string.IsNullOrEmpty(path) ? null : GetKey(PathToKeys(path));
        }
        public PdValue GetKey(string[] keys)
        {
            var _token = GetObjectByKey(m_parameters, keys);
            return _token;
        }

        public bool TryGetKey(string path, out PdValue value)
        {
            value = PdValue.Empty();

            if (string.IsNullOrEmpty(path)) return false;
            var _keys = path.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries);
            return TryGetKey(_keys, out value);
        }
        public bool TryGetKey(string[] keys, out PdValue value)
        {
            throw new NotImplementedException();
        }

        public void SetKey(string path, PdValue value)
        {
            if (string.IsNullOrEmpty(path)) return;
            var _keys = path.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries);
            SetKey(_keys, value);
        }
        public void SetKey(string[] keys, PdValue value)
        {
            if (value == null) return;

            if (value.Value.GetType().IsTypeAnonymous())
            {
                SetObjectByKey(keys, JObject.Parse(JsonConvert.SerializeObject(value.Value)));
                return;
            }

            SetObjectByKey(keys, new JValue(value.Value));
        }

        public void RemoveKey(string path)
        {
            RemoveKey(PathToKeys(path));
        }
        public void RemoveKey(string[] keys)
        {
            RemoveObjectByKey(keys);
        }


        public bool ReadJson(string json)
        {
            if (string.IsNullOrEmpty(json)) return false;

            lock (this)
            {
                try
                {
                    m_parameters = JObject.Parse(json);
                    return true;
                }
                catch
                {
                    m_parameters = new JObject();
                }
            }

            return false;
        }
        public bool ReadFile(string file)
        {
            lock (this)
            {
                bool _positiv;

                using (var _file = File.OpenText(file))
                using (var _jReader = new JsonTextReader(_file))
                {
                    try
                    {
                        m_parameters = (JObject)JToken.ReadFrom(_jReader);
                        _positiv = true;
                    }
                    catch
                    {
                        m_parameters = new JObject();
                        _positiv = false;
                    }
                    _file.Close();
                }

                return _positiv;
            }
        }
        public void WriteToFile(string file)
        {
            lock (this)
            {
                using (var _file = new StreamWriter(file))
                {
                    _file.WriteLine(ToJson());
                    _file.Close();
                }
            }
        }


        public ParamDict Clone()
        {
            lock (this)
            {
                return new ParamDict(JObject.Parse(JsonConvert.SerializeObject(m_parameters)));
            }
        }
        public JObject ToJObject()
        {
            lock (this)
            {
                return Clone().m_parameters;
            }
        }


        public PdToken[] ToArray()
        {
            lock (this)
            {
                var _array =
                    m_parameters.PropertyValues()
                        .Select(p => new PdToken(p.Path, new PdValue(p.Value<object>())))
                        .ToArray();

                return _array;
            }
        }
        public List<PdToken> ToList()
        {
            lock (this)
            {
                var _lst =
                    m_parameters.PropertyValues()
                        .Select(p => new PdToken(p.Path, new PdValue(p.Value<object>())))
                        .ToList();

                return _lst;
            }
        }
        public IEnumerator GetEnumerator()
        {
            return ToArray().GetEnumerator();
        }
        IEnumerator<PdToken> IEnumerable<PdToken>.GetEnumerator()
        {
            return ToList().GetEnumerator();
        }


        public override string ToString()
        {
            return ToJson();
        }
        public string ToJson(bool indentedFormated = true)
        {
            lock (this)
            {
                return JsonConvert.SerializeObject(m_parameters, indentedFormated ? Formatting.Indented : Formatting.None);
            }
        }


        private JToken GetObjectByKey(JToken token, string[] keys)
        {
            lock (this)
            {
                for (var _index = 0; _index < keys.Length && token != null; _index++)
                {
                    var _segment = keys[_index];

                    token = token[_segment];
                }

                return token;
            }
        }
        private void SetObjectByKey(string[] path, JToken value)
        {
            lock (this)
            {
                var _token = m_parameters;
                for (var _i = 0; _i < path.Length - 1; _i++)
                {
                    if (_token[path[_i]] == null)
                    {
                        _token[path[_i]] = new JObject();
                    }
                    _token = (JObject)_token[path[_i]];
                }

                _token = _token ?? m_parameters;

                _token[path[path.Length - 1]] = value;
            }
        }
        private void RemoveObjectByKey(string[] path)
        {
            lock (this)
            {
                var _token = m_parameters;
                for (var _i = 0; _i < path.Length - 1; _i++)
                {
                    if (_token[path[_i]] == null)
                    {
                        _token[path[_i]] = new JObject();
                    }
                    _token = (JObject)_token[path[_i]];
                }

                _token = _token ?? m_parameters;

                _token.Remove(path[path.Length - 1]);
            }
        }


        protected virtual string[] PathToKeys(string path)
        {
            return path.Split(new[] { ".", "\\", "/" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
