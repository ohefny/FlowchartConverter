using Crainiate.Diagramming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingLast.Extended
{
    class ExtendedShape : Shape
    {
        OnShapeClickListener listener;
      
        internal OnShapeClickListener Listener
        {
            get
            {
                return listener;
            }

            set
            {
                listener = value;
            }
        }

        protected override void OnSelectedChanged() {
            base.OnSelectedChanged();
            if (Selected) {
                if (Listener != null) {
                    Listener.onShapeClicked();
                }
            }
        }
    }
}
