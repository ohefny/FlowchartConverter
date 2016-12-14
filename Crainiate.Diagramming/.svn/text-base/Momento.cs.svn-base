using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;

using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
    //Momento follows the Momento design pattern
    public class Momento<T> : IMomento<T>
    {
        //Working variables
        private byte[] _bytes;

        public Momento(T graph)
        {
            SerializeItem(graph, true);
        }

        public Momento(T graph, bool shallow)
        {
            SerializeItem(graph, shallow);
        }

        public virtual T CreateItem()
        {
            return DeserializeItem(null);
        }

        public virtual void WriteItem(T item)
        {
            DeserializeItem(item);
        }

        private void SerializeItem(T graph, bool shallow)
        {
            MemoryStream stream = new MemoryStream();
            GZipStream zipStream = new GZipStream(stream, CompressionMode.Compress);

            //Serialize into the stream
            try
            {
                XmlFormatter serializer = Singleton.Instance.XmlFormatter;
                serializer.Shallow = shallow;

                serializer.Serialize(zipStream, graph);
                zipStream.Close();

                _bytes = stream.ToArray();
            }
            finally
            {
                zipStream.Dispose();
                stream.Dispose();
            }
        }

        private T DeserializeItem(object item)
        {
            MemoryStream stream = new MemoryStream(_bytes);
            GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress);

            XmlFormatter serializer = Singleton.Instance.XmlFormatter;
            serializer.Shallow = true;
            serializer.Target = item;
            return (T) serializer.Deserialize(zipStream);
        }
    }
}
