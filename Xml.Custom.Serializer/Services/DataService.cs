using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Xml.Custom.Serializer;

namespace Services
{
    public class DataService : IDataService
    {
        private readonly StoreDbContext dbContext;

        public DataService(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> DeserializeAllDbEntitiesToXml()
        {
            var serializer = new XmlCustomSerializer();
            var vendors = await this.dbContext.Vendors.Include(x => x.Products).ToListAsync();
            return serializer.Deserialize<List<Vendor>>(vendors);
        }

        public async Task<string> DeserializeDbEntitesToXml()
        {
            var serializer = new XmlCustomSerializer();
            var vendor = this.dbContext.Vendors.Include(x => x.Products).FirstOrDefault(x => x.Id == new Guid("f3e2062d-9466-4164-b6ca-19b4d0045526"));
            return serializer.Deserialize<Vendor>(vendor);
        }

        public async Task SerializeXmlToDbEntities(string xml)
        {
            var serializer = new XmlCustomSerializer();
            var result = serializer.Serialize<Vendor>(xml, true);
            await this.dbContext.Vendors.AddAsync(result);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
