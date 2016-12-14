using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Crainiate.Diagramming.Collections
{
    [Serializable, DebuggerDisplay("Count = {Count}")]
    public class List<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        // Fields
        private const int _defaultCapacity = 4;

        private static T[] _emptyArray;
        private T[] _items;
        private int _size;
        [NonSerialized] private object _syncRoot;
        private int _version;

        private bool _modifiable;

        // Constructors
        static List()
        {
            List<T>._emptyArray = new T[0];
        }

        public List()
        {
            SetModifiable(true);
            this._items = List<T>._emptyArray;
        }

        public List(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            SetModifiable(true);
            
            ICollection<T> is2 = collection as ICollection<T>;
            if (is2 != null)
            {
                int count = is2.Count;
                this._items = new T[count];
                is2.CopyTo(this._items, 0);
                this._size = count;
            }
            else
            {
                this._size = 0;
                this._items = new T[4];
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        this.Add(enumerator.Current);
                    }
                }
            }
        }

        public List(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException("capacity", "Capacity may not be less than zero.");
            SetModifiable(true);
            
            this._items = new T[capacity];
        }

        //Properties
        public bool Modifiable
        {
            get
            {
                return _modifiable;
            }
        }

        public int Capacity
        {
            get
            {
                return this._items.Length;
            }
            set
            {
                if (value != this._items.Length)
                {
                    if (value < this._size) throw new ArgumentOutOfRangeException("The capacity does not match the size required.");

                    if (value > 0)
                    {
                        T[] destinationArray = new T[value];
                        if (this._size > 0)
                        {
                            Array.Copy(this._items, 0, destinationArray, 0, this._size);
                        }
                        this._items = destinationArray;
                    }
                    else
                    {
                        this._items = List<T>._emptyArray;
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                return this._size;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index >= this._size) throw new ArgumentOutOfRangeException();

                return this._items[index];
            }
            set
            {
                if (index >= this._size) throw new ArgumentOutOfRangeException();

                this._items[index] = value;
                this._version++;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null) Interlocked.CompareExchange(ref this._syncRoot, new object(), null);

                return this._syncRoot;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                List<T>.VerifyValueType(value);
                this[index] = (T)value;
            }
        }

        //Methods
        public void Add(T item)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            if (this._size == this._items.Length) this.EnsureCapacity(this._size + 1);
            
            this._items[this._size++] = item;
            this._version++;

            OnInserted(item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            this.InsertRange(this._size, collection);
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            return new ReadOnlyCollection<T>(this);
        }

        public int BinarySearch(T item)
        {
            return this.BinarySearch(0, this.Count, item, null);
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return this.BinarySearch(0, this.Count, item, comparer);
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            if ((index < 0) || (count < 0)) throw new ArgumentOutOfRangeException("The index or count parameter may not be less than zero.");
            if ((this._size - index) < count) throw new ArgumentException("Invalid offset provided.");
            
            return Array.BinarySearch<T>(this._items, index, count, item, comparer);
        }

        public virtual void Clear()
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            Array.Clear(this._items, 0, this._size);
            this._size = 0;
            this._version++;
        }

        public bool Contains(T item)
        {
            if (item == null)
            {
                for (int j = 0; j < this._size; j++)
                {
                    if (this._items[j] == null)
                    {
                        return true;
                    }
                }
                return false;
            }
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < this._size; i++)
            {
                if (comparer.Equals(this._items[i], item))
                {
                    return true;
                }
            }
            return false;
        }

        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            if (converter == null) throw new ArgumentNullException("converter");
            
            List<TOutput> list = new List<TOutput>(this._size);
            for (int i = 0; i < this._size; i++)
            {
                list._items[i] = converter(this._items[i]);
            }
            list._size = this._size;
            return list;
        }

        public void CopyTo(T[] array)
        {
            this.CopyTo(array, 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this._items, 0, array, arrayIndex, this._size);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if ((this._size - index) < count) throw new ArgumentException("An invalid index offset was provided.");
            
            Array.Copy(this._items, index, array, arrayIndex, count);
        }

        private void EnsureCapacity(int min)
        {
            if (this._items.Length < min)
            {
                int num = (this._items.Length == 0) ? 4 : (this._items.Length * 2);
                if (num < min) num = min;
                
                this.Capacity = num;
            }
        }

        public bool Exists(Predicate<T> match)
        {
            return (this.FindIndex(match) != -1);
        }

        public T Find(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException("match");
            
            for (int i = 0; i < this._size; i++)
            {
                if (match(this._items[i]))
                {
                    return this._items[i];
                }
            }
            return default(T);
        }

        public List<T> FindAll(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException("match");
            
            List<T> list = new List<T>();
            for (int i = 0; i < this._size; i++)
            {
                if (match(this._items[i])) list.Add(this._items[i]);
            }
            return list;
        }

        public int FindIndex(Predicate<T> match)
        {
            return this.FindIndex(0, this._size, match);
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return this.FindIndex(startIndex, this._size - startIndex, match);
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            if (startIndex > this._size) throw new ArgumentOutOfRangeException("startIndex is greater than the size of the collection.");
            if ((count < 0) || (startIndex > (this._size - count))) throw new ArgumentOutOfRangeException("Count paramter is out of the range of the collection.");
            if (match == null) throw new ArgumentNullException("match");
            
            int num = startIndex + count;
            for (int i = startIndex; i < num; i++)
            {
                if (match(this._items[i])) return i;
            }
            return -1;
        }

        public T FindLast(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException("match");
            
            for (int i = this._size - 1; i >= 0; i--)
            {
                if (match(this._items[i])) return this._items[i];
            }
            return default(T);
        }

        public int FindLastIndex(Predicate<T> match)
        {
            return this.FindLastIndex(this._size - 1, this._size, match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return this.FindLastIndex(startIndex, startIndex + 1, match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException("match");
            
            if (this._size == 0)
            {
                if (startIndex != -1) throw new ArgumentOutOfRangeException("startIndex");
            }
            else if (startIndex >= this._size)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            if ((count < 0) || (((startIndex - count) + 1) < 0))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            int num = startIndex - count;
            for (int i = startIndex; i > num; i--)
            {
                if (match(this._items[i])) return i;
            }
            return -1;
        }

        public void ForEach(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException("match");
            
            for (int i = 0; i < this._size; i++)
            {
                action(this._items[i]);
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator((List<T>)this);
        }

        public List<T> GetRange(int index, int count)
        {
            if ((index < 0) || (count < 0)) throw new ArgumentOutOfRangeException("Index or count may not be ess than zero.");
            if ((this._size - index) < count) throw new ArgumentException("Invalid offset.");
            
            List<T> list = new List<T>(count);
            Array.Copy(this._items, index, list._items, 0, count);
            list._size = count;
            return list;
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf<T>(this._items, item, 0, this._size);
        }

        public int IndexOf(T item, int index)
        {
            if (index > this._size) throw new ArgumentOutOfRangeException("Index");
            
            return Array.IndexOf<T>(this._items, item, index, this._size - index);
        }

        public int IndexOf(T item, int index, int count)
        {
            if (index > this._size) throw new ArgumentOutOfRangeException("index");
            if ((count < 0) || (index > (this._size - count))) throw new ArgumentOutOfRangeException("count");
            
            return Array.IndexOf<T>(this._items, item, index, count);
        }

        public void Insert(int index, T item)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            if (index > this._size) throw new ArgumentOutOfRangeException("Index may not be greater than the size of the collection.");
            if (this._size == this._items.Length) this.EnsureCapacity(this._size + 1);
            if (index < this._size) Array.Copy(this._items, index, this._items, index + 1, this._size - index);
            
            this._items[index] = item;
            this._size++;
            this._version++;

            OnInserted(item);
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            if (collection == null) throw new ArgumentNullException("collection");
            if (index > this._size) throw new ArgumentOutOfRangeException("Index may not be greater than the size of the collection.");
            
            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    this.Insert(index++, enumerator.Current);
                }
            }

            this._version++;
        }

        private static bool IsCompatibleObject(object value)
        {
            if (!(value is T) && ((value != null) || typeof(T).IsValueType)) return false;
            return true;
        }

        public int LastIndexOf(T item)
        {
            return this.LastIndexOf(item, this._size - 1, this._size);
        }

        public int LastIndexOf(T item, int index)
        {
            if (index >= this._size) throw new ArgumentOutOfRangeException("Index may not be greater than the collection size.");
            
            return this.LastIndexOf(item, index, index + 1);
        }

        public int LastIndexOf(T item, int index, int count)
        {
            if (this._size == 0) return -1;
            if ((index < 0) || (count < 0)) throw new ArgumentOutOfRangeException("Index or count may not be less than zero.");
            
            if ((index >= this._size) || (count > (index + 1))) throw new ArgumentOutOfRangeException("Index or count may not be larger than the collection size.");
            
            return Array.LastIndexOf<T>(this._items, item, index, count);
        }

        public bool Remove(T item)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            int index = this.IndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }
            return false;
        }

        public int RemoveAll(Predicate<T> match)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            if (match == null) throw new ArgumentNullException("match");
            
            int index = 0;
            while ((index < this._size) && !match(this._items[index]))
            {
                index++;
            }
            if (index >= this._size) return 0;
            
            int num2 = index + 1;
            while (num2 < this._size)
            {
                while ((num2 < this._size) && match(this._items[num2]))
                {
                    num2++;
                }
                if (num2 < this._size)
                {
                    this._items[index++] = this._items[num2++];
                }
            }
            Array.Clear(this._items, index, this._size - index);
            int num3 = this._size - index;
            this._size = index;
            this._version++;
            return num3;
        }

        public void RemoveAt(int index)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            if (index >= this._size) throw new ArgumentOutOfRangeException();

            OnRemove(this._items[index]);
            
            this._size--;
            if (index < this._size)
            {
                Array.Copy(this._items, index + 1, this._items, index, this._size - index);
            }
            this._items[this._size] = default(T);
            this._version++;

            
        }

        public void RemoveRange(int index, int count)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            if ((index < 0) || (count < 0)) throw new ArgumentOutOfRangeException("Count or inde may not be less than zero.");
            if ((this._size - index) < count) throw new ArgumentException("Invalid offset.");
            
            if (count > 0)
            {
                this._size -= count;
                if (index < this._size) Array.Copy(this._items, index + count, this._items, index, this._size - index);
                
                Array.Clear(this._items, this._size, count);
                this._version++;
            }
        }

        public void Reverse()
        {
            this.Reverse(0, this.Count);
        }

        public void Reverse(int index, int count)
        {
            if ((index < 0) || (count < 0)) throw new ArgumentOutOfRangeException();
            if ((this._size - index) < count) throw new ArgumentException("Invalid offset.");
            
            Array.Reverse(this._items, index, count);
            this._version++;
        }

        public virtual void Sort()
        {
            this.Sort(0, this.Count, null);
        }

        public virtual void Sort(IComparer<T> comparer)
        {
            this.Sort(0, this.Count, comparer);
        }

        public virtual void Sort(int index, int count, IComparer<T> comparer)
        {
            if ((index < 0) || (count < 0)) throw new ArgumentOutOfRangeException("Index and count may not be less than zero.");
            if ((this._size - index) < count) throw new ArgumentException("Invalid offset.");
            
            Array.Sort<T>(this._items, index, count, comparer);
            this._version++;
        }

        public virtual void SetModifiable(bool value)
        {
            _modifiable = value;
        }

        protected internal virtual void OnInserted(T item)
        {

        }

        protected internal virtual void OnRemove(T item)
        {

        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator((List<T>)this);
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            if ((array != null) && (array.Rank != 1)) throw new ArgumentException("Multi dimensional arrays are not supported.");
            
            try
            {
                Array.Copy(this._items, 0, array, arrayIndex, this._size);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("An argument type mismatch exception occurred.");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator((List<T>)this);
        }

        int IList.Add(object item)
        {
            List<T>.VerifyValueType(item);
            this.Add((T)item);
            return (this.Count - 1);
        }

        bool IList.Contains(object item)
        {
            if (List<T>.IsCompatibleObject(item)) return this.Contains((T)item);
            
            return false;
        }

        int IList.IndexOf(object item)
        {
            if (List<T>.IsCompatibleObject(item)) return this.IndexOf((T)item);
            
            return -1;
        }

        void IList.Insert(int index, object item)
        {
            List<T>.VerifyValueType(item);
            this.Insert(index, (T)item);
        }

        void IList.Remove(object item)
        {
            if (List<T>.IsCompatibleObject(item)) this.Remove((T)item);
        }

        public T[] ToArray()
        {
            T[] destinationArray = new T[this._size];
            Array.Copy(this._items, 0, destinationArray, 0, this._size);
            return destinationArray;
        }

        public void TrimExcess()
        {
            int num = (int)(this._items.Length * 0.9);
            if (this._size < num) this.Capacity = this._size;
        }

        public bool TrueForAll(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException("match");
            
            for (int i = 0; i < this._size; i++)
            {
                if (!match(this._items[i])) return false;
            }
            return true;
        }

        private static void VerifyValueType(object value)
        {
            if (!List<T>.IsCompatibleObject(value)) throw new ArgumentException("The argument type was not compatible.");
        }



        // Nested Types
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private List<T> list;
            private int index;
            private int version;
            private T current;
            internal Enumerator(List<T> list)
            {
                this.list = list;
                this.index = 0;
                this.version = list._version;
                this.current = default(T);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (this.version != this.list._version) throw new InvalidOperationException("Enumeration failed due to version mismatch.");
                
                if (this.index < this.list._size)
                {
                    this.current = this.list._items[this.index];
                    this.index++;
                    return true;
                }
                this.index = this.list._size + 1;
                this.current = default(T);
                return false;
            }

            public T Current
            {
                get
                {
                    return this.current;
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    if ((this.index == 0) || (this.index == (this.list._size + 1))) throw new InvalidOperationException("Enumeration was not successful.");
                    
                    return this.Current;
                }
            }
            void IEnumerator.Reset()
            {
                if (this.version != this.list._version) throw new InvalidOperationException("Enumeration version mismatch.");
                
                this.index = 0;
                this.current = default(T);
            }
        }
    }
}
