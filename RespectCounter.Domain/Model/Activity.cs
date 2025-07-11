﻿using System.ComponentModel.DataAnnotations;
using RespectCounter.Domain.Contracts;

namespace RespectCounter.Domain.Model;

public class Activity : Entity, IReactionable
{
    public string Value { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime? Happend { get; set; } = DateTime.MinValue;
    public string Source { get; set; } = string.Empty;
    public ActivityStatus Status { get; set; } = ActivityStatus.NotVerified;
    public ActivityType Type { get; set; } = ActivityType.Act;
    public Guid PersonId { get; set; }
    [Required]
    public virtual Person? Person { get; set; }
    public virtual List<Reaction> Reactions { get; set; } = new();
    public virtual List<Comment> Comments { get; set; } = new();
    public virtual List<Tag> Tags { get; set; } = new();
}

public enum ActivityType
{
    Act,
    Quote
}

public enum ActivityStatus
{
    NotVerified,
    Verified,
    Hidden
}
