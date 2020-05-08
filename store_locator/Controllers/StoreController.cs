using System;
using System.Collections.Generic;
// using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using store_locator.Models;
using System.Net.Http.HttpClient;


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
        public async IActionResult getStores(string address)    // loc is the location provided by the user
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(store_locator.Models.StoreContext)) 
                as StoreContext;
            Console.WriteLine("LOC: {0}", address);

            // geocode address to get latitude and longtitude
            
            var msg = client.GetStringAsync("https://maps.googleapis.com/maps/api/geocode/json?
            address=");

            // return Redirect("/nearbystores");
            // foreach (Store st in context.getStores())
            // {
            //     Console.WriteLine(st.name);
            // }
        }

        [HttpGet]
        [Route("/nearbystores")]
        public IActionResult showStores() {
            return View("Stores");
        }
    }
}