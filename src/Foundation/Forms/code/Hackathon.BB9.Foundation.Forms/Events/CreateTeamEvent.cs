using System.Runtime.Serialization;

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

        public CreateTeamEvent(string name, string contactEmail, string contactGitHubProfile, string country)
        {
            this.Name = name;
            this.ContactEmail = contactEmail;
            this.ContactGitHubProfile = ContactGitHubProfile;
            this.Country = country;
        }

        public CreateTeamEvent()
        {

        }
    }
}