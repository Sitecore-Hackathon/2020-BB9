using System;
using System.Collections.Generic;
using System.Linq;
using Hackathon.BB9.Foundation.Forms.Events;
using Hackathon.BB9.Foundation.Forms.Models;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.ExperienceForms.SubmitActions;
using Sitecore.SecurityModel;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;

namespace Hackathon.BB9.Foundation.Forms.SubmitActions
{
    public class SaveTeam : AnalyticsActionBase<EmailCampaignData>
    {
        public SaveTeam(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        public override void ExecuteAction(FormSubmitContext formSubmitContext, string parameters)
        {
            Assert.ArgumentNotNull((object)parameters, nameof(parameters));
            parameters = parameters.Replace("\"", "");
            parameters = parameters.Split(':')[1];
            parameters = parameters.Substring(0, parameters.LastIndexOf('}'));
            var data = new EmailCampaignData { EmailCampaignId = Guid.Parse(parameters) };
            
            if (data.EmailCampaignId != Guid.Empty)
            {
                Execute(data, formSubmitContext);
            }
        }

        protected override bool Execute(EmailCampaignData data, FormSubmitContext formSubmitContext)
        {
            var evt = new CreateTeamEvent();
            evt.Name = (formSubmitContext.Fields.First(c => c.Name == "Team Name") as StringInputViewModel).Value;
            evt.ContactEmail = (formSubmitContext.Fields.First(c => c.Name == "Main Email") as StringInputViewModel).Value;
            evt.ContactGitHubProfile = (formSubmitContext.Fields.First(c => c.Name == "GitHub Username") as StringInputViewModel).Value;
            evt.Country = (formSubmitContext.Fields.First(c => c.Name == "Country") as StringInputViewModel).Value;

            var teamMember1 = new TeamMember
            {
                Name = (formSubmitContext.Fields.First(c => c.Name == "FirstUserName") as StringInputViewModel).Value,
                GitHub = (formSubmitContext.Fields.First(c => c.Name == "FirstUserGitHub") as StringInputViewModel).Value,
                LinkedIn = (formSubmitContext.Fields.First(c => c.Name == "FirstUserLinkedIn") as StringInputViewModel).Value,
                Twitter = (formSubmitContext.Fields.First(c => c.Name == "FirstUserTwitter") as StringInputViewModel).Value,
            };

            var teamMember2 = new TeamMember
            {
                Name = (formSubmitContext.Fields.First(c => c.Name == "SecondUserName") as StringInputViewModel).Value,
                GitHub = (formSubmitContext.Fields.First(c => c.Name == "SecondUserGitHub") as StringInputViewModel).Value,
                LinkedIn = (formSubmitContext.Fields.First(c => c.Name == "SecondUserLinkedIn") as StringInputViewModel).Value,
                Twitter = (formSubmitContext.Fields.First(c => c.Name == "secondUserTwitter") as StringInputViewModel).Value,
            };

            var teamMember3 = new TeamMember
            {
                Name = (formSubmitContext.Fields.First(c => c.Name == "ThirdUserName") as StringInputViewModel).Value,
                GitHub = (formSubmitContext.Fields.First(c => c.Name == "ThirdUserGitHub") as StringInputViewModel).Value,
                LinkedIn = (formSubmitContext.Fields.First(c => c.Name == "ThirdUserLinkedIn") as StringInputViewModel).Value,
                Twitter = (formSubmitContext.Fields.First(c => c.Name == "ThirdUserTwitter") as StringInputViewModel).Value,
            };
            evt.TeamMembers = new List<TeamMember>
            {
                teamMember1,
                teamMember2,
                teamMember3
            };

            evt.EmailCampaignId = data.EmailCampaignId;

            using (var client = SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    Tracker.Current.Session.IdentifyAs("saveteam", evt.ContactEmail);
                    var trackerIdentifier = new IdentifiedContactReference("saveteam", evt.ContactEmail);
                    var expandOptions = new ContactExpandOptions(CollectionModel.FacetKeys.PersonalInformation, CollectionModel.FacetKeys.EmailAddressList);

                    var contact = client.Get(trackerIdentifier, expandOptions);

                    SetEmail(GetValue(evt.ContactEmail), contact, client);

                    client.Submit();

                    var manager = Factory.CreateObject("tracking/contactManager", true) as Sitecore.Analytics.Tracking.ContactManager;
                    manager.RemoveFromSession(Tracker.Current.Contact.ContactId); // Use tracker ID, not xConnect ID
                    Tracker.Current.Session.Contact = manager.LoadContact(Tracker.Current.Contact.ContactId);

                    evt.Identifier = contact.Identifiers.FirstOrDefault(i => i.Identifier == evt.ContactEmail);
                }
                catch(Exception ex)
                {

                }
            }
            Sitecore.Eventing.EventManager.RaiseEvent(evt);
            return true;
        }

        /// <summary>
        /// Gets the <paramref name="field" /> value.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The field value.</returns>
        private static string GetValue(object field)
        {
            return field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Sets the <see cref="EmailAddressList"/> facet of the specified <paramref name="contact" />.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="contact">The contact.</param>
        /// <param name="client">The client.</param>
        private static void SetEmail(string email, Contact contact, IXdbContext client)
        {
            if (string.IsNullOrEmpty(email))
            {
                return;
            }

            EmailAddressList emailFacet = contact.Emails();
            if (emailFacet == null)
            {
                emailFacet = new EmailAddressList(new EmailAddress(email, false), "Preferred");
            }
            else
            {
                if (emailFacet.PreferredEmail?.SmtpAddress == email)
                {
                    return;
                }

                emailFacet.PreferredEmail = new EmailAddress(email, false);
            }

            client.SetEmails(contact, emailFacet);
        }

        protected override bool TryParse(string value, out EmailCampaignData target) => base.TryParse(value, out target);
    }
}