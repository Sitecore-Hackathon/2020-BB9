using System;

using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Publishing;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;

namespace Hackathon.BB9.Foundation.Forms.Events
{
    public class CreateTeamEventHandler
    {
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

                    using (new EditContext(parentItem))
                    {
                        var newItem = parentItem.Add(args.Name, templateItem);
                        using (new EditContext(newItem))
                        {
                            newItem[ID.Parse(Templates.Team.Fields.Name)] = args.Name;
                            newItem[ID.Parse(Templates.Team.Fields.ContactGitHubProfile)] = args.ContactGitHubProfile;
                            newItem[ID.Parse(Templates.Team.Fields.ContactEmail)] = args.ContactEmail;
                            newItem[ID.Parse(Templates.Team.Fields.Country)] = args.Country;
                        }
                        PublishItem(newItem);
                    }
                }
            }
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