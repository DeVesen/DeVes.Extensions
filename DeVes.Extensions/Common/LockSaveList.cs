using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DeVes.Extensions.Common
{
    public class LockSaveList<T> : ICollection<T>, IReadOnlyList<T>
    {
        private readonly List<T> m_innerExpandableObjects = new List<T>();


        public T this[int index]
        {
            get
            {
                lock (this)
                {
                    return m_innerExpandableObjects[index];
                }
            }
            set
            {
                lock (this)
                {
                    m_innerExpandableObjects[index] = value;
                }
            }
        }


        public int Count
        {
            get
            {
                lock (this)
                {
                    return m_innerExpandableObjects.Count;
                }
            }
        }
        public int Capacity
        {
            get
            {
                lock (this)
                {
                    return m_innerExpandableObjects.Capacity;
                }
            }
            set
            {
                lock (this)
                {
                    m_innerExpandableObjects.Capacity = value;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                lock (this)
                {
                    return false;
                }
            }
        }


        public void Add(T item)
        {
            lock (this)
            {
                m_innerExpandableObjects.Add(item);
            }
        }
        public void AddRange(IEnumerable<T> collection)
        {
            lock (this)
            {
                m_innerExpandableObjects.AddRange(collection);
            }
        }
        public ReadOnlyCollection<T> AsReadOnly()
        {
            lock (this)
            {
                return m_innerExpandableObjects.AsReadOnly();
            }
        }
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            lock (this)
            {
                return m_innerExpandableObjects.BinarySearch(index, count, item, comparer);
            }
        }
        public int BinarySearch(T item)
        {
            lock (this)
            {
                return m_innerExpandableObjects.BinarySearch(item);
            }
        }
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            lock (this)
            {
                return m_innerExpandableObjects.BinarySearch(item, comparer);
            }
        }
        public void Clear()
        {
            lock (this)
            {
                m_innerExpandableObjects.Clear();
            }
        }
        public bool Contains(T item)
        {
            lock (this)
            {
                return m_innerExpandableObjects.Contains(item);
            }
        }
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            lock (this)
            {
                return m_innerExpandableObjects.ConvertAll(converter);
            }
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this)
            {
                m_innerExpandableObjects.CopyTo(array, arrayIndex);
            }
        }
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            lock (this)
            {
                m_innerExpandableObjects.CopyTo(index, array, arrayIndex, count);
            }
        }
        public void CopyTo(T[] array)
        {
            lock (this)
            {
                m_innerExpandableObjects.CopyTo(array);
            }
        }
        public bool Exists(Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.Exists(match);
            }
        }
        public T Find(Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.Find(match);
            }
        }
        public List<T> FindAll(Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.FindAll(match);
            }
        }
        public int FindIndex(Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.FindIndex(match);
            }
        }
        public int FindIndex(int startIndex, Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.FindIndex(startIndex, match);
            }
        }
        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.FindIndex(startIndex, count, match);
            }
        }
        public T FindLast(Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.FindLast(match);
            }
        }
        public int FindLastIndex(Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.FindLastIndex(match);
            }
        }
        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.FindLastIndex(startIndex, match);
            }
        }
        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.FindLastIndex(startIndex, count, match);
            }
        }
        public void ForEach(Action<T> action)
        {
            lock (this)
            {
                m_innerExpandableObjects.ForEach(action);
            }
        }
        public List<T> GetRange(int index, int count)
        {
            lock (this)
            {
                return m_innerExpandableObjects.GetRange(index, count);
            }
        }
        public int IndexOf(T item, int index, int count)
        {
            lock (this)
            {
                return m_innerExpandableObjects.IndexOf(item, index, count);
            }
        }
        public int IndexOf(T item, int index)
        {
            lock (this)
            {
                return m_innerExpandableObjects.IndexOf(item, index);
            }
        }
        public int IndexOf(T item)
        {
            lock (this)
            {
                return m_innerExpandableObjects.IndexOf(item);
            }
        }
        public void Insert(int index, T item)
        {
            lock (this)
            {
                m_innerExpandableObjects.Insert(index, item);
            }
        }
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            lock (this)
            {
                m_innerExpandableObjects.InsertRange(index, collection);
            }
        }
        public int LastIndexOf(T item)
        {
            lock (this)
            {
                return m_innerExpandableObjects.LastIndexOf(item);
            }
        }
        public int LastIndexOf(T item, int index)
        {
            lock (this)
            {
                return m_innerExpandableObjects.LastIndexOf(item, index);
            }
        }
        public int LastIndexOf(T item, int index, int count)
        {
            lock (this)
            {
                return m_innerExpandableObjects.LastIndexOf(item, index, count);
            }
        }
        public bool Remove(T item)
        {
            lock (this)
            {
                return m_innerExpandableObjects.Remove(item);
            }
        }
        public int RemoveAll(Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.RemoveAll(match);
            }
        }
        public void RemoveAt(int index)
        {
            lock (this)
            {
                m_innerExpandableObjects.RemoveAt(index);
            }
        }
        public void RemoveRange(int index, int count)
        {
            lock (this)
            {
                m_innerExpandableObjects.RemoveRange(index, count);
            }
        }
        public void Reverse(int index, int count)
        {
            lock (this)
            {
                m_innerExpandableObjects.Reverse(index, count);
            }
        }
        public void Reverse()
        {
            lock (this)
            {
                m_innerExpandableObjects.Reverse();
            }
        }
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            lock (this)
            {
                m_innerExpandableObjects.Sort(index, count, comparer);
            }
        }
        public void Sort(Comparison<T> comparison)
        {
            lock (this)
            {
                m_innerExpandableObjects.Sort(comparison);
            }
        }
        public void Sort()
        {
            lock (this)
            {
                m_innerExpandableObjects.Sort();
            }
        }
        public void Sort(IComparer<T> comparer)
        {
            lock (this)
            {
                m_innerExpandableObjects.Sort(comparer);
            }
        }
        public T[] ToArray()
        {
            lock (this)
            {
                return m_innerExpandableObjects.ToArray();
            }
        }
        public void TrimExcess()
        {
            lock (this)
            {
                m_innerExpandableObjects.TrimExcess();
            }
        }
        public bool TrueForAll(Predicate<T> match)
        {
            lock (this)
            {
                return m_innerExpandableObjects.TrueForAll(match);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (this)
            {
                return m_innerExpandableObjects.GetEnumerator();
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            lock (this)
            {
                return m_innerExpandableObjects.GetEnumerator();
            }
        }
    }
}
