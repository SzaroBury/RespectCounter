namespace RespectCounter.Domain.DTO;

public record PersonDTO(
    string FirstName, 
    string LastName, 
    string Description, 
    string Nationality, 
    string Birthday, 
    string? DeathDate, 
    string Tags
);