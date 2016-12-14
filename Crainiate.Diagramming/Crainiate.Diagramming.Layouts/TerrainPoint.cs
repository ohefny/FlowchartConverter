// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Collections;

namespace Crainiate.Diagramming.Layouts
{
	internal struct TerrainPoint
	{
        public int X;
        public int Y;
        public int MovementCost2;

		public TerrainPoint(int x, int y)
		{
			X = x;
            Y = y;
            MovementCost2 = -1;
		}
		
        public bool IsEmpty
        {
            get
            {
                return (MovementCost2 == 0 && X == 0 && Y == 0);
            }
        }

	}
}
