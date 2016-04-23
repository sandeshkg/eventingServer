using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Models
{
    public class EventDetails
    {
        public int id { get; set; }
        public string title { get; set; }
        public string venue { get; set; }
        public string iconImageURL { get; set; }
        //public string LastUpdatedUser { get; set; }
        //public string LastUpdatedOn { get; set; }
        public string description { get; set; }
        public DateTime startTime { get; set; }
        public string type { get; set; }
        public string duration { get; set; }
    }
    public class SpecEventDetails
    {
        public int id { get; set; }
        public string lastUpdateOn { get; set; }
    }
}
