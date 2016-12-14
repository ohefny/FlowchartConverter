// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public class TablePort: Port
	{
		//Property variables
		TableItem _tableItem;

        public TablePort(TableItem tableItem): base(0F)
        {
            TableItem = tableItem;
            Fixed = true;
        }

        public virtual TableItem TableItem
        {
            get
            {
                return _tableItem;
            }
            set
            {
                _tableItem = value;
            }
        }

        public override bool AllowMove
        {
            get
            {
                return false;
            }
            set
            {
                throw new ArgumentException("Table ports cannot be moved.");
            }
        }

        public override bool Fixed
        {
            get
            {
                return base.Fixed;
            }
            set
            {
                if (!value) throw new ArgumentException("Table ports must be fixed.");
                base.Fixed = true;
            }
        }
	}
}
