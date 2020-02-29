namespace Hackathon.BB9.Foundation.Forms
{
    public static class Templates
    {
        public static string EmailCampaignMessageId = "{C7175C0C-F142-40E2-9645-D6B33E9B920A}";

        public static class TeamFolder
        {
            public static string TemplateId = "{6642799F-838E-461D-86B2-B99051D8DC8E}";
        }

        public static class HackathonSettings
        {
            public static string TemplateId = "{7E257750-F6B8-4C07-B353-5D789B2B6BDE}";

            public static class Fields
            {
                public static string ActualContest = "{0F58B054-9C31-4FF1-97CB-BAA1A015B4AD}";
            }
        }

        public static class Team
        {
            public static string TemplateId = "{16B71753-E859-41CC-9343-DB2C26612E1B}";

            public static class Fields
            {
                public static string Name = "{398A304B-6ADE-4480-8C8B-83A05EEEAD38}";
                public static string ContactEmail = "{127A496F-D255-428C-877C-88C574E10507}";
                public static string ContactGitHubProfile = "{EDD9DDE4-4F30-4D04-8512-3FDCF925515F}";
                public static string Country = "{65BBC0C0-F014-43E9-9D70-D4DDEA6B2708}";
            }
        }

        public static class TeamMember
        {
            public static string TemplateId = "{741992D9-83CA-49B7-9A69-8CC7AF3149F9}";

            public static class Fields
            {
                public static string Name = "{FC0B8189-E74C-4B8F-91D6-3C465DCEE74B}";
                public static string LinkedInProfile = "{92230E46-D534-45F4-A64B-842B2309CF89}";
                public static string TwitterProfile = "{EEC2B56F-F2DA-4BA0-8010-5A61980D0634}";
                public static string GitHubUser = "{9B1BB466-5594-48CA-A3D0-D27EE256FBBB}";
            }
        }
    }
}