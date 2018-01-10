using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DeVes.Extensions.Common;

namespace DeVes.Extensions
{
    public static class EnvirementExtentions
    {
        public static DateTime GetMonthStart(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }
        public static DateTime? GetMonthStart(this DateTime? value)
        {
            return value?.GetMonthStart();
        }


        public static T ConvTo<T>(this object value, T defaultValue = default(T))
        {
            return (T)value.ConvTo(typeof(T), defaultValue);
        }
        public static object ConvTo(this object value, Type targetType, object defaultValue)
        {
            if (value == null)
                return defaultValue;

            if (targetType == typeof(object))
                return value;

            if (targetType == value.GetType())
                return value;

            if (targetType.IsTypeOf(typeof(string)))
            {
                return Convert.ToString(value);
            }

            if (targetType.IsTypeOf(typeof(short), typeof(short?)))
            {
                return short.TryParse(value.ConvTo<string>(), out short _result) ? _result : defaultValue;
            }
            if (targetType.IsTypeOf(typeof(int), typeof(int?)))
            {
                return int.TryParse(value.ConvTo<string>(), out int _result) ? _result : defaultValue;
            }
            if (targetType.IsTypeOf(typeof(long), typeof(long?)))
            {
                return long.TryParse(value.ConvTo<string>(), out long _result) ? _result : defaultValue;
            }

            if (targetType.IsTypeOf(typeof(float), typeof(float?)))
            {
                return float.TryParse(value.ConvTo<string>(), out float _result) ? _result : defaultValue;
            }
            if (targetType.IsTypeOf(typeof(double), typeof(double?)))
            {
                return double.TryParse(value.ConvTo<string>(), out double _result) ? _result : defaultValue;
            }

            if (targetType.IsTypeOf(typeof(byte), typeof(byte?)))
            {
                return byte.TryParse(value.ConvTo<string>(), out byte _result) ? _result : defaultValue;
            }
            if (targetType.IsTypeOf(typeof(bool), typeof(bool?)))
            {
                return bool.TryParse(value.ConvTo<string>(), out bool _result) ? _result : defaultValue;
            }

            if (targetType.IsTypeOf(typeof(DateTime), typeof(DateTime?)))
            {
                return DateTime.TryParse(value.ConvTo<string>(), out DateTime _result) ? _result : defaultValue;
            }
            if (targetType.IsTypeOf(typeof(Guid), typeof(Guid?)))
            {
                return Guid.TryParse(value.ConvTo<string>(), out Guid _result) ? _result : defaultValue;
            }

            return defaultValue;
        }


        public static bool IsTypeOf<T>(params Type[] references)
        {
            return typeof(T).IsTypeOf(references);
        }
        public static bool IsTypeOf(this Type type, params Type[] compareTo)
        {
            if (compareTo == null) return false;

            if (compareTo.Contains(type)) return true;

            var _parents = type.GetParentTypes().ToArray();

            return _parents.Any() && compareTo.Any(compType => _parents.Contains(compType));
        }

        public static IEnumerable<Type> GetParentTypes(this Type type)
        {
            // is there any base type?
            if (type == null || type.BaseType == null)
            {
                yield break;
            }

            // return all implemented or inherited interfaces
            foreach (var _i in type.GetInterfaces())
            {
                yield return _i;
            }

            // return all inherited types
            var _currentBaseType = type.BaseType;
            while (_currentBaseType != null)
            {
                yield return _currentBaseType;
                _currentBaseType = _currentBaseType.BaseType;
            }
        }



        public static T GetPropertyValue<T>(this object host, string path)
        {
            var _splitWords = " !\"§$%&/()=?´`\\}][{+*~#'-.,;:_<>|".Select(p => p.ToString()).ToArray();
            return host.GetPropertyValue<T>(path.Split(_splitWords, StringSplitOptions.RemoveEmptyEntries));
        }
        private static T GetPropertyValue<T>(this object host, IEnumerable<string> keys, T defaultValue = default(T))
        {
            var _valueHost = host;
            var _keyLst = (keys as string[] ?? new string[0]).ToList();
            if (!_keyLst.Any()) return defaultValue;


            while (_valueHost != null && _keyLst.Count > 0)
            {
                var _hostProperties = _valueHost.GetType().GetProperties();
                if (!_hostProperties.Any()) return defaultValue;

                var _localProperty =
                    _hostProperties.FirstOrDefault(
                        p => string.Compare(p.Name, _keyLst[0], StringComparison.OrdinalIgnoreCase) == 0);
                if (_localProperty == null) return defaultValue;

                _keyLst.RemoveAt(0);

                _valueHost = _localProperty.GetValue(_valueHost, null);
            }

            return _valueHost.ConvTo<T>();
        }

