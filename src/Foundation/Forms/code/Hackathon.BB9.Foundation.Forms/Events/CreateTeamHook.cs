using System;

using Sitecore.Eventing;
using Sitecore.Events.Hooks;

namespace Hackathon.BB9.Foundation.Forms.Events
{
    public class CreateTeamHook : IHook
    {
        public void Initialize()
        {
            EventManager.Subscribe<CreateTeamEvent>(new Action<CreateTeamEvent>(CreateTeamEventHandler.Run));
        }
    }
}