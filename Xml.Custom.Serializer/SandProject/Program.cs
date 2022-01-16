// See https://aka.ms/new-console-template for more information
using Data;
using Services;
using Services.Contracts;

string xml = File.ReadAllText(@"D:\Programs\Xml.Custom.Serializer\Xml.Custom.Serializer\SandProject\Input.xml");

using (var dbContext = new StoreDbContext())
{
    IDataService dataService = new DataService(dbContext);
    dataService.SerializeXmlToDbEntities(xml).GetAwaiter().GetResult();
    Console.WriteLine(dataService.DeserializeAllDbEntitiesToXml().GetAwaiter().GetResult());
}

