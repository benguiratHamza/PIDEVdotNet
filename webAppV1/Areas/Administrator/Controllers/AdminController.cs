using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using webAppV1.Models;

namespace webAppV1.Areas.Administrator.Controllers
{
    public class AdminController : Controller
    {
        
        public async Task<ActionResult> Index()
        {
            if (Session["jwt"] != null)
            {
                List<User> users = new List<User>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:8088/Spring/pi/");

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("Application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["jwt"].ToString()); ;
                    HttpResponseMessage res = await client.GetAsync("Admin/User/getAllUsers");
                    if (res.IsSuccessStatusCode)
                    {
                        var userResponse = res.Content.ReadAsStringAsync().Result;
                        users = JsonConvert.DeserializeObject<List<User>>(userResponse);
                    }

                }

                return View(users);
            }
            return View("Index","Home",new { area =("")});
        
        }

        public ActionResult logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home", new { area = ("") });
        }
        [HttpGet]
        public ActionResult MyProfile()
        {
            Director director = Session["loggedInUser"] as Director;
            return View(director);
        }
        [HttpPost]
        public async Task<ActionResult> MyProfile(Director director)
        {
            using (var client = new HttpClient())
            {
                var directorChange = JsonConvert.SerializeObject(director,
               new JsonSerializerSettings
               {
                   ContractResolver = new CamelCasePropertyNamesContractResolver()
               });
                client.BaseAddress = new Uri("http://localhost:8088/Spring/pi/");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session["jwt"].ToString());
                HttpResponseMessage res = await client.PostAsync("updateDirector", new StringContent(directorChange, Encoding.UTF8, "application/json"));
                if (res.IsSuccessStatusCode)
                {
                    res = await client.GetAsync("getDirectorById/"+ (Session["loggedInUser"] as Director).Id);
                    var userResponse = res.Content.ReadAsStringAsync().Result;
                    Director loggedInUser = JsonConvert.DeserializeObject<Director>(userResponse);
                    Session["loggedInUser"] = loggedInUser;

                    TempData["directorUpdated"] = "Updated";
                    return RedirectToAction("MyProfile", loggedInUser);
                }
            }
            TempData["directorNotUpdated"] = "Error";
            return RedirectToAction("MyProfile",Session["loggedInUser"] as Director);
        }
    }
}