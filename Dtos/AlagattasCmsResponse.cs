using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Dtos
{
    public class AlagattasCmsResponse
    {
        public object message { get; set; }
        public string responseStatus { get; set; }
        public Accessinfo accessInfo { get; set; }
        public Profile profile { get; set; }
    }

    public class Accessinfo
    {
        public Access access { get; set; }
    }

    public class Access
    {
        public string message { get; set; }
        public Token token { get; set; }
    }

    public class Token
    {
        public Datetime expires { get; set; }
        public object id { get; set; }
        public object UDID { get; set; }
        public Permissions permissions { get; set; }
    }

    public class Permissions
    {
        public string[] orgGroups { get; set; }
        public string[] channelGroups { get; set; }
    }

    public class Profile
    {
        public object message { get; set; }
        public object responseStatus { get; set; }
        public string UDID { get; set; }
        public object name { get; set; }
        public object age { get; set; }
        public object gender { get; set; }
        public object maritalStatus { get; set; }
        public object familySize { get; set; }
        public object lat { get; set; }
        public object lon { get; set; }
        public object birthDay { get; set; }
        public object imageURL { get; set; }
        public string sessionId { get; set; }
        public object FBId { get; set; }
        public object expiry { get; set; }
        public string email { get; set; }
        public object workLocationLat { get; set; }
        public object workLocationLong { get; set; }
        public object workLocationAddress { get; set; }
        public object homeLocationAddress { get; set; }
        public object homeLocationLat { get; set; }
        public object homeLocationLong { get; set; }
        public int id { get; set; }
        public bool isFBIdAlreadyExists { get; set; }
        public object[] tagList { get; set; }
    }

}
