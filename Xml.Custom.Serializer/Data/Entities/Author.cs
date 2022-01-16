using System;
using Data.Entities.Contracts;
using Xml.Custom.Serializer.Attributes;

namespace Data.Entities
{
    public class Author : BaseModel<Guid>
    {
        public Author()
        {
            this.books = new List<Book>();
        }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public int Age { get; set; }

        [XmlArray]
        public List<Book> books { get; set; }
    }
}
