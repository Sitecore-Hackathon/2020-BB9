using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;

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
            return true;
        }
    }
}