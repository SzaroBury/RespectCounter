using Microsoft.AspNetCore.Mvc;

namespace RespectCounter.API.Controllers;

[ApiController]
public class CommentController: ControllerBase
{
    private readonly ILogger<CommentController> logger;
    public CommentController(ILogger<CommentController> logger)
    {
        this.logger = logger;
    }
}