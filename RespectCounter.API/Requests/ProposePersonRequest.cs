namespace RespectCounter.API.Requests;

public record ProposePersonRequest(
    string FirstName, 
    string LastName, 
    string? NickName,
    string? Description, 
    string Nationality, 
    string? Birthday, 
    string? DeathDate, 
    string Tags
);