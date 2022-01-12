using Data.Entities.Contracts;
using Xml.Custom.Serializer.Attributes;

namespace Data.Entities
{
    public class Publisher : BaseModel<Guid>
    {
        public string Name { get; set; }

        [XmlArray]
        public List<Book> Books { get; set; }
    }
}
