// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Collections;

namespace Crainiate.Diagramming.Collections
{
    internal enum NodeDirection
    {
        Up = 0,
        Down = 1,
        Right = 2,
        Left = 3
    }
	internal sealed class RouteNode: IComparable<RouteNode>, IReusable
	{
        public int X;
        public int Y; 
		private RouteNode _parent; //Parent of this node
		public int MovementCost; //G cost
		public int Heuristic; //H cost = best guess distance to move to goal (Heuristic). Must not be greater than possible
		public int TotalCost; //F cost = G Cost + H (heuristic) cost
		public bool Closed;
        public NodeDirection Direction;

		public RouteNode()
		{

		}

        public RouteNode Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                Direction = GetDirection();
            }

        }

        public void Reuse()
        {
            X = 0;
            Y = 0;
            Parent = null;
            MovementCost = 0;
            Heuristic = 0;
            TotalCost = 0;
            Closed = false;
        }

        public void SetPoint(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

		public bool Equals(RouteNode node)
		{
			return (node.X == X && node.Y == Y);
		}

		public int CompareTo(RouteNode obj)
		{
			return TotalCost.CompareTo(obj.TotalCost);
		}

        //Returns true if the routenode's parents are in a straight line
        public bool IsLinear()
        {
            //Return true if doesnt have 3 parents
            if (Parent == null || Parent.Parent == null) return true;

            if (X == Parent.X && X == Parent.Parent.X) return true;
            if (Y == Parent.Y && Y == Parent.Parent.Y) return true;

            return false;
        }

        public override string ToString()
        {
            string detail = string.Format("{0}+{1}={2}",MovementCost,Heuristic,TotalCost);
            return string.Format("X={0},Y={1} ({2}))", X, Y, detail + Direction.ToString());
        }

        private NodeDirection GetDirection()
        {
            if (_parent == null) return NodeDirection.Down;

            if (_parent.Y < Y) return NodeDirection.Down;
            if (_parent.X < X) return NodeDirection.Right;
            if (_parent.Y > Y) return NodeDirection.Up;
            if (_parent.X > X) return NodeDirection.Left;

            return NodeDirection.Down;
        }
	}
}
