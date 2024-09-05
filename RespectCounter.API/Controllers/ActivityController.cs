using Microsoft.AspNetCore.Mvc;

namespace RespectCounter.API.Controllers;

//Get Activities By
// Search: Tags, Description, Value, Respect, Happend, Verified/NotVerified
//  Sorting: Best Match, Most Respect, Least Respect, Trending (last 7d reactions and respect)
// PersonId
// ?IdentityUserUid
// Pages/Autoload more?
//Get Activity By Id

//Propose an Activity
//Verify an Activity
//React
//Comment
//?Propose a change
//?Hide a person
//?Add a tag

//other todos: auto added tag based on nationality

[ApiController]
public class ActivityController: ControllerBase
{
    private readonly ILogger<ActivityController> logger;

    public ActivityController(ILogger<ActivityController> logger)
    {
        this.logger = logger;
    }


    
    
    
}