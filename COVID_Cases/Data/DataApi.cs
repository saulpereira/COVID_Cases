using COVID_Cases.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace COVID_Cases.Data
{
    public class DataApi
    {
        public static IEnumerable<SelectListItem> GetRegions()
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://covid-19-statistics.p.rapidapi.com/");
            cliente.DefaultRequestHeaders.Add("x-rapidapi-key", "ccc4385e04mshf4ed1ca42485851p15dd54jsn577c8b2c68c7");
            cliente.DefaultRequestHeaders.Add("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com");


            RegionsViewModel model = new RegionsViewModel();
            Countries countries = new Countries();
            IEnumerable<SelectListItem> ListRegions;
            //IEnumerable<CountriesSelected> countriesList;
            // Reading Regions

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
                regions = JsonConvert.DeserializeObject<RegionHolder>(resultString, settings);

                return FormatRegions(regions);
            }

            return null;
        }

        public static IEnumerable<SelectListItem> FormatRegions(RegionHolder regions1)
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

            return item.OrderBy(x => x.Text);
        }

        public static IEnumerable<CountriesSelected> GetCountries()
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://covid-19-statistics.p.rapidapi.com/");
            cliente.DefaultRequestHeaders.Add("x-rapidapi-key", "ccc4385e04mshf4ed1ca42485851p15dd54jsn577c8b2c68c7");
            cliente.DefaultRequestHeaders.Add("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com");

            Countries countries = new Countries();
            IEnumerable<CountriesSelected> countriesList;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            // Reading Top 10 Countries
            var request = cliente.GetAsync("reports").Result;
            //Countries countries = new Countries();

            if (request.IsSuccessStatusCode)
            {
                var resultString = request.Content.ReadAsStringAsync().Result;
                countries = JsonConvert.DeserializeObject<Countries>(resultString, settings);

                countriesList = GetTopEntities(countries, "country");
                return countriesList;
                //countriesList = model.dataCountries;

                //return Json(countriesList);
            }

            return null;

            //IEnumerable<CountriesSelected> countriesList;
            //countriesList = GetProvinces("USA");
        }

        public static IEnumerable<CountriesSelected> GetTopEntities(Countries countries, string entity)
        {

            List<CountriesSelected> lista = new List<CountriesSelected>();
            CountriesSelected countries1 = new CountriesSelected();


            if (entity == "country")
            {
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
            }
            else //Provinces
            {
                var countries2 = countries.data
               .GroupBy(y => y.Region.province)
               .OrderByDescending(group => group.Sum(c => c.confirmed))
               .Take(10)
               .Select(group => new { Name = group.Key, Cases = group.Sum(a => a.confirmed), Deaths = group.Sum(b => b.deaths) })
               .ToList();

                foreach (var item in countries2)
                {
                    lista.Add(new CountriesSelected { Name = item.Name, cases = item.Cases, deaths = item.Deaths });
                }
            }

            return lista;
        }

        public JsonResult GetProvinces(string ProvinceId)
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://covid-19-statistics.p.rapidapi.com/");
            cliente.DefaultRequestHeaders.Add("x-rapidapi-key", "ccc4385e04mshf4ed1ca42485851p15dd54jsn577c8b2c68c7");
            cliente.DefaultRequestHeaders.Add("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com");

            UriBuilder builder = new UriBuilder("https://covid-19-statistics.p.rapidapi.com/reports");
            builder.Query = "iso=" + ProvinceId;

            Countries countries = new Countries();
            IEnumerable<CountriesSelected> countriesList;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var request = cliente.GetAsync(builder.Uri).Result;
            //Countries countries = new Countries();
            //IEnumerable<CountriesSelected> countriesList;

            if (request.IsSuccessStatusCode)
            {
                var resultString = request.Content.ReadAsStringAsync().Result;
                countries = JsonConvert.DeserializeObject<Countries>(resultString, settings);

                //model.dataCountries = this.GetTopProvinces(countries);
                countriesList = GetTopEntities(countries, "province");

                return Json(countriesList);
            }

            return null;
        }

        private JsonResult Json(IEnumerable<CountriesSelected> countriesList)
        {
            throw new NotImplementedException();
        }
    }
}