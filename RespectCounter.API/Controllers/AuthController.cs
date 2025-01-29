using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Models;
using RespectCounter.Application.Commands;
using RespectCounter.Application.Queries;

namespace RespectCounter.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController: ControllerBase
{
    private readonly ILogger<TagController> logger;
    private readonly ISender mediator;
    

    public AuthController(ILogger<TagController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel request)
    {
        try
        {
            CookieOptions option = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(10)
            };
    
            var command = new LoginCommand(request.Username, request.Password);
            var token = await mediator.Send(command);

            Response.Cookies.Append("jwt", token, option);
            return Ok(new { message = "Login successful" });
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(new { message = e.Message });
        }
        catch (Exception e)
        {
            var errorResponse = new
            {
                title = "Internal Server Error",
                message = $"An unexpected error occurred: '{e.Message}'."
            };
            return StatusCode(500, errorResponse);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel request)
    {
        try
        {
            CookieOptions option = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(10)
            };
    
            var command = new RegisterCommand(request.Email, request.Username, request.Password);
            var result = await mediator.Send(command);
            if (result.Succeeded)
            {
                var loginCommand = new LoginCommand(request.Username, request.Password);
                var token = await mediator.Send(loginCommand);
                Response.Cookies.Append("jwt", token, option);
                return Ok(new { message = "User registered successfully" });
            }
            var errorMessage = $"Failed: {string.Join(" ", result.Errors.Select(x => x.Description).ToList())}";
            return BadRequest(new { title = "Bad request", message = errorMessage });
        }
        catch (Exception e)
        {
            var errorResponse = new
            {
                title = "Internal Server Error",
                message = $"An unexpected error occurred: '{e.Message}'."
            };
            return StatusCode(500, errorResponse);
        }
    }

    [Authorize]
    [HttpGet("claims")]
    public async Task<IActionResult> GetClaims()
    {
        try
        {
            var query = new GetClaimsQuery(Request.Cookies["jwt"] ?? "");
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch(UnauthorizedAccessException e)
        {
            return Unauthorized(new { message = e.Message });
        }
        catch (Exception e)
        {
            var errorResponse = new
            {
                title = "Internal Server Error",
                message = $"An unexpected error occurred: '{e.Message}'."
            };
            return StatusCode(500, errorResponse);
        }
    }
}