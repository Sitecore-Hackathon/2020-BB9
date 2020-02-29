using System;
using Sitecore.Data;
using Sitecore.ExperienceForms.ValueProviders;

namespace Hackathon.BB9.Foundation.Forms.ValueProviders
{
    public class TeamValueProvider : IFieldValueProvider
    {
        public FieldValueProviderContext ValueProviderContext { get; set; }

        public object GetValue(string parameters)
        {
            var token = Sitecore.Context.Request.GetQueryString("t");

            if(string.IsNullOrEmpty(token))
            {
                return "";
            }

            var secret = Sitecore.Configuration.Settings.GetAppSetting("TokenSecretKey");
            var itemId = Tokenizer.Tokenizer.Decrypt(token, secret);
            try
            {
                var item = Sitecore.Context.Database.GetItem(itemId) ?? null;
                var child1 = item.Children[0] ?? null;
                var child2 = item.Children[1] ?? null;
                var child3 = item.Children[2] ?? null;

                switch (parameters)
                {
                    case "teamName":
                        return item?.Fields?[ID.Parse(Templates.Team.Fields.Name)];
                    case "mainEmail":
                        return item?.Fields?[ID.Parse(Templates.Team.Fields.ContactEmail)];
                    case "country":
                        return item?.Fields?[ID.Parse(Templates.Team.Fields.Country)];
                    case "gitHubUsername":
                        return item?.Fields?[ID.Parse(Templates.Team.Fields.ContactGitHubProfile)];
                    case "firstUserName":
                        return child1?.Fields?[ID.Parse(Templates.TeamMember.Fields.Name)];
                    case "secondUserName":
                        return child2?.Fields?[ID.Parse(Templates.TeamMember.Fields.Name)];
                    case "thirdUserName":
                        return child3?.Fields?[ID.Parse(Templates.TeamMember.Fields.Name)];
                    case "firstUserLinkedIn":
                        return child1?.Fields?[ID.Parse(Templates.TeamMember.Fields.LinkedInProfile)];
                    case "secondUserLinkedIn":
                        return child2?.Fields?[ID.Parse(Templates.TeamMember.Fields.LinkedInProfile)];
                    case "thirdUserLinkedIn":
                        return child3?.Fields?[ID.Parse(Templates.TeamMember.Fields.LinkedInProfile)];
                    case "firstUserTwitter":
                        return child1?.Fields?[ID.Parse(Templates.TeamMember.Fields.TwitterProfile)];
                    case "secondUserTwitter":
                        return child2?.Fields?[ID.Parse(Templates.TeamMember.Fields.TwitterProfile)];
                    case "thirdUserTwitter":
                        return child3?.Fields?[ID.Parse(Templates.TeamMember.Fields.TwitterProfile)];
                    case "firstUserGitHub":
                        return child1?.Fields?[ID.Parse(Templates.TeamMember.Fields.GitHubUser)];
                    case "secondUserGitHub":
                        return child2?.Fields?[ID.Parse(Templates.TeamMember.Fields.GitHubUser)];
                    case "thirdUserGitHub":
                        return child3?.Fields?[ID.Parse(Templates.TeamMember.Fields.GitHubUser)];
                    default:
                        return "default";
                }
            }
            catch(Exception ex)
            {
                return "";
            }
        }
    }
}