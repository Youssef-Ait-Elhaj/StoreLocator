using System;
using System.Collections.Generic;
// using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using store_locator.Models;
using System.Net.Http;
using Newtonsoft.Json;


namespace store_locator.Controllers
{
    public class StoreController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

     // GET
        public IActionResult Index()
        {
            return Content("home");
        }

        [HttpPost]
        [Route("/stores")]
        public async Task<IActionResult> getStores(string address)    // loc is the location provided by the user
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(store_locator.Models.StoreContext)) 
                as StoreContext;

            // geocode address to get latitude and longtitude
            var msg = await client.GetStringAsync("http://open.mapquestapi.com/geocoding/v1/address?key=52YfEnPGDpkwfmKBphJ4egihwH6w09K8&location=" + address);
            // // parse json to dynamic and access lat and lng
            dynamic obj = JsonConvert.DeserializeObject(msg);
            double lat = obj.results[0].locations[0].latLng.lat;
            double lng = obj.results[0].locations[0].latLng.lng;
            
            List<Store> nearbyStores = context.getStores(lat, lng);
            Console.WriteLine("{0} nearby stores", nearbyStores.Count);
            foreach (Store st in nearbyStores)
            {
                Console.WriteLine(st.address);
            }
            
            return  View("Stores", nearbyStores);
        }
    }
}