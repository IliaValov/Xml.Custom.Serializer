// See https://aka.ms/new-console-template for more information
using ConsoleApp1;
using Xml.Custom.Serializer;

const string xml = @"<book name='Jivota mi' something='1'><author name='Gayorgy'></author></book>";


var serializer = new XmlCustomSerializer();
var book = serializer.Serialize<Book>(xml);
Console.WriteLine();



