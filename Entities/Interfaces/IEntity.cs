using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Entities.Interfaces
{
    public class IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public IdentityUser CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public IdentityUser LastUpdatedBy { get; set;}
    }   
}
