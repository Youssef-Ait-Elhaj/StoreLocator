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
using System.Web;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
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
            
            List<Store> nearbyStores = await context.getStores(lat, lng);

            // serialize nearbyStores object to load it into map as geoJSON data to be able to 
            // able to add markers to these locations
            string nearbyStoresAsString = JsonConvert.SerializeObject(nearbyStores, Formatting.Indented);
            
            List<Feature> mapFeatures = new List<Feature>();
            
            // For each position from the response create a Feature and add it to previously created list
            foreach (Store st in nearbyStores)
            {
                mapFeatures.Add(new Feature(
                    new Point(new Position(st.latitude, st.longtitude)),
                    new Dictionary<string, dynamic>() {{"address", st.address}}
                    ));
            }
            
            // Use the list to build a FeatureCollection
            FeatureCollection mapFeatureCollection = new FeatureCollection(mapFeatures);
            // FeatureCollection instances provides the method ToJson which converts them to Json strings, use it before sending the FeatureCollection to the frontend
            string nearbyStoresAsGeoJSON = JsonConvert.SerializeObject(mapFeatureCollection, Formatting.Indented);
            Console.WriteLine(nearbyStoresAsGeoJSON);
            
            // pass the json to view to mark them
            ViewBag.nearbyStores = nearbyStoresAsGeoJSON;
            ViewBag.msg = "hello";
            return View("Stores");
        }
    }
}