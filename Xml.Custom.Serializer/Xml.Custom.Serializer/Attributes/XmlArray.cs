﻿namespace Xml.Custom.Serializer.Attributes
{
    public class XmlArray : System.Attribute
    {
        public XmlArray(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
