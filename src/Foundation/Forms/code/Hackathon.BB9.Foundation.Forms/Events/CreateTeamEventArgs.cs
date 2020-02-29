using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Events;

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

    }
}