// (c) Copyright Crainiate Software 2010




using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Crainiate.Diagramming.Forms
{
    public delegate void UserActionEventHandler(object sender, UserActionEventArgs e);
    public delegate void RenderEventHandler(object sender, RenderEventArgs e);
    public delegate void LayerRenderEventHandler(object sender, LayerRenderEventArgs e);
    public delegate void TabEventHandler(object sender, TabEventArgs e);
}

