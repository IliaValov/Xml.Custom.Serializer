using System.ComponentModel.DataAnnotations;
using Xml.Custom.Serializer.Attributes;

namespace Data.Entities.Contracts
{
    public abstract class BaseModel<T>
    {
        [Key]
        [XmlAttribute("Id")]
        public T Id { get; set; }
    }
}
