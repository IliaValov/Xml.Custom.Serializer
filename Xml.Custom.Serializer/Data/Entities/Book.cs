using Data.Entities.Contracts;

namespace Data.Entities
{
    public class Book : BaseModel<Guid>
    {
        public string Name { get; set; }

        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
