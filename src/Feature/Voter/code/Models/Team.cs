using System;

namespace Hackathon.BB9.Feature.Voter.Models
{
    public class Team
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? TopicId { get; set; }

        public string TopicName { get; set; }
    }
}