        public static void SetPropertyValue<T>(this object host, string path, T value)
        {
            var _splitWords = " !\"§$%&/()=?´`\\}][{+*~#'-.,;:_<>|".Select(p => p.ToString()).ToArray();
            host.SetPropertyValue(path.Split(_splitWords, StringSplitOptions.RemoveEmptyEntries), value);
        }
        private static void SetPropertyValue<T>(this object host, IEnumerable<string> keys, T value)
        {
            var _valueHost = host;
            var _keyLst = (keys as string[] ?? new string[0]).ToList();
            if (!_keyLst.Any()) return;

            while (_valueHost != null && _keyLst.Count > 0)
            {
                var _hostProperties = _valueHost.GetType().GetProperties();
                if (!_hostProperties.Any()) return;

                var _localProperty =
                    _hostProperties.FirstOrDefault(
                        p => string.Compare(p.Name, _keyLst[0], StringComparison.OrdinalIgnoreCase) == 0);
                if (_localProperty == null) return;

                _keyLst.RemoveAt(0);

                if (_keyLst.Count > 0)
                    _valueHost = _localProperty.GetValue(_valueHost, null);
                else
                    _localProperty.SetValue(_valueHost, value);
            }
        }




        public static void DisposeDataTable(ref DataTable table)
        {
            if (table == null) return;

            var _dataset = table.DataSet;
            if (_dataset != null)
            {
                for (var _relationIndex = 0; _relationIndex < _dataset.Relations.Count; _relationIndex++)
                {
                    if (_dataset.Relations[_relationIndex].ParentTable == table || _dataset.Relations[_relationIndex].ChildTable == table)
                    {
                        var _relationName = _dataset.Relations[_relationIndex].RelationName;
                        var _childTable = _dataset.Relations[_relationIndex].ChildTable;
                        for (var _childIndex = 0; _childIndex < _childTable.Constraints.Count; _childIndex++)
                            if (_childTable.Constraints[_childIndex].ConstraintName == _relationName && _childTable.Constraints[_childIndex].GetType() == typeof(ForeignKeyConstraint))
                                _childTable.Constraints.RemoveAt(_childIndex);

                        _dataset.Relations.RemoveAt(_relationIndex);
                    }
                }

                table.DataSet.Tables.Remove(table);
                if (_dataset.Tables.Count <= 0)
                {
                    _dataset.Tables.Clear();
                    _dataset.Dispose();
                }
            }

            table.Clear();
            table.Dispose();
            table = null;
        }
        public static void DisposeMe(this DataTable table)
        {
            DisposeDataTable(ref table);
        }

        public static void DisposeDataSet(ref DataSet set)
        {
            if (set == null) return;

            set.Reset();
            set.Relations.Clear();
            set.Clear();

            foreach (DataTable _table in set.Tables)
            {
                _table.Dispose();
            }

            set.Tables.Clear();
            set.Dispose();
            set = null;
        }
        public static void DisposeMe(this DataSet set)
        {
            DisposeDataSet(ref set);
        }



        public static ulong CreateHashCode(this object obj)
        {
            ulong _hash = 0;
            var _objType = obj.GetType();

            if (_objType.IsValueType || obj is string)
            {
                unchecked
                {
                    _hash = (uint)obj.GetHashCode() * 397;
                }

                return _hash;
            }

            if (obj is DynamicExpandableObject)
            {
                return (uint)((DynamicExpandableObject)obj).GetHashCode();
            }

            unchecked
            {
                _hash = obj.GetType()
                    .GetProperties()
                    .Select(property => property.GetValue(obj, null))
                    .Aggregate(_hash, (current, value) => current ^ value.CreateHashCode());
            }

            return _hash;
        }
    }
}
