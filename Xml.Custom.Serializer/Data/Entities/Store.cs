using Data.Entities.Contracts;
using Xml.Custom.Serializer.Attributes;

namespace Data.Entities
{
    public class Store : BaseModel<Guid>
    {
        [XmlArray]
        public List<Book> Books { get; set; }

        [XmlArray]
        public List<Publisher> Publishers { get; set; }
    }
}
