using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Hackathon.BB9.Foundation.Forms.Models;
using Sitecore.XConnect;

namespace Hackathon.BB9.Foundation.Forms.Events
{
    [DataContract]
    public class CreateTeamEvent
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ContactEmail { get; set; }

        [DataMember]
        public string ContactGitHubProfile { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public Guid EmailCampaignId { get; set; }

        [DataMember]
        public ContactIdentifier Identifier { get; set; }

        [DataMember]
        public List<TeamMember> TeamMembers { get; set; }

        public CreateTeamEvent(string name, string contactEmail, string contactGitHubProfile, string country, Guid emailCampaignId, ContactIdentifier identifier, List<TeamMember> TeamMembers)
        {
            this.Name = name;
            this.ContactEmail = contactEmail;
            this.ContactGitHubProfile = ContactGitHubProfile;
            this.Country = country;
            this.EmailCampaignId = emailCampaignId;
            this.Identifier = identifier;
            this.TeamMembers = TeamMembers;
        }

        public CreateTeamEvent()
        {

        }
    }
}