// See https://aka.ms/new-console-template for more information
using SandProject;
using Xml.Custom.Serializer;

const string xml = @"<book name='Jivota mi' something='1'><author name='Gayorgy'><book name='Jivota mi2' something='1'></book><book name='Jivota mi 3' something='1'></book></author></book>";


var serializer = new XmlCustomSerializer();
var book = serializer.Serialize<Book>(xml);
Console.WriteLine(book.ToString());