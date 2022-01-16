using Data.Entities.Contracts;
using Xml.Custom.Serializer.Attributes;

namespace Data.Entities
{
    public class Product : BaseModel<Guid>
    {
        public Product()
        {
            this.Id = Guid.NewGuid();
            this.Name = "";
        }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Price")]
        public double Price { get; set; }

        [XmlAttribute("VendorId")]
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}
