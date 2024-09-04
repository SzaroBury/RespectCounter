using Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Entities.Model
{
    public class Tag : IEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsMainTag { get; set; } = false;
        public List<Person> Persons { get; set; } = new();
    }
}
