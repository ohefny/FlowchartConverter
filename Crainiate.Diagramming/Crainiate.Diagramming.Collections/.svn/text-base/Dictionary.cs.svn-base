
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Threading;

namespace Crainiate.Diagramming.Collections
{
    /// <summary>Represents a collection of keys and values.</summary>
    /// <filterpriority>1</filterpriority>
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false)]
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback
    {   
        private const string ComparerName = "Comparer";
        private const string KeyValuePairsName = "KeyValuePairs";
        private const string HashSizeName = "HashSize";
        private const string VersionName = "Version";

        private object _syncRoot;
        private int[] _buckets;
        private IEqualityComparer<TKey> _comparer;
        
        private int _count;
        private Entry[] _entries;
        private int _freeCount;
        private int _freeList;
        private KeyCollection _keys;
        private SerializationInfo _siInfo;
        private ValueCollection _values;
        private int _version;

        private bool _modifiable;

        //Constructors
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2"></see> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.</summary>
        public Dictionary()
            : this(0, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2"></see> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2"></see> and uses the default equality comparer for the key type.</summary>
        /// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        /// <exception cref="T:System.ArgumentException">dictionary contains one or more duplicate keys.</exception>
        /// <exception cref="T:System.ArgumentNullException">dictionary is null.</exception>
        public Dictionary(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2"></see> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see>.</summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"></see> for the type of the key.</param>
        public Dictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2"></see> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.</summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2"></see> can contain.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">capacity is less than 0.</exception>
        public Dictionary(int capacity)
            : this(capacity, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2"></see> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2"></see> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see>.</summary>
        /// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"></see> for the type of the key.</param>
        /// <exception cref="T:System.ArgumentException">dictionary contains one or more duplicate keys.</exception>
        /// <exception cref="T:System.ArgumentNullException">dictionary is null.</exception>
        public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : this((dictionary != null) ? dictionary.Count : 0, comparer)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                this.Add(pair.Key, pair.Value);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2"></see> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see>.</summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2"></see> can contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"></see> for the type of the key.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">capacity is less than 0.</exception>
        public Dictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            SetModifiable(true);

            if (capacity < 0) throw new ArgumentOutOfRangeException("capacity");
            if (capacity > 0) this.Initialize(capacity);
            if (comparer == null) comparer = EqualityComparer<TKey>.Default;

            this._comparer = comparer;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2"></see> class with serialized data.</summary>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        protected Dictionary(SerializationInfo info, StreamingContext context)
        {
            this._siInfo = info;
        }

        /// <summary>Adds the specified key and value to the dictionary.</summary>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</exception>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public virtual void Add(TKey key, TValue value)
        {   
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            this.Insert(key, value, true);
        }

        /// <summary>Removes all keys and values from the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
        public virtual void Clear()
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");

            if (this._count > 0)
            {
                for (int i = 0; i < this._buckets.Length; i++)
                {
                    this._buckets[i] = -1;
                }
                Array.Clear(this._entries, 0, this._count);
                this._freeList = -1;
                this._count = 0;
                this._freeCount = 0;
                this._version++;
            }
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2"></see> contains the specified key.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.Dictionary`2"></see> contains an element with the specified key; otherwise, false.</returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return (this.FindEntry(key) >= 0);
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2"></see> contains a specific value.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.Dictionary`2"></see> contains an element with the specified value; otherwise, false.</returns>
        /// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>. The value can be null for reference types.</param>
        public bool ContainsValue(TValue value)
        {
            if (value == null)
            {
                for (int i = 0; i < this._count; i++)
                {
                    if ((this._entries[i].hashCode >= 0) && (this._entries[i].value == null))
                    {
                        return true;
                    }
                }
            }
            else
            {
                EqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;
                for (int j = 0; j < this._count; j++)
                {
                    if ((this._entries[j].hashCode >= 0) && comparer.Equals(this._entries[j].value, value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null) throw new ArgumentNullException("array");
            if ((index < 0) || (index > array.Length)) throw new ArgumentOutOfRangeException("index");
            if ((array.Length - index) < this.Count) throw new ArgumentException();

            int count = this._count;
            Entry[] entries = this._entries;
            for (int i = 0; i < count; i++)
            {
                if (entries[i].hashCode >= 0)
                {
                    array[index++] = new KeyValuePair<TKey, TValue>(entries[i].key, entries[i].value);
                }
            }
        }

        private int FindEntry(TKey key)
        {
            if (key == null) throw new ArgumentNullException("key");

            if (this._buckets != null)
            {
                int num = this._comparer.GetHashCode(key) & 0x7fffffff;
                for (int i = this._buckets[num % this._buckets.Length]; i >= 0; i = this._entries[i].next)
                {
                    if ((this._entries[i].hashCode == num) && this._comparer.Equals(this._entries[i].key, key))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator"></see> structure for the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator((Dictionary<TKey, TValue>)this, 2);
        }

        /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable"></see> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"></see> instance.</summary>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2"></see> instance.</param>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"></see> instance.</param>
        /// <exception cref="T:System.ArgumentNullException">info is null.</exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");

            info.AddValue("Version", this._version);
            info.AddValue("Comparer", this._comparer, typeof(IEqualityComparer<TKey>));
            info.AddValue("HashSize", (this._buckets == null) ? 0 : this._buckets.Length);
            if (this._buckets != null)
            {
                KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this.Count];
                this.CopyTo(array, 0);
                info.AddValue("KeyValuePairs", array, typeof(KeyValuePair<TKey, TValue>[]));
            }
        }

        private void Initialize(int capacity)
        {
            int prime = HashHelpers.GetPrime(capacity);
            this._buckets = new int[prime];
            for (int i = 0; i < this._buckets.Length; i++)
            {
                this._buckets[i] = -1;
            }
            this._entries = new Entry[prime];
            this._freeList = -1;
        }

        private void Insert(TKey key, TValue value, bool add)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");

            int freeList;
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (this._buckets == null) this.Initialize(0);

            int num = this._comparer.GetHashCode(key) & 0x7fffffff;
            for (int i = this._buckets[num % this._buckets.Length]; i >= 0; i = this._entries[i].next)
            {
                if ((this._entries[i].hashCode == num) && this._comparer.Equals(this._entries[i].key, key))
                {
                    if (add) throw new ArgumentException("A duplicate key cannot be added to the dictionary");

                    this._entries[i].value = value;
                    this._version++;
                    return;
                }
            }
            if (this._freeCount > 0)
            {
                freeList = this._freeList;
                this._freeList = this._entries[freeList].next;
                this._freeCount--;
            }
            else
            {
                if (this._count == this._entries.Length)
                {
                    this.Resize();
                }
                freeList = this._count;
                this._count++;
            }
            int index = num % this._buckets.Length;
            this._entries[freeList].hashCode = num;
            this._entries[freeList].next = this._buckets[index];
            this._entries[freeList].key = key;
            this._entries[freeList].value = value;
            this._buckets[index] = freeList;
            this._version++;
        }

        private static bool IsCompatibleKey(object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return (key is TKey);
        }

        /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable"></see> interface and raises the deserialization event when the deserialization is complete.</summary>
        /// <param name="sender">The source of the deserialization event.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object associated with the current <see cref="T:System.Collections.Generic.Dictionary`2"></see> instance is invalid.</exception>
        public virtual void OnDeserialization(object sender)
        {
            if (this._siInfo != null)
            {
                int num = this._siInfo.GetInt32("Version");
                int num2 = this._siInfo.GetInt32("HashSize");
                this._comparer = (IEqualityComparer<TKey>)this._siInfo.GetValue("Comparer", typeof(IEqualityComparer<TKey>));
                if (num2 != 0)
                {
                    this._buckets = new int[num2];
                    for (int i = 0; i < this._buckets.Length; i++)
                    {
                        this._buckets[i] = -1;
                    }
                    this._entries = new Entry[num2];
                    this._freeList = -1;
                    KeyValuePair<TKey, TValue>[] pairArray = (KeyValuePair<TKey, TValue>[])this._siInfo.GetValue("KeyValuePairs", typeof(KeyValuePair<TKey, TValue>[]));
                    if (pairArray == null) throw new SerializationException("KeyValuePairs not found.");

                    for (int j = 0; j < pairArray.Length; j++)
                    {
                        if (pairArray[j].Key == null) throw new SerializationException("Null key exception.");

                        this.Insert(pairArray[j].Key, pairArray[j].Value, true);
                    }
                }
                else
                {
                    this._buckets = null;
                }
                this._version = num;
                this._siInfo = null;
            }
        }

        /// <summary>Removes the value with the specified key from the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
        /// <returns>true if the element is successfully found and removed; otherwise, false.  This method returns false if key is not found in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</returns>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public virtual bool Remove(TKey key)
        {
            if (!Modifiable) throw new CollectionException("The collection is not marked as modifiable.");
            if (key == null) throw new ArgumentNullException("key");

            if (this._buckets != null)
            {
                int num = this._comparer.GetHashCode(key) & 0x7fffffff;
                int index = num % this._buckets.Length;
                int num3 = -1;
                for (int i = this._buckets[index]; i >= 0; i = this._entries[i].next)
                {
                    if ((this._entries[i].hashCode == num) && this._comparer.Equals(this._entries[i].key, key))
                    {
                        if (num3 < 0)
                        {
                            this._buckets[index] = this._entries[i].next;
                        }
                        else
                        {
                            this._entries[num3].next = this._entries[i].next;
                        }
                        this._entries[i].hashCode = -1;
                        this._entries[i].next = this._freeList;
                        this._entries[i].key = default(TKey);
                        this._entries[i].value = default(TValue);
                        this._freeList = i;
                        this._freeCount++;
                        this._version++;
                        return true;
                    }
                    num3 = i;
                }
            }
            return false;
        }

        private void Resize()
        {
            int prime = HashHelpers.GetPrime(this._count * 2);
            int[] numArray = new int[prime];
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = -1;
            }
            Entry[] destinationArray = new Entry[prime];
            Array.Copy(this._entries, 0, destinationArray, 0, this._count);
            for (int j = 0; j < this._count; j++)
            {
                int index = destinationArray[j].hashCode % prime;
                destinationArray[j].next = numArray[index];
                numArray[index] = j;
            }
            this._buckets = numArray;
            this._entries = destinationArray;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            this.Add(keyValuePair.Key, keyValuePair.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            int index = this.FindEntry(keyValuePair.Key);
            return ((index >= 0) && EqualityComparer<TValue>.Default.Equals(this._entries[index].value, keyValuePair.Value));
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            this.CopyTo(array, index);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            int index = this.FindEntry(keyValuePair.Key);
            if ((index >= 0) && EqualityComparer<TValue>.Default.Equals(this._entries[index].value, keyValuePair.Value))
            {
                this.Remove(keyValuePair.Key);
                return true;
            }
            return false;
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Enumerator((Dictionary<TKey, TValue>)this, 2);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (array.Rank != 1) throw new ArgumentException("Array rank greater than one not supported");
            if (array.GetLowerBound(0) != 0) throw new ArgumentException("Non zero lower bound not supported.");
            if ((index < 0) || (index > array.Length)) throw new ArgumentOutOfRangeException("index", "Non negative index required.");

            if ((array.Length - index) < this.Count) throw new ArgumentException("Array offset too small");

            KeyValuePair<TKey, TValue>[] pairArray = array as KeyValuePair<TKey, TValue>[];
            if (pairArray != null)
            {
                this.CopyTo(pairArray, index);
            }
            else if (array is DictionaryEntry[])
            {
                DictionaryEntry[] entryArray = array as DictionaryEntry[];
                Entry[] entries = this._entries;
                for (int i = 0; i < this._count; i++)
                {
                    if (entries[i].hashCode >= 0)
                    {
                        entryArray[index++] = new DictionaryEntry(entries[i].key, entries[i].value);
                    }
                }
            }
            else
            {
                object[] objArray = array as object[];
                if (objArray == null) throw new ArgumentException("Invalid array type");

                try
                {
                    int count = this._count;
                    Entry[] entryArray3 = this._entries;
                    for (int j = 0; j < count; j++)
                    {
                        if (entryArray3[j].hashCode >= 0)
                        {
                            objArray[index++] = new KeyValuePair<TKey, TValue>(entryArray3[j].key, entryArray3[j].value);
                        }
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException("Invalid array type.");
                }
            }
        }

        void IDictionary.Add(object key, object value)
        {
            Dictionary<TKey, TValue>.VerifyKey(key);
            Dictionary<TKey, TValue>.VerifyValueType(value);
            this.Add((TKey)key, (TValue)value);
        }

        bool IDictionary.Contains(object key)
        {
            if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
            {
                return this.ContainsKey((TKey)key);
            }
            return false;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new Enumerator((Dictionary<TKey, TValue>)this, 1);
        }

        void IDictionary.Remove(object key)
        {
            if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
            {
                this.Remove((TKey)key);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator((Dictionary<TKey, TValue>)this, 2);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = this.FindEntry(key);
            if (index >= 0)
            {
                value = this._entries[index].value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        private static void VerifyKey(object key)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (!(key is TKey)) throw new ArgumentException("Type of key provided is not a compatible type.");

        }

        private static void VerifyValueType(object value)
        {
            if (!(value is TValue) && ((value != null) || typeof(TValue).IsValueType)) throw new ArgumentException("The type of value provided is not a compatible type.");
        }

        /// <summary>Gets the <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see> that is used to determine equality of keys for the dictionary. </summary>
        /// <returns>The <see cref="T:System.Collections.Generic.IEqualityComparer`1"></see> generic interface implementation that is used to determine equality of keys for the current <see cref="T:System.Collections.Generic.Dictionary`2"></see> and to provide hash values for the keys.</returns>
        public IEqualityComparer<TKey> Comparer
        {
            get
            {
                return this._comparer;
            }
        }

        /// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
        /// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</returns>
        public int Count
        {
            get
            {
                return (this._count - this._freeCount);
            }
        }

        /// <summary>Gets or sets the value associated with the specified key.</summary>
        /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException"></see>, and a set operation creates a new element with the specified key.</returns>
        /// <param name="key">The key of the value to get or set.</param>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key does not exist in the collection.</exception>
        public TValue this[TKey key]
        {
            get
            {
                int index = this.FindEntry(key);
                if (index >= 0)
                {
                    return this._entries[index].value;
                }
                throw new KeyNotFoundException();
                return default(TValue);
            }
            set
            {
                this.Insert(key, value, false);
            }
        }

        /// <summary>Gets a collection containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see> containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</returns>
        public KeyCollection Keys
        {
            get
            {
                if (this._keys == null)
                {
                    this._keys = new KeyCollection((Dictionary<TKey, TValue>)this);
                }
                return this._keys;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                if (this._keys == null)
                {
                    this._keys = new KeyCollection((Dictionary<TKey, TValue>)this);
                }
                return this._keys;
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                if (this._values == null)
                {
                    this._values = new ValueCollection((Dictionary<TKey, TValue>)this);
                }
                return this._values;
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
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Modifiable
        {
            get
            {
                return _modifiable;
            }
        }

        public void SetModifiable(bool value)
        {
            _modifiable = value;
        }

        object IDictionary.this[object key]
        {
            get
            {
                if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
                {
                    int index = this.FindEntry((TKey)key);
                    if (index >= 0)
                    {
                        return this._entries[index].value;
                    }
                }
                return null;
            }
            set
            {
                Dictionary<TKey, TValue>.VerifyKey(key);
                Dictionary<TKey, TValue>.VerifyValueType(value);
                this[(TKey)key] = (TValue)value;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return this.Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return this.Values;
            }
        }

        /// <summary>Gets a collection containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see> containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</returns>
        public ValueCollection Values
        {
            get
            {
                if (this._values == null)
                {
                    this._values = new ValueCollection((Dictionary<TKey, TValue>)this);
                }
                return this._values;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Entry
        {
            public int hashCode;
            public int next;
            public TKey key;
            public TValue value;
        }

        /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IDictionaryEnumerator, IEnumerator
        {
            internal const int DictEntry = 1;
            internal const int KeyValuePair = 2;
            private Dictionary<TKey, TValue> dictionary;
            private int version;
            private int index;
            private KeyValuePair<TKey, TValue> current;
            private int getEnumeratorRetType;
            internal Enumerator(Dictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
            {
                this.dictionary = dictionary;
                this.version = dictionary._version;
                this.index = 0;
                this.getEnumeratorRetType = getEnumeratorRetType;
                this.current = new KeyValuePair<TKey, TValue>();
            }

            /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public bool MoveNext()
            {
                if (this.version != this.dictionary._version) throw new InvalidOperationException("Invalid operation version mismatch");

                while (this.index < this.dictionary._count)
                {
                    if (this.dictionary._entries[this.index].hashCode >= 0)
                    {
                        this.current = new KeyValuePair<TKey, TValue>(this.dictionary._entries[this.index].key, this.dictionary._entries[this.index].value);
                        this.index++;
                        return true;
                    }
                    this.index++;
                }
                this.index = this.dictionary._count + 1;
                this.current = new KeyValuePair<TKey, TValue>();
                return false;
            }

            /// <summary>Gets the element at the current position of the enumerator.</summary>
            /// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2"></see> at the current position of the enumerator.</returns>
            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    return this.current;
                }
            }
            /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator"></see>.</summary>
            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get
                {
                    if ((this.index == 0) || (this.index == (this.dictionary._count + 1))) throw new InvalidOperationException("Invalid enumeration operation.");
                    if (this.getEnumeratorRetType == 1) return new DictionaryEntry(this.current.Key, this.current.Value);

                    return new KeyValuePair<TKey, TValue>(this.current.Key, this.current.Value);
                }
            }
            void IEnumerator.Reset()
            {
                if (this.version != this.dictionary._version) throw new InvalidOperationException("Invalid enumeration operation.");

                this.index = 0;
                this.current = new KeyValuePair<TKey, TValue>();
            }

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    if ((this.index == 0) || (this.index == (this.dictionary._count + 1))) throw new InvalidOperationException("Invalid enumeration operation.");

                    return new DictionaryEntry(this.current.Key, this.current.Value);
                }
            }
            object IDictionaryEnumerator.Key
            {
                get
                {
                    if ((this.index == 0) || (this.index == (this.dictionary._count + 1))) throw new InvalidOperationException("Invalid enumeration operation.");

                    return this.current.Key;
                }
            }
            object IDictionaryEnumerator.Value
            {
                get
                {
                    if ((this.index == 0) || (this.index == (this.dictionary._count + 1))) throw new InvalidOperationException("Invalid enumeration operation.");

                    return this.current.Value;
                }
            }
        }

        /// <summary>Represents the collection of keys in a <see cref="T:System.Collections.Generic.Dictionary`2"></see>. This class cannot be inherited.</summary>
        [Serializable, DebuggerDisplay("Count = {Count}")]
        public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
        {
            private Dictionary<TKey, TValue> dictionary;

            /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see> class that reflects the keys in the specified <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
            /// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2"></see> whose keys are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see>.</param>
            /// <exception cref="T:System.ArgumentNullException">dictionary is null.</exception>
            public KeyCollection(Dictionary<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                {
                    throw new ArgumentNullException("dictionary");
                }
                this.dictionary = dictionary;
            }

            /// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see> elements to an existing one-dimensional <see cref="T:System.Array"></see>, starting at the specified array index.</summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in array at which copying begins.</param>
            /// <exception cref="T:System.ArgumentNullException">array is null. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.</exception>
            /// <exception cref="T:System.ArgumentException">index is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see> is greater than the available space from index to the end of the destination array.</exception>
            public void CopyTo(TKey[] array, int index)
            {
                if (array == null) throw new ArgumentNullException("array");
                if ((index < 0) || (index > array.Length)) throw new ArgumentOutOfRangeException("index", "Index may not be negative.");
                if ((array.Length - index) < this.dictionary.Count) throw new ArgumentException("Array offset too small.");

                int count = this.dictionary._count;
                Dictionary<TKey, TValue>.Entry[] entries = this.dictionary._entries;
                for (int i = 0; i < count; i++)
                {
                    if (entries[i].hashCode >= 0)
                    {
                        array[index++] = entries[i].key;
                    }
                }
            }

            /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see>.</summary>
            /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator"></see> for the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see>.</returns>
            public Enumerator GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            void ICollection<TKey>.Add(TKey item)
            {
                throw new NotSupportedException();
            }

            void ICollection<TKey>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<TKey>.Contains(TKey item)
            {
                return this.dictionary.ContainsKey(item);
            }

            bool ICollection<TKey>.Remove(TKey item)
            {
                throw new NotSupportedException();
            }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            void ICollection.CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (array.Rank != 1)
                {
                    throw new ArgumentException("Rank greater than one not supported.");
                }
                if (array.GetLowerBound(0) != 0)
                {
                    throw new ArgumentException("Non-zero lower bound not supported.");
                }
                if ((index < 0) || (index > array.Length))
                {
                    throw new ArgumentOutOfRangeException("index", "Index may not be negative.");
                }
                if ((array.Length - index) < this.dictionary.Count)
                {
                    throw new ArgumentException("Invalid argument offset too small.");
                }
                TKey[] localArray = array as TKey[];
                if (localArray != null)
                {
                    this.CopyTo(localArray, index);
                }
                else
                {
                    object[] objArray = array as object[];
                    if (objArray == null)
                    {
                        throw new ArgumentException("Invalid array type.");
                    }
                    int count = this.dictionary._count;
                    Dictionary<TKey, TValue>.Entry[] entries = this.dictionary._entries;
                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (entries[i].hashCode >= 0)
                            {
                                objArray[index++] = entries[i].key;
                            }
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArgumentException("Invalid array type.");
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see>.</summary>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see>.Retrieving the value of this property is an O(1) operation.</returns>
            public int Count
            {
                get
                {
                    return this.dictionary.Count;
                }
            }

            bool ICollection<TKey>.IsReadOnly
            {
                get
                {
                    return true;
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
                    return ((ICollection)this.dictionary).SyncRoot;
                }
            }

            /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see>.</summary>
            [Serializable, StructLayout(LayoutKind.Sequential)]
            public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
            {
                private Dictionary<TKey, TValue> dictionary;
                private int index;
                private int version;
                private TKey currentKey;
                internal Enumerator(Dictionary<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    this.version = dictionary._version;
                    this.index = 0;
                    this.currentKey = default(TKey);
                }

                /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator"></see>.</summary>
                public void Dispose()
                {
                }

                /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see>.</summary>
                /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
                /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
                public bool MoveNext()
                {
                    if (this.version != this.dictionary._version)
                    {
                        throw new InvalidOperationException("Invalid enumeration operation.");
                    }
                    while (this.index < this.dictionary._count)
                    {
                        if (this.dictionary._entries[this.index].hashCode >= 0)
                        {
                            this.currentKey = this.dictionary._entries[this.index].key;
                            this.index++;
                            return true;
                        }
                        this.index++;
                    }
                    this.index = this.dictionary._count + 1;
                    this.currentKey = default(TKey);
                    return false;
                }

                /// <summary>Gets the element at the current position of the enumerator.</summary>
                /// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection"></see> at the current position of the enumerator.</returns>
                public TKey Current
                {
                    get
                    {
                        return this.currentKey;
                    }
                }
                object IEnumerator.Current
                {
                    get
                    {
                        if ((this.index == 0) || (this.index == (this.dictionary._count + 1)))
                        {
                            throw new InvalidOperationException("Invalid enumeration operation");
                        }
                        return this.currentKey;
                    }
                }
                void IEnumerator.Reset()
                {
                    if (this.version != this.dictionary._version)
                    {
                        throw new InvalidOperationException("Invalid enumeration operation.");
                    }
                    this.index = 0;
                    this.currentKey = default(TKey);
                }
            }
        }

        /// <summary>Represents the collection of values in a <see cref="T:System.Collections.Generic.Dictionary`2"></see>. This class cannot be inherited.</summary>
        [Serializable, DebuggerDisplay("Count = {Count}")]
        public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
        {
            private Dictionary<TKey, TValue> dictionary;

            /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see> class that reflects the values in the specified <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</summary>
            /// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2"></see> whose values are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see>.</param>
            /// <exception cref="T:System.ArgumentNullException">dictionary is null.</exception>
            public ValueCollection(Dictionary<TKey, TValue> dictionary)
            {
                if (dictionary == null)
                {
                    throw new ArgumentNullException("dictionary");
                }
                this.dictionary = dictionary;
            }

            /// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see> elements to an existing one-dimensional <see cref="T:System.Array"></see>, starting at the specified array index.</summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in array at which copying begins.</param>
            /// <exception cref="T:System.ArgumentException">index is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see> is greater than the available space from index to the end of the destination array.</exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.</exception>
            /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
            public void CopyTo(TValue[] array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if ((index < 0) || (index > array.Length))
                {
                    throw new ArgumentOutOfRangeException("index", "index may not be less than zero.");
                }
                if ((array.Length - index) < this.dictionary.Count)
                {
                    throw new ArgumentException("Array offset too small.");
                }
                int count = this.dictionary._count;
                Dictionary<TKey, TValue>.Entry[] entries = this.dictionary._entries;
                for (int i = 0; i < count; i++)
                {
                    if (entries[i].hashCode >= 0)
                    {
                        array[index++] = entries[i].value;
                    }
                }
            }

            /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see>.</summary>
            /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator"></see> for the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see>.</returns>
            public Enumerator GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            void ICollection<TValue>.Add(TValue item)
            {
                throw new NotSupportedException();
            }

            void ICollection<TValue>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<TValue>.Contains(TValue item)
            {
                return this.dictionary.ContainsValue(item);
            }

            bool ICollection<TValue>.Remove(TValue item)
            {
                throw new NotSupportedException();
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            void ICollection.CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (array.Rank != 1)
                {
                    throw new ArgumentException("Rank greater than one not supported.");
                }
                if (array.GetLowerBound(0) != 0)
                {
                    throw new ArgumentException("Non-zero lower bound not supported.");
                }
                if ((index < 0) || (index > array.Length))
                {
                    throw new ArgumentOutOfRangeException("index", "Index may not be less than zero.");
                }
                if ((array.Length - index) < this.dictionary.Count)
                {
                    throw new ArgumentException("Array offset too small.");
                }
                TValue[] localArray = array as TValue[];
                if (localArray != null)
                {
                    this.CopyTo(localArray, index);
                }
                else
                {
                    object[] objArray = array as object[];
                    if (objArray == null)
                    {
                        throw new ArgumentException("Invalid array type.");
                    }
                    int count = this.dictionary._count;
                    Dictionary<TKey, TValue>.Entry[] entries = this.dictionary._entries;
                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (entries[i].hashCode >= 0)
                            {
                                objArray[index++] = entries[i].value;
                            }
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArgumentException("Invalid array type.");
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see>.</summary>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see>.</returns>
            public int Count
            {
                get
                {
                    return this.dictionary.Count;
                }
            }

            bool ICollection<TValue>.IsReadOnly
            {
                get
                {
                    return true;
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
                    return ((ICollection)this.dictionary).SyncRoot;
                }
            }

            /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see>.</summary>
            [Serializable, StructLayout(LayoutKind.Sequential)]
            public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
            {
                private Dictionary<TKey, TValue> dictionary;
                private int index;
                private int version;
                private TValue currentValue;
                internal Enumerator(Dictionary<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    this.version = dictionary._version;
                    this.index = 0;
                    this.currentValue = default(TValue);
                }

                /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator"></see>.</summary>
                public void Dispose()
                {
                }

                /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see>.</summary>
                /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
                /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
                public bool MoveNext()
                {
                    if (this.version != this.dictionary._version)
                    {
                        throw new InvalidOperationException("Enumeration operation failed.");
                    }
                    while (this.index < this.dictionary._count)
                    {
                        if (this.dictionary._entries[this.index].hashCode >= 0)
                        {
                            this.currentValue = this.dictionary._entries[this.index].value;
                            this.index++;
                            return true;
                        }
                        this.index++;
                    }
                    this.index = this.dictionary._count + 1;
                    this.currentValue = default(TValue);
                    return false;
                }

                /// <summary>Gets the element at the current position of the enumerator.</summary>
                /// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection"></see> at the current position of the enumerator.</returns>
                public TValue Current
                {
                    get
                    {
                        return this.currentValue;
                    }
                }
                object IEnumerator.Current
                {
                    get
                    {
                        if ((this.index == 0) || (this.index == (this.dictionary._count + 1)))
                        {
                            throw new InvalidOperationException("Enumeration operation failed.");
                        }
                        return this.currentValue;
                    }
                }
                void IEnumerator.Reset()
                {
                    if (this.version != this.dictionary._version)
                    {
                        throw new InvalidOperationException("Enumeration operation failed.");
                    }
                    this.index = 0;
                    this.currentValue = default(TValue);
                }
            }
        }
    }

    internal static class HashHelpers
    {
        // Fields
        internal static readonly int[] primes = new int[] { 
        3, 7, 11, 0x11, 0x17, 0x1d, 0x25, 0x2f, 0x3b, 0x47, 0x59, 0x6b, 0x83, 0xa3, 0xc5, 0xef, 
        0x125, 0x161, 0x1af, 0x209, 0x277, 0x2f9, 0x397, 0x44f, 0x52f, 0x63d, 0x78b, 0x91d, 0xaf1, 0xd2b, 0xfd1, 0x12fd, 
        0x16cf, 0x1b65, 0x20e3, 0x2777, 0x2f6f, 0x38ff, 0x446f, 0x521f, 0x628d, 0x7655, 0x8e01, 0xaa6b, 0xcc89, 0xf583, 0x126a7, 0x1619b, 
        0x1a857, 0x1fd3b, 0x26315, 0x2dd67, 0x3701b, 0x42023, 0x4f361, 0x5f0ed, 0x72125, 0x88e31, 0xa443b, 0xc51eb, 0xec8c1, 0x11bdbf, 0x154a3f, 0x198c4f, 
        0x1ea867, 0x24ca19, 0x2c25c1, 0x34fa1b, 0x3f928f, 0x4c4987, 0x5b8b6f, 0x6dda89};

        // Methods
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal static int GetPrime(int min)
        {
            if (min < 0) throw new ArgumentException("Capacity overflow exception.");
            
            for (int i = 0; i < primes.Length; i++)
            {
                int num2 = primes[i];
                if (num2 >= min) return num2;
            }
            for (int j = min | 1; j < 0x7fffffff; j += 2)
            {
                if (IsPrime(j)) return j;
            }
            return min;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal static bool IsPrime(int candidate)
        {
            if ((candidate & 1) == 0) return (candidate == 2);
            
            int num = (int)Math.Sqrt((double)candidate);
            for (int i = 3; i <= num; i += 2)
            {
                if ((candidate % i) == 0) return false;
            }
            return true;
        }
    }
}


