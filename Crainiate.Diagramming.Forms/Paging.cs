// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.ComponentModel;

using Crainiate.Diagramming.Forms.Rendering;

namespace Crainiate.Diagramming.Forms
{
	public class Paging
	{
		//Property variables
        private bool _enabled;
        private Margin _margin;
        private Color _workspacecolor;
        private Size _workspaceSize;
        private SizeF _pageSize;
        private Point _workspaceOffset;
        private Point _pageOffset;
        private SizeF _padding;
        private int _page;

		//Constructors
		public Paging()
		{
            Margin = new Margin();
            WorkspaceColor = SystemColors.AppWorkspace;
            SetWorkspaceOffset(new Point(20, 20));
            Enabled = true;
            Padding = new SizeF(40, 40);
            Page = 1;
		}

		//Properties
        [Description("Determines whether the view is drawn as a set of pages.")]
        public virtual bool Enabled
        {
            get 
            { 
                return _enabled; 
            }
            set 
            {
                _enabled = value;
                HasChanges = true;  
            }
        }


        [Description("Gets Origin sets the current page displayed.")]
        public virtual int Page
        {
            get
            {
                return _page;
            }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException("Page may not be less than 1.");
                _page = value;
            }
        }

        [Description("Defines the distance away from the edge of the page that elements should not be placed in.")]
        public virtual Margin Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _margin = value;
                HasChanges = true; 
            }
        }

        [Description("Sets or gets the color used to draw the application workspace.")]
        public virtual Color WorkspaceColor
        {
            get
            {
                return _workspacecolor;
            }
            set
            {
                _workspacecolor = value;
                HasChanges = true; 
            }
        }

        [Description("Sets or gets the overall workspace rectangle for a paged render.")]
        public virtual Size WorkspaceSize
        {
            get
            {
                return _workspaceSize;
            }
        }

        [Description("Defines the size of the page displayed when pagging is enabled.")]
        public virtual SizeF PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                HasChanges = true;
            }
        }

        public virtual Point WorkspaceOffset
        {
            get
            {
                return _workspaceOffset;
            }
        }

        public virtual Point PageOffset
        {
            get
            {
                return _pageOffset;
            }
        }

        public virtual SizeF Padding
        {
            get
            {
                return _padding;
            }
            set
            {
                _padding = value;
                HasChanges = true;
            }
        }

        protected internal bool HasChanges { get; set; }

        protected internal void SetWorkspaceSize(Size value)
        {
            _workspaceSize = value;
            HasChanges = true;
        }

        protected internal void SetWorkspaceOffset(Point value)
        {
            _workspaceOffset = value;
            HasChanges = true;
        }

        protected internal void SetPageOffset(Point value)
        {
            _pageOffset = value;
            HasChanges = true;
        }
	}
}
