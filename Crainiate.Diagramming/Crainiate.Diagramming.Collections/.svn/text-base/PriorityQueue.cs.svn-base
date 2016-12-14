// (c) Copyright Crainiate Software 2010




using System;
using System.Collections.Generic;

namespace Crainiate.Diagramming.Collections
{
	internal class PriorityQueue<T>
	{
        private List<T> _innerList = new List<T>();
		private IComparer<T> _comparer;
        private bool _fifo;

		public PriorityQueue()
		{
			_comparer = System.Collections.Generic.Comparer<T>.Default;
            _fifo = true;
		}

		public PriorityQueue(IComparer<T> c)
		{
			_comparer = c;
            _fifo = true;
		}

        protected List<T> InnerList
        {
            get
            {
                return _innerList;
            }
        }

		protected void SwitchElements(int i, int j)
		{
			T h = _innerList[i];
			_innerList[i] = _innerList[j];
			_innerList[j] = h;
		}

		protected virtual int Compare(int i, int j)
		{
			return _comparer.Compare(_innerList[i],_innerList[j]);
		}

		// Push an object onto the PQ
		// Returns the index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.
		public int Push(T O)
		{
			int p = _innerList.Count, p2;

			_innerList.Add(O); // E[p] = O
		
			while (true)
			{
				if (p==0) break;
				p2 = (p-1) / 2;
			
                int result = Compare(p, p2);
				if (result < 0 || (_fifo && result == 0)) //Move ahead of equals if smaller, or equal and fifo
				{
					SwitchElements(p, p2);
					p = p2;
				}
				else
				{
					break;
				}
			}

			return p;
		}

		// Get the smallest object and remove it.
		public T Pop()
		{
			T result = _innerList[0];
			int p = 0, p1, p2, pn;

			_innerList[0] = _innerList[_innerList.Count-1];
			_innerList.RemoveAt(_innerList.Count-1);
		
			while (true)
			{
				pn = p;
				p1 = 2*p + 1;
				p2 = 2*p + 2;
				if (_innerList.Count > p1 && Compare(p,p1) > 0) p = p1; 
				if (_innerList.Count > p2 && Compare(p,p2) > 0) p = p2;
			
				if (p == pn) break;
				SwitchElements(p, pn);
			}
			return result;
		}

		// Get the smallest object without removing it.
		public T Peek()
		{
			if (_innerList.Count > 0) return _innerList[0];
			return default(T);
		}

		public void Update(T node)
		{
			//Get the index of the node into i
			int i = _innerList.BinarySearch(node);
			if (i < 0) return;

			int p = i,pn;
			int p1, p2;
			
			while (true)
			{
				if(p==0) break;
				p2 = (p-1)/2;
				
				if(Compare(p,p2) < 0)
				{
					SwitchElements(p,p2);
					p = p2;
				}
				else
				{
					break;
				}
			}

			if (p < i) return;
			
			while (true)
			{
				pn = p;
				p1 = 2*p+1;
				p2 = 2*p+2;
				if (_innerList.Count > p1 && Compare(p,p1) > 0) p = p1;
				if (_innerList.Count > p2 && Compare(p,p2) > 0) p = p2;
				
				if(p==pn) break;
				SwitchElements(p,pn);
			}
		}


		public bool Contains(T value)
		{
			return _innerList.Contains(value);
		}

		public void Clear()
		{
			_innerList.Clear();
		}

		public int Count
		{
			get
			{
				return _innerList.Count;
			}
		}

		public void CopyTo(T[] array, int index)
		{
			_innerList.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}
	}
}


