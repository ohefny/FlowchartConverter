// (c) Copyright Crainiate Software 2010

using System;
using System.Text;

namespace Crainiate.Diagramming.Collections
{
    public abstract class Elements<T>: Dictionary<string, T>
        where T: Element
    {
        private Model _model;

        //Events
        public event ElementsEventHandler InsertElement;
        public event ElementsEventHandler RemoveElement;
        public event EventHandler Cleared;

        //Constructors
        public Elements(Model model)
        {
            SetModel(model);
        }

        //Deserialisation only
        internal Elements()
        {

        }

        public virtual Model Model
        {
            get
            {
                return _model;
            }
        }

        //Methods
        public override void Add(string key, T value)
        {
            if (key.Contains(".")) throw new CollectionException("Key may not contain '.' character.");

            base.Add(key, value);
            value.SetKey(key);

            ElementInsert(value);

            OnElementInserted(value);
        }

        public virtual void Add(T value)
        {
            this.Add(CreateKey(), value);
        }

        public override bool Remove(string key)
        {
            return base.Remove(key);
        }

        //Creates a new key from the collection
        public string CreateKey()
        {
            if (Singleton.Instance.KeyCreateMode == KeyCreateMode.Normal)
            {
                Type type = typeof(T);
                string typeString = type.Name;
                int i = 0;

                for (i = this.Count; i >= 0; i--)
                {
                    if (this.ContainsKey((typeString + i.ToString()))) break;
                }

                if (i <= 0)
                {
                    return typeString + "1";
                }
                else
                {
                    while (this.ContainsKey(typeString + i.ToString()))
                    {
                        i += 1;
                    }
                    return typeString + i.ToString();
                }
            }
            else
            {
                return Guid.NewGuid().ToString();
            }
        }

        public void SetModel(Model model)
        {
            _model = model;
        }

        protected internal virtual void ElementInsert(T element)
        {
            if (element.Model == null) element.SetModel(Model);

            //Set the layer if not already set
            if (element.Layer == null && Model != null)
            {
                Layer layer = Model.Layers.CurrentLayer;
                element.SetLayer(layer);
            }

            //Set up ports in a port container
            if (element is IPortContainer)
            {
                IPortContainer portContainer = element as IPortContainer;

                foreach (Port port in portContainer.Ports.Values)
                {
                    port.SetModel(Model);
                    port.SetLayer(element.Layer);
                    if (port.Location.IsEmpty) portContainer.LocatePort(port);
                }
            }
        }

        protected virtual void OnElementInserted(Element element)
        {
            if (InsertElement != null) InsertElement(this, new ElementsEventArgs(element));
        }

        protected virtual void OnElementRemoved(Element element)
        {
            if (RemoveElement != null) RemoveElement(this, new ElementsEventArgs(element));
        }
    }
}
