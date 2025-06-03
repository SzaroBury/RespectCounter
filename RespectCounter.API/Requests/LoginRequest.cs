namespace RespectCounter.API.Requests;

public record LoginRequest(
    string Username,
    string Password
);