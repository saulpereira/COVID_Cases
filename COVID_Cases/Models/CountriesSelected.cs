using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COVID_Cases.Models
{
    public class CountriesSelected
    {
        public string Name { get; set; }
        [DisplayFormat(DataFormatString ="{0:n0}")]
        public int cases { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public int deaths { get; set; }
    }
}