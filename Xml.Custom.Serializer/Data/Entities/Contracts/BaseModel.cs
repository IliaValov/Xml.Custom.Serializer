namespace Data.Entities.Contracts
{
    public abstract class BaseModel<T>
    {
        public T Id { get; set; }
    }
}
