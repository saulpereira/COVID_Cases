using COVID_Cases.Data;
using COVID_Cases.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace COVID_Cases.Controllers
{
    public class HomeController : Controller
    {
        IEnumerable<CountriesSelected> countries;
        public ActionResult Index()
        {
            RegionsViewModel model = new RegionsViewModel();
            

            //Getting Regions
            model.Regions = DataApi.GetRegions();

            countries = DataApi.GetCountries();
            model.dataCountries = countries;

            return View(model);
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
        
            if (request.IsSuccessStatusCode)
            {
                var resultString = request.Content.ReadAsStringAsync().Result;
                countries = JsonConvert.DeserializeObject<Countries>(resultString, settings);

                countriesList = DataApi.GetTopEntities(countries, "province");

                return Json(countriesList);
            }

            return null;
        }

        //public async Task<IActionResult> ConvertToJson(int? id)
        public ViewResult ConvertToXML()
        {
            countries = DataApi.GetCountries();

            XElement xml = new XElement("countries",
                                       from country in countries
                                       select new XElement("country",
                                                           new XElement("name", country.Name),
                                                           new XElement("cases", country.cases),
                                                           new XElement("deaths", country.deaths))
                                       );
            xml.Save(@"c:\temp\countries.xml");

            return View();
        }


        public ViewResult ConvertToJson()
        {
            string countriesFile = @"C:\Temp\countries.txt";
            
            countries = DataApi.GetCountries();

            string json = JsonConvert.SerializeObject(countries.ToArray());

            //write string to file
            System.IO.File.WriteAllText(countriesFile, json);

            return View();
        }

        public ViewResult ConvertToCSV()
        {
            string countriesFile = @"C:\Temp\countries.csv";
           
            countries = DataApi.GetCountries();
            SaveToTextFile(countries, countriesFile);

            return View();
        }

        public static void SaveToTextFile(IEnumerable<CountriesSelected> data, string filePath) 
        {
            List<string> lines = new List<string>();
            StringBuilder line = new StringBuilder();

            if (data == null || data.Count() == 0)
            {
                throw new ArgumentNullException("data", "You must populate the data parameter with at least one value.");
            }

            line.Append("Name,Cases,Deaths");
            lines.Add(line.ToString().Substring(0, line.Length - 1));

            foreach ( var item in data)
            {
                line = new StringBuilder();
                line.Append(item.Name + "," + item.cases + "," + item.deaths) ;
                lines.Add(line.ToString().Substring(0, line.Length - 1));
            };
           

            System.IO.File.WriteAllLines(filePath, lines);
        }
    }
}