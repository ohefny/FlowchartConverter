// (c) Copyright Crainiate Software 2010




using System;
using System.IO;
using System.Runtime.Serialization;
using System.Drawing.Imaging;

// Mimics the System.Runtime.Serialization.SerializationEntry class
// to make it possible to create our own entries. The class acts as a placeholder for a type,
// it's name and it's value. This class is used in the XmlFormatter class to serialize objects.
namespace Crainiate.Diagramming.Serialization
{
    public struct ResourceEntry
    {
        private string _name;
        private Type _objectType;
        private object _value;

        //Constructors 
        public ResourceEntry(string name, Type objectType, object value)
        {
            _name = name;
            _objectType = objectType;
            _value = value;
        }

        //Properties
        //Gets the name of the object.
        public string Name
        {
            get
            {
                return _name;
            }
        }

        //Gets the System.Type of the object.
        public Type ObjectType
        {
            get
            {
                return _objectType;
            }
        }

        //Gets or sets the value contained in the object.
        public object Value
        {
            get
            {
                return _value;
            }
        }

        //Saves the resource value to a stream
        public void SaveResourceStream(Stream stream)
        {
            System.Drawing.Bitmap image = _value as System.Drawing.Bitmap;
            if (image == null) return;

            //Determine the format to use
            ImageFormat format = ImageFormat.Bmp;

            image.Save(stream, format);
        }

        //Returns the file extension for the resource
        public string GetResourceType()
        {
            return "bmp";
        }

        //Returns the file extension for the resource
        public string GetMimeType()
        {
            return "image/bmp";
        }
    }
}