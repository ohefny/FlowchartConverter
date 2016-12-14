// (c) Copyright Crainiate Software 2010

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Crainiate.Diagramming;

namespace Crainiate.Diagramming.Forms.Rendering
{
    //Interface for any class that requires forms rendering to implement
    public interface IFormsRenderer: IRenderer
    {
        void RenderAction(IRenderable element, Graphics graphics, ControlRender render);
        void RenderElement(IRenderable element, Graphics graphics, Render render);
        void RenderHighlight(IRenderable element, Graphics graphics,ControlRender render);
        void RenderSelection(IRenderable element, Graphics graphics, ControlRender render);
        void RenderShadow(IRenderable element, Graphics graphics, Render render);
    }
}

