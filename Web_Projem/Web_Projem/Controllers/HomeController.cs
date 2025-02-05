﻿using FireSharp.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using Web_Projem.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp;

namespace Web_Projem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        FirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "gcXSL3OsYxwnTQ4l2MlbJ2O1LN3k8gD1ZLbSO0Pp",
            BasePath = "https://crsmartled-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
                     
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Paylasim");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Paylasim>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Paylasim>(((JProperty)item).Value.ToString()));
            }
            
            ViewBag.Liste = list;
            return View();
        }
        [HttpGet]
        public IActionResult KalpA(int id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Paylasim");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Paylasim>();
            Boolean dogru = false;
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Paylasim>(((JProperty)item).Value.ToString()));

            }
            foreach (var item in list)
            {
                if (item.Id==id)
                {
                    dogru = true;
                    item.Begeni += 1;
                    SetResponse respone = client.Set("Paylasim/" + item.Id, item);
                    return RedirectToAction("Index");
                }
            }
            return Ok(id);
           
          
        }
    }
}