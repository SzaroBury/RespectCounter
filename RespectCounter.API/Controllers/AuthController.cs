using System.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Requests;
using RespectCounter.Application.Commands;
using RespectCounter.Application.Queries;

namespace RespectCounter.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> logger;
    private readonly ISender mediator;


    public AuthController(ILogger<AuthController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        logger.LogInformation($"{DateTime.Now}: Login([LoginRequest])");

        var command = new LoginCommand(request.Username, request.Password);
        var tokens = await mediator.Send(command);

        AppendSecureCookie("AccessToken", tokens.AccessToken, tokens.AccessTokenExpiration);
        AppendSecureCookie("RefreshToken", tokens.RefreshToken, tokens.RefreshTokenExpiration);
        return Ok("Login successful");
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        logger.LogInformation($"{DateTime.Now}: Register([RegisterRequest])");

        var command = new RegisterCommand(request.Email, request.Username, request.Password);
        var tokens = await mediator.Send(command);

        AppendSecureCookie("AccessToken", tokens.AccessToken, tokens.AccessTokenExpiration);
        AppendSecureCookie("RefreshToken", tokens.RefreshToken, tokens.RefreshTokenExpiration);

        return Ok("Tokens refreshed successfully");
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        logger.LogInformation($"{DateTime.Now}: Refresh()");

        var refreshToken = Request.Cookies["RefreshToken"]
            ?? throw new SecurityException("RefreshToken was not found in headers of the request.");
        var command = new RefreshCommand(refreshToken);
        var tokens = await mediator.Send(command);

        AppendSecureCookie("AccessToken", tokens.AccessToken, tokens.AccessTokenExpiration);
        AppendSecureCookie("RefreshToken", tokens.RefreshToken, tokens.RefreshTokenExpiration);

        return Ok("User registered successfully");
    }

    [Authorize]
    [HttpGet("claims")]
    public async Task<IActionResult> GetClaims()
    {
        logger.LogInformation($"{DateTime.Now}: GetClaims()");
        var query = new GetClaimsQuery(Request.Cookies["AccessToken"]);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    private void AppendSecureCookie(string key, string value, DateTime expiresDate)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiresDate
        };
        Response.Cookies.Append(key, value, options);
    }
}