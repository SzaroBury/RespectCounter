namespace RespectCounter.Application.DTOs;

public record CommentDTO(
    string Id, 
    string CreatedBy,
    string CreatedById,
    string Created,
    string Content, 
    string ActivityId,
    string PersonId,
    string ParentId,
    int Status,
    int Respect,
    int ChildrenCount,
    List<CommentDTO> Children
);