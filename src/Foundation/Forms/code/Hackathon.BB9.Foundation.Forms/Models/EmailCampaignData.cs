using System;
using Newtonsoft.Json;

namespace Hackathon.BB9.Foundation.Forms.Models
{
    public class EmailCampaignData
    {
        [JsonProperty("{selectEmailFieldId}")]
        public Guid EmailCampaignId { get; set; }
    }
}