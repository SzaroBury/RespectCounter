using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using System.Security.Claims;

namespace RespectCounter.Application.Queries;

public record GetCommentsForPersonQuery(Guid PersonId, int Levels, Guid? UserId) : IRequest<List<CommentDTO>>;

public class GetCommentsForPersonQueryHandler : IRequestHandler<GetCommentsForPersonQuery, List<CommentDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public GetCommentsForPersonQueryHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<List<CommentDTO>> Handle(GetCommentsForPersonQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // string? userGuid = null;
        // if(request.UserId.HasValue)
        // {
        //     User? user = await userService.GetByIdAsync(request.UserId.Value);
        //     userGuid = user?.Id.ToString();
        // }

        // var result = await uow.Repository()
        //     .FindQueryable<Comment>(c => c.PersonId == request.PersonId && c.CommentStatus != CommentStatus.Hidden)
        //     .Include(c => c.Children).Include(c => c.Reactions).Include(c => c.CreatedBy)
        //     .OrderByDescending(c => c.Created)
        //     .Select(c => c.ToDTO(request.Levels, userGuid))
        //     .ToListAsync(cancellationToken);
        // return result;
    }
}