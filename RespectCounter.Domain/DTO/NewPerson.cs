namespace RespectCounter.Domain.DTO;

public class NewPerson
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Birthday { get; set; } = string.Empty;
        public string? DeathDate { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
    }