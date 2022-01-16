﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using Xml.Custom.Serializer.Attributes;

namespace Xml.Custom.Serializer
{
    public class XmlCustomSerializer
    {

        private bool isXmlValid = false;
        private HashSet<object> pastObject = new HashSet<object>();

        public T Serialize<T>(string xml, bool dtdValidation = false)
        {
            const string filePath = "file.xml";
            XmlDocument doc = new XmlDocument();

            if (dtdValidation)
            {
                var settings = new XmlReaderSettings { 
                    DtdProcessing = DtdProcessing.Parse,
                    ValidationType = ValidationType.DTD 
                };
                settings.ValidationEventHandler += new ValidationEventHandler(this.XmlValidationHandler);

                File.WriteAllText(filePath, xml);
                XmlReader reader = XmlReader.Create(filePath, settings);
                while (reader.Read()) ;
            }

            doc.LoadXml(xml);

            XmlElement rootElement = doc.DocumentElement;

            var obj = this.SerializeObject(rootElement, typeof(T));

            return (T)obj;
        }

        public object SerializeObject(XmlNode currentElement, Type type)
        {
            var obj = Activator.CreateInstance(type);
            var objProperties = obj.GetType().GetProperties();

            foreach (System.Xml.XmlAttribute attribute in currentElement.Attributes)
            {
                var attributPproperty = objProperties.FirstOrDefault(x => x.Name.ToLower() == attribute.Name.ToLower());

                if (attributPproperty != null)
                {
                    var propertyType = attributPproperty.PropertyType;
                    if (obj.GetType().CustomAttributes.Any(x => x.AttributeType == typeof(XmlArray)))
                    {
                        attributPproperty.SetValue(obj, Convert.ChangeType(this.SerializeCollection(attribute, attributPproperty), propertyType));
                    }
                    else
                    {
                        attributPproperty.SetValue(obj, Convert.ChangeType(attribute.Value, propertyType));
                    }
                }
            }


            foreach (XmlNode element in currentElement.ChildNodes)
            {

                var elementProperty = obj.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == element.Name.ToLower());

                if (elementProperty != null)
                {
                    var elementObjType = elementProperty.PropertyType;

                    if (elementProperty.CustomAttributes.Any(x => x.AttributeType == typeof(XmlArray)))
                    {
                        elementProperty.SetValue(obj, this.SerializeCollection(element, elementProperty));
                    }
                    else
                    {
                        var result = SerializeObject(element, elementObjType);
                        elementProperty.SetValue(obj, result);
                    }
                }
            }

            return obj;
        }

        public string Deserialize<T>(T obj)
        {
            XmlDocument root = new XmlDocument();
            this.pastObject.Clear();

            var xmlNode = this.DeserializeObject(obj, root);

            if (xmlNode != null)
                root.AppendChild(xmlNode);

            return root.OuterXml;
        }

        private XmlNode DeserializeObject(object obj, XmlDocument root)
        {
            System.Xml.XmlElement xmlElement = null;
            if (typeof(IEnumerable).IsAssignableFrom(obj.GetType()))
            {
                this.AppendItems(root, root, (IList)obj);
                return null;
            }
            else
            {
                xmlElement = root.CreateElement(obj.GetType().Name);
            }

            var objProps = obj.GetType().GetProperties();

            foreach (var p in objProps)
            {
                if (p.PropertyType.IsPrimitive || p.PropertyType == typeof(String) || p.PropertyType == typeof(Guid))
                {
                    this.AppentAttributeToNode(xmlElement, root, obj, p);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                {
                    this.AppendItems(xmlElement, root, (IList)p.GetValue(obj), p);
                }
                else
                {
                    if (!this.pastObject.Contains(obj))
                    {
                        this.pastObject.Add(obj);
                        xmlElement.AppendChild(this.DeserializeObject(p.GetValue(obj), root));
                    }
                }
            }


            return xmlElement;
        }

        private void AppentAttributeToNode(XmlNode xmlNode, XmlDocument root, object obj, PropertyInfo property)
        {
            var attributeName = ((Attributes.XmlAttribute)property.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(Attributes.XmlAttribute))).Name;
            System.Xml.XmlAttribute atribute = root.CreateAttribute(attributeName);
            atribute.Value = property.GetValue(obj).ToString();
            xmlNode.Attributes.Append(atribute);
        }

        private void AppendItems(XmlNode xmlNode, XmlDocument root, IList collection, PropertyInfo property = null)
        {
            var collectionName = property != null ? ((XmlArray)property.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(XmlArray))).Name : "Root";
            var itemName = property != null ? ((XmlArrayItem)property.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(XmlArrayItem))).Name : "RootItem";

            if (collection != null && collection.Count > 0)
            {
                var node = xmlNode.AppendChild(root.CreateElement(collectionName));
                foreach (var item in collection)
                {
                    if (!this.pastObject.Contains(item))
                    {
                        this.pastObject.Add(item);
                        node.AppendChild(this.DeserializeObject(item, root));
                    }
                }
            }
        }

        private object SerializeCollection(XmlNode xmlNode, PropertyInfo property)
        {

            var itemName = ((XmlArrayItem)property.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(XmlArrayItem))).Name;
            var itemType = property.PropertyType.GetGenericArguments().Single();
            IList result = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            foreach (XmlNode item in xmlNode.ChildNodes)
            {
                result.Add(this.SerializeObject(item, itemType));
            }

            return result;
        }

        private void XmlValidationHandler(object sender, ValidationEventArgs args)
        {
            throw new ArgumentException("Xml doesn't follow DTD.");
        }
    }
}
