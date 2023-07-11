using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Entities.Interfaces
{
    public class IEntity
    {
        public int Id { get; set; }
        
        public DateTime Created { get; set; } = DateTime.Now;
        public string CreatedById { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public IdentityUser CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public string LastUpdatedById { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public IdentityUser LastUpdatedBy { get; set;}
    }   
}
