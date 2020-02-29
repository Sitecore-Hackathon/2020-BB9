using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hackathon.BB9.Foundation.Forms.Models;
using Sitecore.Events;
using Sitecore.XConnect;

namespace Hackathon.BB9.Foundation.Forms.Events
{
    public class CreateTeamEventArgs : EventArgs, IPassNativeEventArgs
    {
        private CreateTeamEvent _evt;
        
        public CreateTeamEventArgs(CreateTeamEvent evt)
        {
            this._evt = evt;
        }

        public string Name
        {
            get
            {
                return this._evt.Name;
            }
        }

        public string ContactEmail
        {
            get
            {
                return this._evt.ContactEmail;
            }
        }

        public string ContactGitHubProfile
        {
            get
            {
                return this._evt.ContactGitHubProfile;
            }
        }

        public string Country
        {
            get
            {
                return this._evt.Country;
            }
        }

        public Guid EmailCampaignId
        {
            get
            {
                return this._evt.EmailCampaignId;
            }
        }

        public ContactIdentifier Identifier
        {
            get
            {
                return this._evt.Identifier;
            }
        }

        public List<TeamMember> TeamMembers
        {
            get
            {
                return this._evt.TeamMembers;
            }
        }

    }
}