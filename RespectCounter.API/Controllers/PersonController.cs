using System.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Mappers;
using RespectCounter.API.Requests;
using RespectCounter.Application.Commands;
using RespectCounter.Application.Queries;
using RespectCounter.Domain.Model;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/person")]
public class PersonController: ControllerBase
{
    private readonly ILogger<PersonController> logger;
    private readonly ISender mediator;

    public PersonController(ILogger<PersonController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    #region Queries
    [HttpGet("/api/persons")]
    public async Task<IActionResult> GetVerifiedPersons([FromQuery] string search = "", [FromQuery] string order = "")
    {        
        logger.LogInformation($"{DateTime.Now}: GetVerifiedPersons(search: '{search}', order: '{order}')");
        var query = new GetVerifiedPersonsQuery(search, order, User);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("/api/persons/all")]
    public async Task<IActionResult> GetPersons([FromQuery] string search = "", [FromQuery] string order = "")
    {
        logger.LogInformation($"{DateTime.Now}: GetPersons(search: '{search}', order: '{order}')");
        var query = new GetPersonsQuery(search, order, User.TryGetCurrentUserId());
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(string id)
    {
        logger.LogInformation($"{DateTime.Now}: GetPerson(id: '{id}')");
        var query = new GetPersonByIdQuery(id.ToGuid(), User.TryGetCurrentUserId());
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("/api/persons/names")]
    public async Task<IActionResult> GetSimplePersons()
    {
        logger.LogInformation($"{DateTime.Now}: GetSimplePersons()");
        var query = new GetSimplePersonsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }
    #endregion

    #region Commands
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProposePerson(ProposePersonRequest newPerson)
    {
        logger.LogInformation($"{DateTime.Now}: ProposePerson([ProposePersonRequest])");
        AddPersonCommand command = newPerson.ToCommand(User.GetCurrentUserId());
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/verify")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> VerifyPerson(string id)
    {
        logger.LogInformation($"{DateTime.Now}: VerifyPerson(id: '{id}')");
        var command = new VerifyPersonCommand(id.ToGuid());
        Person result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> ProposeUpdatePerson(string id, Person person)
    {
        logger.LogInformation($"{DateTime.Now}: ProposeUpdatePerson(id: '{id}', [Person])");
        throw new NotImplementedException();
    }

    [HttpPut("{id}/hide")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> HidePerson(string id)
    {
        logger.LogInformation($"{DateTime.Now}: HidePerson(id: '{id}')");
        throw new NotImplementedException();
    }
    #endregion
}