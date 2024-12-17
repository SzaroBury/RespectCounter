namespace RespectCounter.API.Models;

public record ProposePersonModel(
    string FirstName, 
    string LastName, 
    string Description, 
    string Nationality, 
    string Birthday, 
    string? DeathDate, 
    string Tags
);