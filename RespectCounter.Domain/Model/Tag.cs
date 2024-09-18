using RespectCounter.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RespectCounter.Domain.Model
{
    public class Tag : Entity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Level { get; set; } = 5;
        public virtual List<Person> Persons { get; set; } = new();
        public virtual List<Activity> Activities { get; set; } = new();
        public int Count => Persons.Count + Activities.Count;
    }
}
