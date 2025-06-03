using System.Security;
using MediatR;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Commands;

public record AddCommentToPersonCommand(Guid PersonId, string Content, Guid UserId) : IRequest<Person>;

public class AddCommentToPersonCommandHandler : IRequestHandler<AddCommentToPersonCommand, Person>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public AddCommentToPersonCommandHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<Person> Handle(AddCommentToPersonCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        User? user = await userService.GetByIdAsync(request.UserId);
        if(user == null) throw new SecurityException("Authentication issue. No user found.");

        //Person? targetPerson = await uow.Repository().FindByIdAsync<Person>(request.PersonId) ?? throw new KeyNotFoundException("There is no person object with the given id value.");

        // DateTime now = DateTime.Now;

        // Comment comment = new Comment 
        // {
        //     PersonId = request.PersonId,
        //     Content = request.Content,
            
        //     Created = now,
        //     CreatedById = user.Id,
        //     LastUpdated = now,
        //     LastUpdatedById = user.Id,
        // };
        // targetPerson.Comments.Add(comment);
        // await uow.CommitAsync(cancellationToken);

        // //cleaning data before sending it to the client
        // comment.CreatedBy = null;
        // comment.LastUpdatedBy = null;

        // return targetPerson; //toDTO
    }
}