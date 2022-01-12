using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Xml.Custom.Serializer.Attributes;

namespace Xml.Custom.Serializer
{
    public class XmlCustomSerializer
    {

        public T Serialize<T>(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlElement rootElement = doc.DocumentElement;

            var obj = this.SerializeObject(rootElement, typeof(T));

            return (T)obj;
        }

        public object SerializeObject(XmlNode currentElement, Type type)
        {
            var obj = Activator.CreateInstance(type);
            var objProperties = obj.GetType().GetProperties();

            foreach (XmlAttribute attribute in currentElement.Attributes)
            {
                var property = objProperties.FirstOrDefault(x => x.Name.ToLower() == attribute.Name.ToLower());

                if (property != null)
                {
                    var propertyType = property.PropertyType;
                    if (obj.GetType().CustomAttributes.Any(x => x.AttributeType == typeof(XmlArray)))
                    {
                        var collection = (IList)property.GetValue(obj);
                        if (collection == null)
                            collection = new List<object>();
                        collection.Add(Convert.ChangeType(attribute.Value, propertyType));
                        property.SetValue(obj, collection);
                    }
                    else
                    {
                        property.SetValue(obj, Convert.ChangeType(attribute.Value, propertyType));
                    }
                }
            }

            var lastObj = obj;

            foreach (XmlNode element in currentElement.ChildNodes)
            {
                var innerProperty = lastObj.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == element.Name.ToLower());

                if (innerProperty != null)
                {
                    var innerObjType = innerProperty.PropertyType;
                    var innerObj = Activator.CreateInstance(innerObjType);

                    var result = SerializeObject(element, innerObjType);

                    if (innerObjType.CustomAttributes.Any(x => x.AttributeType == typeof(XmlArray)))
                    {
                        var collection = (IList)innerProperty.GetValue(obj);
                        if (collection == null)
                            collection = new List<object>();
                        collection.Add(obj);
                        innerProperty.SetValue(obj, collection);
                    }
                    else
                    {
                        innerProperty.SetValue(obj, result);
                    }
                }
            }

            return obj;
        }

        public string Deserialize<T>(T obj)
        {
            var objProps = obj.GetType().GetProperties();

            XmlDocument root = new XmlDocument();
            foreach (var p in objProps)
            {
                XmlAttribute atribute = root.CreateAttribute(p.Name);
                atribute.Value = p.GetValue(obj).ToString();

                root.Attributes.Append(atribute);
            }

            return root.ToString();
        }
    }
}
