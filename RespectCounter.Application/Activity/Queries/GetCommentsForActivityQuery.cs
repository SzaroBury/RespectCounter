using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Application.Queries;

public record GetCommentsForActivityQuery(string ActId, int Levels, Guid? UserId) : IRequest<List<CommentDTO>>;

public class GetCommentsForActivityQueryHandler : IRequestHandler<GetCommentsForActivityQuery, List<CommentDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public GetCommentsForActivityQueryHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<List<CommentDTO>> Handle(GetCommentsForActivityQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // if (!Guid.TryParse(request.ActId, out Guid actGuid))
        // {
        //     throw new FormatException($"Given value for id parameter '{request.ActId}' has not a valid guid format.");
        // }

        // string? userGuid = null;
        // if(request.UserId.HasValue)
        // {
        //     User? user = await userService.GetByIdAsync(request.UserId.Value);
        //     userGuid = user?.Id.ToString();
        // }

        // var result = await uow.Repository()
        //     .FindQueryable<Comment>(c => c.ActivityId == actGuid && c.CommentStatus != CommentStatus.Hidden)
        //     .Include(c => c.Children).Include(c => c.Reactions).Include(c => c.CreatedBy)
        //     .OrderByDescending(c => c.Created)
        //     .Select(c => c.ToDTO(request.Levels, userGuid))
        //     .ToListAsync();
        // return result;
    }
}