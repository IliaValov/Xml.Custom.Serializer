using System;
using Data.Entities.Contracts;

namespace Data.Entities
{
    public class Worker : BaseModel<Guid>
    {
        public string Name { get; set; }

        public Guid StoreId { get; set; }
        public Store Store { get; set; }
    }
}
