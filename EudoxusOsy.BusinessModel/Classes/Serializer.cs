using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EudoxusOsy.BusinessModel
{
    public class Serializer<T>
    {
        private static XmlSerializer xs;

        static Serializer()
        {
            xs = new XmlSerializer(typeof(T));
        }

        public string Serialize(T value, bool omitXmlNamespaces = false)
        {
            if (value == null)
                return string.Empty;

            var sb = new StringBuilder();
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var xmlWriterSettings = new XmlWriterSettings() { OmitXmlDeclaration = true };

            using (var xw = XmlTextWriter.Create(sb, xmlWriterSettings))
            {
                xs.Serialize(xw, value, omitXmlNamespaces ? emptyNamepsaces: null);
            }

            return sb.ToString();
        }

        public T Deserialize(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return default(T);

            return (T)xs.Deserialize(new StringReader(xml));
        }
    }
}
