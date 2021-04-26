using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using webAppV1.Models;

namespace webAppV1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        public async Task<ActionResult> Users()
        {
            if (Session["jwt"] != null)
            {
                List<User> users = new List<User>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:8088/Spring/pi/");

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("Application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJoYW16YSIsImNyZWF0ZWQiOjE2MTg0Mzk2NDIwOTcsImV4cCI6MTYxOTA0NDQ0Mn0.4DJcFDXfrTfCsjOj5XGatLwPnFvQXMNYkL-oOj6Q_H0LjSJumsscH9Stm-Bh4HMoIWXvvgO83uR2gl1Qf5IFgA"); ;
                    HttpResponseMessage res = await client.GetAsync("Admin/User/getAllUsers");
                    if (res.IsSuccessStatusCode)
                    {
                        var userResponse = res.Content.ReadAsStringAsync().Result;
                        users = JsonConvert.DeserializeObject<List<User>>(userResponse);
                    }

                }

                return View(users);
            }
            return View();
        }
        public ActionResult getTokenSub()
        {
            var jwToken = new JwtSecurityToken("eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJoYW16YSIsImNyZWF0ZWQiOjE2MTg4NzU0MTkwNjksImV4cCI6MTYxOTQ4MDIxOX0.QutluisJcr9LlIAop9Z6dxuQUEySJv7FKDxfIRd7nUybV0JcikI1F2-Q_-ezk736TC7S6e89FgGg7SuG8r8wQw");
            
            ViewBag.myString=jwToken.Subject;
            return View();
        }
    }
}