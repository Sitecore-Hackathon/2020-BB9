using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hackathon.BB9.Foundation.Forms.Tokenizer;
using Sitecore.Analytics;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.EmailCampaign.Cd.Services;
using Sitecore.EmailCampaign.Model.Messaging;
using Sitecore.Events;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Publishing;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;

namespace Hackathon.BB9.Foundation.Forms.Events
{
    public class CreateTeamEventHandler
    {
        public readonly IClientApiService ClientApiService = DependencyResolver.Current.GetService<IClientApiService>();

        //This method is used to create Team item in the database
        public virtual void OnUpdateCommentRemote(object sender, EventArgs e)
        {
            if (e is CreateTeamEventArgs)
            {
                var args = e as CreateTeamEventArgs;
                var master = Sitecore.Configuration.Factory.GetDatabase("master");
                using (new SecurityDisabler())
                {
                    TemplateItem templateItem = master.GetTemplate(ID.Parse(Templates.Team.TemplateId));
                    var parentItem = master.GetItem(Sitecore.Context.Site.StartPath);

                    ReferenceField droplink = parentItem.Fields[ID.Parse(Templates.HackathonSettings.Fields.ActualContest)];

                    if(droplink == null)
                    {
                        //Field does not exist
                    }
                    else if (droplink.TargetItem == null)
                    {
                        // No item selected
                    }
                    else if (droplink.TargetItem.HasChildren && templateItem != null)
                    {
                        var childItem = droplink.TargetItem.Children.InnerChildren.Find(f => f.TemplateID.ToString() == Templates.TeamFolder.TemplateId);
                        if(childItem != null)
                        {
                            EditItem(templateItem, childItem, args);
                        }
                    }
                }
            }
        }

        private void EditItem(TemplateItem templateItem, Item childItem, CreateTeamEventArgs args)
        {
            using (new EditContext(childItem))
            {
                var newItem = childItem.Add(args.Name, templateItem);
                
                using (new EditContext(newItem))
                {
                    newItem[ID.Parse(Templates.Team.Fields.Name)] = args.Name;
                    newItem[ID.Parse(Templates.Team.Fields.ContactGitHubProfile)] = args.ContactGitHubProfile;
                    newItem[ID.Parse(Templates.Team.Fields.ContactEmail)] = args.ContactEmail;
                    newItem[ID.Parse(Templates.Team.Fields.Country)] = args.Country;
                    newItem.DeleteChildren();
                    PublishItem(newItem);
                    TemplateItem memberTemplateItem = Sitecore.Configuration.Factory.GetDatabase("master").GetTemplate(ID.Parse(Templates.TeamMember.TemplateId));
                    foreach (var teammember in args.TeamMembers)
                    {
                        var member = newItem.Add(teammember.Name, memberTemplateItem);
                        using(new EditContext(member))
                        {
                            member[ID.Parse(Templates.TeamMember.Fields.Name)] = teammember.Name;
                            member[ID.Parse(Templates.TeamMember.Fields.LinkedInProfile)] = teammember.LinkedIn;
                            member[ID.Parse(Templates.TeamMember.Fields.TwitterProfile)] = teammember.Twitter;
                            member[ID.Parse(Templates.TeamMember.Fields.GitHubUser)] = teammember.GitHub;
                        }
                        PublishItem(member);
                    }
                }
                SendEmailCampaign(newItem.ID.ToString(), args.Identifier, args.EmailCampaignId);
            }
        }

        private void SendEmailCampaign(string newItemId, Sitecore.XConnect.ContactIdentifier identifier, Guid emailCampaignId)
        {
            var secret = Sitecore.Configuration.Settings.GetSetting("TokenSecretKey");
            var encrypted = Tokenizer.Tokenizer.Encrypt(newItemId, secret);
            var url = LinkManager.GetItemUrl(Sitecore.Configuration.Factory.GetDatabase("master").GetItem(Sitecore.Context.Site.StartPath), new ItemUrlBuilderOptions { AlwaysIncludeServerUrl = true });

            var messageOpitons = new AutomatedMessage();

            //Required Parameters
            messageOpitons.MessageId = Guid.Parse(Templates.EmailCampaignMessageId);
            messageOpitons.ContactIdentifier = identifier;

            //Custom Tokens - Optional
            var tokens = new Dictionary<string, object> { { "urlwithtoken", $"{url}?t={encrypted}" } };
            messageOpitons.CustomTokens = tokens;

            //Language - Optional
            messageOpitons.TargetLanguage = "en";

            //Send Message
            ClientApiService.SendAutomatedMessage(messageOpitons);
        }

        private void PublishItem(Item item)
        {
            using (new UserSwitcher("sitecore\\admin", true))
            {
                // The publishOptions determine the source and target database,
                // the publish mode and language, and the publish date
                var publishOptions = new Sitecore.Publishing.PublishOptions(item.Database,
                    Database.GetDatabase("web"),
                    PublishMode.SingleItem,
                    item.Language,
                    DateTime.Now);  
                
                // Create a publisher with the publishoptions
                var publisher = new Sitecore.Publishing.Publisher(publishOptions);

                // Choose where to publish from
                publisher.Options.RootItem = item;

                // Publish children as well?
                publisher.Options.Deep = true;

                // Do the publish!
                publisher.PublishAsync();

                item.Publishing.ClearPublishingCache();
            }
        }

        // This methos is used to raise the local event
        public static void Run(CreateTeamEvent evt)
        {
            var args = new CreateTeamEventArgs(evt);
            Event.RaiseEvent("createteam:remote", new object[] { args });
        }
    }
}