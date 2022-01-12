using System;
using System.Linq;
using System.Xml;

namespace Xml.Custom.Serializer
{
    public class XmlCustomSerializer
    {
        public T Serialize<T>(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlElement firstElement = doc.DocumentElement;

            var obj = this.SerializeObject(firstElement, typeof(T));

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

                    property.SetValue(obj, Convert.ChangeType(attribute.Value, propertyType));
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
                    innerProperty.SetValue(obj, result);
                }
            }

            return obj;
        }
    }
}
