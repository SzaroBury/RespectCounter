﻿using RespectCounter.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RespectCounter.Domain.Model
{
    public class Activity : Entity
    {
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime Happend { get; set; } = DateTime.MinValue;
        public string Source { get; set; } = string.Empty;
        public ActivityStatus Status { get; set; } = ActivityStatus.NotVerified;
        public ActivityType Type { get; set; } = ActivityType.Event;
        public virtual List<Person> Persons { get; set; } = new();
        public virtual List<Reaction> Reactions { get; set; } = new();
        public virtual List<Comment> Comments { get; set; } = new();
        public virtual List<Tag> Tags { get; set; } = new();
    }

    public enum ActivityType
    {
        Event,
        Quote
    }

    public enum ActivityStatus
    {
        NotVerified,
        Verified,
        Hidden
    }
}
