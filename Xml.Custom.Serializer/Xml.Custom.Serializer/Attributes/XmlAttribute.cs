namespace Xml.Custom.Serializer.Attributes
{
    public class XmlAttribute : System.Attribute
    {
        public XmlAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
