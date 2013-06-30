using System.IO;
using System.Xml.Serialization;

namespace SpaceTest.SpFrwk.Tools
{
    public class SpaceSerializer<T> where T : class, new()
    {
        private XmlSerializer m_serializer = new XmlSerializer(typeof(T));

        public void Serialize(Stream stream, T o)
        {
            m_serializer.Serialize(stream, o);
        }

        public T Deserialize(Stream stream)
        {
            return m_serializer.Deserialize(stream) as T;
        }
    }
}
