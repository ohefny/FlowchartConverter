// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Collections;

using Crainiate.Diagramming;

namespace Crainiate.Diagramming.Forms
{
	public class Tabs: Layers
	{
		//Property variables
		private float _tabHeight;
		private Color _gradientColor;
		private Color _backColor;
		private Color _foreColor;

		#region Interface

		public event EventHandler TabsInvalid;
        public event TabEventHandler InsertTab;
        public event TabEventHandler RemoveTab;

		public Tabs(Model model) //Do not call constructor
		{
            //Add default layer
            Tab tab = new Tab(true);
            tab.Name = "Default";
            Add(tab);
            CurrentLayer = tab;

            SetModel(model);

			GradientColor = SystemColors.Control;
			BackColor = SystemColors.Control;
			ForeColor = Color.FromArgb(66,65,66);
			TabHeight = 18;
		}

		public virtual float TabHeight
		{
			get
			{
				return _tabHeight;
			}
			set
			{
				_tabHeight = value;
			}
		}

		public virtual Color GradientColor
		{
			get
			{
				return _gradientColor;
			}
			set
			{
				_gradientColor = value;
				OnTabsInvalid();
			}
		}

		public virtual Color BackColor
		{
			get
			{
				return _backColor;
			}
			set
			{
				_backColor = value;
				OnTabsInvalid();
			}
		}

		public virtual Color ForeColor
		{
			get
			{
				return _foreColor;
			}
			set
			{
				_foreColor = value;
				OnTabsInvalid();
			}
		}

        public virtual Tab CurrentTab
        {
            get
            {
                return CurrentLayer as Tab;
            }
            set
            {
                CurrentLayer = value;
            }
        }

		protected virtual void OnTabsInvalid()
		{
			if (TabsInvalid != null) TabsInvalid(this,EventArgs.Empty);
		}

        protected override void OnInserted(Layer item)
        {
            base.OnInserted(item);
            if (InsertTab != null) InsertTab(this, new TabEventArgs(item as Tab));
        }

        protected override void OnRemove(Layer item)
        {
            base.OnRemove(item);
            if (RemoveTab != null) RemoveTab(this, new TabEventArgs(item as Tab));
        }

		#endregion
	}
}