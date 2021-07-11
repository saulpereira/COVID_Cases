using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COVID_Cases.Models
{
    public class DataCountries
    {
        public DateTime tdate { get; set; }
        public int confirmed { get; set; }
        public int deaths { get; set; }
        public int recovered { get; set; }
        public int confirmed_diff { get; set; }

        public int deaths_diff { get; set; }
        public int recovered_diff { get; set; }
        public DateTime last_update { get; set; }
        public int active { get; set; }
        public int active_diff { get; set; }
        public double fatality_rate { get; set; }
        public region Region { get; set; }

    }
}