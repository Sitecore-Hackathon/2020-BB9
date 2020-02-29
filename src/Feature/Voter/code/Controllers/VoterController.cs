using Hackathon.BB9.Feature.Voter.Models;
using Sitecore.Services.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hackathon.BB9.Feature.Voter.Controllers
{
    [ServicesController("hackathon/voter")]
    public class VoterController : Controller
    {
        [HttpGet]
        public ActionResult GetTeams()
        {
            var result = new List<dynamic>()
            {
                new
                {
                    Id = "1",
                    Name = "Team 1"
                },
                new
                {
                    Id = "2",
                    Name = "Team 2"
                },
                new
                {
                    Id = "3",
                    Name = "Team 3"
                },
                new
                {
                    Id = "4",
                    Name = "Team 4"
                },
                new
                {
                    Id = "5",
                    Name = "Team 5"
                },
                new
                {
                    Id = "6",
                    Name = "Team 6"
                },
                new
                {
                    Id = "7",
                    Name = "Team 7"
                },
                new
                {
                    Id = "8",
                    Name = "Team 8"
                },
                new
                {
                    Id = "9",
                    Name = "Team 9"
                },
                new
                {
                    Id = "10",
                    Name = "Team 10"
                }
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopics()
        {
            var result = new List<Topic>()
            {
                new Topic
                {
                    Id = "1",
                    Title = "JSS"
                },
                new Topic
                {
                    Id = "2",
                    Title = "xConnect"
                },
                new Topic
                {
                    Id = "3",
                    Title = "SXA"
                },
                new Topic
                {
                    Id = "4",
                    Title = "SPE"
                }
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(Team[] teams)
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}