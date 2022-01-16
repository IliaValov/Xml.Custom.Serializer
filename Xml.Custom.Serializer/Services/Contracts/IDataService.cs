namespace Services.Contracts
{
    public interface IDataService
    {
        Task SerializeXmlToDbEntities(string xml);

        Task<string> DeserializeDbEntitesToXml();

        Task<string> DeserializeAllDbEntitiesToXml();
    }
}
