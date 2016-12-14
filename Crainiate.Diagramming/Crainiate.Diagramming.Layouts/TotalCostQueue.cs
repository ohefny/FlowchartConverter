// (c) Copyright Crainiate Software 2010




using System;
using System.Collections.Generic;
using System.Text;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming.Collections
{
    internal class TotalCostQueue: PriorityQueue<RouteNode>
    {
        public int TotalCost
        {
            get
            {
                return Peek().TotalCost;
            }
        }

        protected override int Compare(int i, int j)
        {
            return InnerList[i].TotalCost.CompareTo(InnerList[j].TotalCost);
        }
    }
}
