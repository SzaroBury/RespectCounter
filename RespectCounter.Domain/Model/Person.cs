﻿using RespectCounter.Domain.Contracts;

namespace RespectCounter.Domain.Model;

public class Person : Entity, IReactionable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string NickName { get; set; } = string.Empty;
    public string Profession { get; set; } = "Unknown";
    public string Description { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; } 
    public DateOnly? Birthday { get; set; }
    public DateOnly? DeathDate { get; set; }
    public PersonStatus Status { get; set; } = PersonStatus.NotVerified;
    public virtual List<Activity> Activities { get; set; } = new();
    public virtual List<Comment> Comments { get; set; } = new();
    public virtual List<Reaction> Reactions { get; set; } = new();
    public virtual List<Tag> Tags { get; set; } = new();
}

public enum PersonStatus
{
    NotVerified,
    Verified,
    Hidden
}