using MediatR;
using RespectCounter.Application.DTOs;
using RespectCounter.Domain.Model;
using RespectCounter.Domain.Contracts;
using RespectCounter.Application.Services;
using RespectCounter.Application.Common;

namespace RespectCounter.Application.Queries;

public record GetCommentsForPersonQuery(
    Guid PersonId,
    int Levels,
    Guid? UserId,
    CommentSortBy? Order = null
) : IRequest<IEnumerable<CommentDTO>>;

public class GetCommentsForPersonQueryHandler : IRequestHandler<GetCommentsForPersonQuery, IEnumerable<CommentDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IUserService userService;

    public GetCommentsForPersonQueryHandler(IUnitOfWork uow, IUserService userService)
    {
        this.uow = uow;
        this.userService = userService;
    }

    public async Task<IEnumerable<CommentDTO>> Handle(GetCommentsForPersonQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = null;
        if (request.UserId.HasValue)
        {
            User? user = await userService.GetByIdAsync(request.UserId.Value);
            userId = user?.Id;
        }

        var query = uow.Repository().FindQueryable<Comment>(
            c => c.PersonId == request.PersonId && c.CommentStatus != CommentStatus.Hidden
        );

        var order = request.Order ?? CommentSortBy.LatestAdded;
        var orderedQuery = query.ApplySorting(order);
        
        var comments = await uow.Repository().FindListAsync<Comment>(
            orderedQuery,
            ["Children", "Reactions"],
            q => q.OrderByDescending(c => c.Created),
            cancellationToken
        );

        foreach (var comment in comments)
        {
            comment.CreatedBy = await userService.GetByIdAsync(comment.CreatedById);
            comment.Children = await uow.Repository().FindListAsync<Comment>(
                c => c.ParentId == comment.Id && c.CommentStatus != CommentStatus.Hidden,
                null,
                q => q.OrderByDescending(c => c.Created),
                cancellationToken
            );
        }

        return comments.Select(c => c.ToDTO(request.Levels, userId));
    }
}