namespace RespectCounter.API.Models;

public record RegisterModel(
    string Email,
    string Username,
    string Password
);