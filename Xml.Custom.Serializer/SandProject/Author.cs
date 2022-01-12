using Xml.Custom.Serializer.Attributes;

namespace SandProject
{
    public class Author
    {
        public Author()
        {
            this.Books = new List<Book>();
        }

        public string Name { get; set; }
        
        [XmlArray]
        public List<Book> Books { get; set; }

        public override string ToString()
        {
            return $"Name: {this.Name}\r\nBooks: ${string.Join(" ", Books)}";
        }
    }
}
