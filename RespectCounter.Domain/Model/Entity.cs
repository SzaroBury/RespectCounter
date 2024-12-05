using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Interfaces;

namespace RespectCounter.Domain.Model
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        
        public DateTime Created { get; set; } = DateTime.Now;
        public string CreatedById { get; set; } = "sys";
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual IdentityUser? CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public string LastUpdatedById { get; set; } = "sys";
        
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual IdentityUser? LastUpdatedBy { get; set;}
    }   
}
