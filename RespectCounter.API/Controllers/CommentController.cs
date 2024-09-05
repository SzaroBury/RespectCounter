using Microsoft.AspNetCore.Mvc;

namespace RespectCounter.API.Controllers;

//Get Comments By
// PersonId
// ActivityId
// ?CommentId
// ?IdentityUserUid
// Pages/Autoload more?
//Get Activity By Id

//Post a comment
//React
//Comment
//?Edit
//?Hide a comment/Mark as 

//other todos: auto added tag based on nationality

[ApiController]
public class CommentController: ControllerBase
{
    private readonly ILogger<CommentController> logger;
    public CommentController(ILogger<CommentController> logger)
    {
        this.logger = logger;
    }
}