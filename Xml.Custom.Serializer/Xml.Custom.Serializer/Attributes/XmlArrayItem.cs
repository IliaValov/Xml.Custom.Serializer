using System;

namespace Xml.Custom.Serializer.Attributes
{
    public class XmlArrayItem : Attribute
    {
        public XmlArrayItem(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
