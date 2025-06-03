namespace RespectCounter.Domain.Model;

public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public Guid CreatedById { get; set; } = Guid.Empty;
    // [DeleteBehavior(DeleteBehavior.NoAction)] // to move to DbContext
    public virtual User? CreatedBy { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.Now;
    public Guid LastUpdatedById { get; set; } = Guid.Empty;
    
    // [DeleteBehavior(DeleteBehavior.NoAction)] // to move to DbContext
    public virtual User? LastUpdatedBy { get; set;}
}   
