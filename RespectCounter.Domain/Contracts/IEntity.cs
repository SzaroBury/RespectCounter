namespace RespectCounter.Domain.Contracts;

public interface IEntity
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public string CreatedById { get; set; }
    public DateTime LastUpdated { get; set; }
    public string LastUpdatedById { get; set; }
}   
