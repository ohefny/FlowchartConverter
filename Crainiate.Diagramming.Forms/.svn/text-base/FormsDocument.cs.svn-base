// (c) Copyright Crainiate Software 2010

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using System.Drawing.Imaging;

using Crainiate.Diagramming.Serialization;
using Crainiate.Diagramming.Forms.Rendering;

namespace Crainiate.Diagramming.Forms
{
    public class FormsDocument: Document 
    {
        //Property variables

        public FormsDocument():base()
        {
        }

        public FormsDocument(Model model): base(model)
        {
        }

        //Exports a diagram in the format specified to the filename provided.
        public virtual void Export(string path, ExportFormat format)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                //Export to image
                if (format == ExportFormat.Bmp)
                {
                    ExportToPicture(stream, ImageFormat.Bmp);
                }
                else if (format == ExportFormat.Gif)
                {
                    ExportToPicture(stream, ImageFormat.Gif);
                }
                else if (format == ExportFormat.Jpeg)
                {
                    ExportToPicture(stream, ImageFormat.Jpeg);
                }
                else if (format == ExportFormat.Png)
                {
                    ExportToPicture(stream, ImageFormat.Png);
                }

                //Close filestream
                stream.Close();
            }
        }

        protected virtual void ExportToPicture(Stream stream, ImageFormat format)
        {
            //Set the renderlists to the whole diagram
            Render render = new Render();

            Rectangle renderRect = new Rectangle(new Point(0, 0), Size.Round(Model.Size));

            //Get the bounds of the renderlist
            if (Singleton.Instance.ClipExport)
            {
                renderRect = System.Drawing.Rectangle.Round(Model.Elements.GetBounds());
                renderRect.Inflate(20, 20);

                if (renderRect.X < 0) renderRect.X = 0;
                if (renderRect.Y < 0) renderRect.Y = 0;
            }

            //Set the render rectangle
            render.Zoom = 100;
            render.RenderRectangle = renderRect;
            render.Layers = Model.Layers;
            render.Elements = Model.Elements;

            //Use a default paging
            render.RenderDiagram(renderRect, new Paging());

            render.Bitmap.Save(stream, format);
        }
    }
}
