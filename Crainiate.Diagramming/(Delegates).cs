// (c) Copyright Crainiate Software 2010




using System;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming
{
    public delegate void ElementEventHandler(object sender, ElementEventArgs e);
    public delegate void ElementRenderEventHandler(object sender, ElementRenderEventArgs e);
    public delegate void LayoutChangedEventHandler(object sender, LayoutChangedEventArgs e);
    public delegate void SegmentsEventHandler(object sender, SegmentsEventArgs e);
    public delegate void DrawShapeEventHandler(object sender, DrawShapeEventArgs e);
    public delegate void ExpandedChangedEventHandler(object sender, bool expanded);
    public delegate void TableItemsEventHandler(object sender, TableItemsEventArgs e);
}
