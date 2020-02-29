using Sitecore.Pipelines;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hackathon.BB9.Feature.Voter.Processors
{
    public class RegisterRoutes
    {
        public virtual void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("WidgetsRoute", "api/hackathon/voter/{controller}/{action}");
        }
    }
}