using System.Linq;
using Hackathon.BB9.Foundation.Forms.Events;
using Sitecore;
using Sitecore.Data;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.SecurityModel;

namespace Hackathon.BB9.Foundation.Forms.SubmitActions
{
    public class SaveTeam : SubmitActionBase<string>
    {
        public SaveTeam(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            var evt = new CreateTeamEvent();
            evt.Name = (formSubmitContext.Fields.First(c => c.Name == "Team Name") as StringInputViewModel).Value;
            evt.ContactEmail = (formSubmitContext.Fields.First(c => c.Name == "Main Email") as StringInputViewModel).Value;
            evt.ContactGitHubProfile = (formSubmitContext.Fields.First(c => c.Name == "GitHub Username") as StringInputViewModel).Value;
            evt.Country = (formSubmitContext.Fields.First(c => c.Name == "Country") as StringInputViewModel).Value;
            Sitecore.Eventing.EventManager.RaiseEvent(evt);

            return true;
        }
    }
}