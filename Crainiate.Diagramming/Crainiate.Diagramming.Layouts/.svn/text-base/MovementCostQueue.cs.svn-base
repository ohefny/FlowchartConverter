// (c) Copyright Crainiate Software 2010



using System;
using System.Collections.Generic;
using System.Text;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming.Layouts
{
    internal class MovementCostQueue: PriorityQueue<RouteNode>
    {
        public int TotalCost
        {
            get
            {
                return Peek().TotalCost;
            }
        }

        //Needs to be in reverse order
        protected override int Compare(int i, int j)
        {
            return InnerList[j].MovementCost.CompareTo(InnerList[i].MovementCost);
        }
    }
}
