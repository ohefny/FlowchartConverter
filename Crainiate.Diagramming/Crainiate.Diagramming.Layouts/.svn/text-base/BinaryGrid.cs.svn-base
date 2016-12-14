// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

namespace Crainiate.Diagramming.Layouts
{
    internal sealed class BinaryGrid<T>
    {
        private Dictionary<Point, T> _internalList;
        private Point _lookupPoint;

        public BinaryGrid()
        {
            _internalList = new Dictionary<Point, T>();
            _lookupPoint = new Point();
        }

        //Returns a item. Returns null if not found
        public T Get(int x, int y)
        {
            _lookupPoint.X = x;
            _lookupPoint.Y = y;

            T t = default(T);
            _internalList.TryGetValue(_lookupPoint, out t);

            return t;
        }

        //Returns a item. Throws null if not found
        public T Item(int x, int y)
        {
            _lookupPoint.X = x;
            _lookupPoint.Y = y;
            return _internalList[_lookupPoint];
        }

        //Adds an item if not found
        public void Add(int x, int y, T t)
        {
            _lookupPoint.X = x;
            _lookupPoint.Y = y;
            _internalList.Add(_lookupPoint, t);
        }

        public void Combine(int x, int y, T t)
        {
            _lookupPoint.X = x;
            _lookupPoint.Y = y;
            if (!_internalList.ContainsKey(_lookupPoint)) _internalList.Add(_lookupPoint, t);
        }

        public bool IsEmpty
        {
            get
            {
                return (_internalList.Count == 0);
            }
        }

        public void Clear()
        {
            _internalList.Clear();
        }

        public int Count
        {
            get
            {
                return _internalList.Count;
            }
        }
    }
}
