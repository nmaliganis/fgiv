namespace erpl.common.infrastructure
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}