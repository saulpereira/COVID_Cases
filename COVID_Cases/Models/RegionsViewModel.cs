using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COVID_Cases.Models
{
    public class RegionsViewModel
    {
        public string iso { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }

        public IEnumerable<CountriesSelected> dataCountries { get; set; }
    }
}