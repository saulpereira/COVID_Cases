﻿using COVID_Cases.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COVID_Cases.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://covid-19-statistics.p.rapidapi.com/");
            cliente.DefaultRequestHeaders.Add("x-rapidapi-key", "ccc4385e04mshf4ed1ca42485851p15dd54jsn577c8b2c68c7");
            cliente.DefaultRequestHeaders.Add("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com");

            RegionsViewModel model = new RegionsViewModel();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var request = cliente.GetAsync("regions").Result;
            RegionHolder regions = new RegionHolder();

            if (request.IsSuccessStatusCode)
            {
                var resultString = request.Content.ReadAsStringAsync().Result;
                regions  = JsonConvert.DeserializeObject<RegionHolder>(resultString, settings);

                model.Regions = this.GetRegions(regions);
            }

            request = cliente.GetAsync("reports").Result;
            Countries countries = new Countries();
            
            if (request.IsSuccessStatusCode)
            {
                var resultString = request.Content.ReadAsStringAsync().Result;
                countries = JsonConvert.DeserializeObject<Countries>(resultString, settings);

                model.dataCountries =  this.GetTopCountries(countries);
                //this.GetTopCountries(countries);
            }


            return View(model);
        }

        public IEnumerable<SelectListItem> GetRegions(RegionHolder regions1)
        {
            List<SelectListItem> item = regions1.data.ConvertAll(a =>
           {
               return new SelectListItem()
               {
                   Text = a.name,
                   Value = a.iso
               };
           });

            item.Insert(0, new SelectListItem
            {
                Text = "(Select a Region...)",
                Value = "0"
            }); ;

            return item;
        }

        public IEnumerable<CountriesSelected> GetTopCountries(Countries countries)
        {

            List<CountriesSelected> lista = new List<CountriesSelected>();
            CountriesSelected countries1 = new CountriesSelected();
            string search = "US";

            //lista = countries.data.OrderByDescending(x => x.confirmed).Take(10).ToList();

            //var countries2 = countries.data
            var countries2 = countries.data
                .GroupBy(y => y.Region.iso)
                .OrderByDescending(group => group.Sum(c => c.confirmed))
                .Take(10)
                .Select(group => new { Name = group.Key, Cases = group.Sum(a => a.confirmed), Deaths = group.Sum(b => b.deaths) })
                .ToList();
              
            foreach (var item in countries2)
            {
                lista.Add(new CountriesSelected { Name = item.Name, cases = item.Cases, deaths = item.Deaths });
            }

            return lista;
        }

       

        

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}