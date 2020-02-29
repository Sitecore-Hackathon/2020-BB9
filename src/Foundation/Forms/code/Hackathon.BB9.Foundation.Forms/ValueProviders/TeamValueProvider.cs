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

            switch(parameters)
            {
                case "teamName":
                    return "teamName";
                case "mainEmail":
                    return "mainEmail";
                case "country":
                    return "country";
                case "gitHubUsername":
                    return "gitHubUsername";
                default:
                    return "default";
            }
        }
    }
}