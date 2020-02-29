using Hackathon.BB9.Feature.Voter.Models;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.SecurityModel;
using Sitecore.Services.Core;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Hackathon.BB9.Feature.Voter.Controllers
{
    [ServicesController("hackathon/voter")]
    public class VoterController : Controller
    {
        private const string StartItemIdSettingKey = "Hackathon.BB9.Feature.Voter.StartItemId";

        private static ID TeamFolderTemplateId = new ID("{6642799F-838E-461D-86B2-B99051D8DC8E}");

        private static ID TopicFolderTemplateId = new ID("{2F8CBAA0-8100-4C23-8606-BD37FF9BC52F}");

        private readonly Database _database;

        public VoterController()
        {
            _database = Factory.GetDatabase("master");
        }

        [HttpGet]
        public ActionResult GetTeams()
        {
            var startItemIdSetting = Sitecore.Configuration.Settings.GetSetting(StartItemIdSettingKey);
            if (string.IsNullOrWhiteSpace(startItemIdSetting))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Missing StartItemId setting.");
            }

            var startItemId = ID.Parse(startItemIdSetting);
            var startItem = _database.GetItem(startItemId);
            if (startItem == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "StartItem not found.");
            }

            var actualContestItem = (ReferenceField)startItem.Fields["Actual Contest"];
            if (actualContestItem == null || actualContestItem.TargetItem == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Actual Contest item is not set.");
            }

            var teamsFolder = actualContestItem.TargetItem.GetChildren().FirstOrDefault(c => c.TemplateID == TeamFolderTemplateId);
            if (teamsFolder == null || (!teamsFolder.Children?.Any() ?? true))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "No Teams folder found under the Actual Contest item.");
            }

            var result = teamsFolder.Children?.Select(c =>
            {
                var topic = (ReferenceField)c.Fields["Topic"];
                return new Team
                {
                    Id = c.ID.Guid,
                    Name = c["Name"],
                    TopicId = topic != null && topic.TargetItem != null ? topic.TargetID.Guid : (Guid?)null,
                    TopicName = topic != null && topic.TargetItem != null ? topic.TargetItem["Name"] : null
                };
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopics()
        {
            var startItemIdSetting = Settings.GetSetting(StartItemIdSettingKey);
            if (string.IsNullOrWhiteSpace(startItemIdSetting))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Missing StartItemId setting.");
            }

            var startItemId = ID.Parse(startItemIdSetting);
            var startItem = _database.GetItem(startItemId);
            if (startItem == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "StartItem not found.");
            }

            var actualContestItem = (ReferenceField)startItem.Fields["Actual Contest"];
            if (actualContestItem == null || actualContestItem.TargetItem == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Actual Contest item is not set.");
            }

            var topicsFolder = actualContestItem.TargetItem.GetChildren().FirstOrDefault(c => c.TemplateID == TopicFolderTemplateId);
            if (topicsFolder == null || (!topicsFolder.Children?.Any() ?? true))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "No Topics folder found under the Actual Contest item.");
            }

            var result = topicsFolder.Children?.Select(c => new Topic
            {
                Id = c.ID.Guid,
                Title = c["Name"],
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(Team[] teams)
        {
            if (teams == null)
            {
                new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Given teams parameter is null.");
            }

            using (new BulkUpdateContext())
            using (new SecurityDisabler())
            {
                foreach (var team in teams.Where(t => t.TopicId != null))
                {
                    var teamItem = _database.GetItem(ID.Parse(team.Id));
                    if (teamItem == null)
                    {
                        continue;
                    }

                    teamItem.Editing.BeginEdit();
                    try
                    {
                        teamItem["Topic"] = ID.Parse(team.TopicId).ToString();
                        teamItem.Editing.EndEdit();
                    }
                    catch (Exception)
                    {
                        teamItem.Editing.CancelEdit();
                    }
                }
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}