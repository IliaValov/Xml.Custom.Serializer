using Data.Entities.Contracts;
using Xml.Custom.Serializer.Attributes;

namespace Data.Entities
{
    public class Vendor : BaseModel<Guid>
    {
        public Vendor()
        {
            this.Id = Guid.NewGuid();
            this.Name = "";
            this.Products = new List<Product>();
        }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlArray("Products")]
        [XmlArrayItem("Product")]
        public ICollection<Product> Products { get; set; }
    }
}
