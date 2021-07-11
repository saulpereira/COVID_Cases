using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COVID_Cases.Models
{
    public class region
    {
        public string iso { get; set; }
        public string name { get; set; }
        public string province { get; set; }
        public double lat { get; set; }
        public double longitude { get; set; }
        //public string cities { get; set; }
    }
}