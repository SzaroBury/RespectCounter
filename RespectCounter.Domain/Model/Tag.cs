using RespectCounter.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RespectCounter.Domain.Model
{
    public class Tag : IEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsMainTag { get; set; } = false;
        public List<Person> Persons { get; set; } = new();
        public List<Activity> Activities { get; set; } = new();
        public int Count => Persons.Count + Activities.Count;
    }
}
