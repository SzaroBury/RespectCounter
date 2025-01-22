namespace RespectCounter.API.Models;

public record ProposeActivityModel(
    int Type, 
    string PersonId,
    string Title, 
    string? Description, 
    string? Location, 
    string? Happend, 
    string Source, 
    string Tags
);