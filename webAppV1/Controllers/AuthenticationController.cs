using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using webAppV1.Models;

namespace webAppV1.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8088/Spring/pi/");
                User loggedInUser;
                var loginInfo = new Dictionary<string, string>();
                loginInfo.Add("username", username);
                loginInfo.Add("password", password);
                var changePassObj = JsonConvert.SerializeObject(loginInfo,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("Application/json"));
                HttpResponseMessage res = await client.PostAsync("auth", new StringContent(changePassObj, Encoding.UTF8, "application/json"));
                string tockenString = "";
                if (res.IsSuccessStatusCode)
                {
                    var loginResponse = res.Content.ReadAsStringAsync().Result;
                    string[] subs = loginResponse.Split('"');
                    tockenString = subs[3];
                    ViewBag.tocken = tockenString;
                    Session["jwt"] = tockenString;
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session["jwt"].ToString());
                    res = await client.GetAsync("Admin/User/getUserByName/" + username);
                    if (res.IsSuccessStatusCode)
                    {
                        var userResponse = res.Content.ReadAsStringAsync().Result;
                        loggedInUser = JsonConvert.DeserializeObject<User>(userResponse);
                        string userType = loggedInUser.Type;
                        switch (userType)
                        {
                            case "Admin":
                                res = await client.GetAsync("getDirectorById/" + loggedInUser.Id);
                                userResponse = res.Content.ReadAsStringAsync().Result;
                                loggedInUser = JsonConvert.DeserializeObject<Director>(userResponse);
                                Session["loggedInUser"] = loggedInUser;
                                return RedirectToAction("Index", "Admin", new { area = "Administrator" });
                            case "Parent":
                                res = await client.GetAsync("getParentById/" + loggedInUser.Id);
                                userResponse = res.Content.ReadAsStringAsync().Result;
                                loggedInUser = JsonConvert.DeserializeObject<Parent>(userResponse);
                                Session["loggedInUser"] = loggedInUser;
                                return RedirectToAction("Index", "Home");
                            default: return RedirectToAction("Index");
                        }
                    }
                }
            }
            TempData["loginError"] = "Username and Password doesn't match";
            return RedirectToAction("Index");
        }

        public ActionResult logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home", new { area = ("") });
        }

        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }

        public async Task<ActionResult> CreateAccountParent(string txtParentName, string txtParentSurname, string txtParentPassword, string txtParentConfirmPassword,
            string txtParentEmail, string txtParentPhone, string txtParentAdress)
        {
            Parent newParent = new Parent
            {
                Name = txtParentName,
                Surname = txtParentSurname,
                Password = txtParentPassword,
                ConfirmPassword = txtParentConfirmPassword,
                Email = txtParentEmail,
                PhoneNumber = txtParentPhone,
                Adress = txtParentAdress,
                Type="Parent"
            };
            using (var client = new HttpClient())
            {
                var newParentJSON = JsonConvert.SerializeObject(newParent,
               new JsonSerializerSettings
               {
                   ContractResolver = new CamelCasePropertyNamesContractResolver()
               });
                client.BaseAddress = new Uri("http://localhost:8088/Spring/pi/");
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session["jwt"].ToString());
                HttpResponseMessage res = await client.PostAsync("addParent", new StringContent(newParentJSON, Encoding.UTF8, "application/json"));
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            TempData["createParentError"] = "Check your information please";

            return RedirectToAction("CreateAccount");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string username)
        {
            
            using (var client = new HttpClient())
            {
              
                client.BaseAddress = new Uri("http://localhost:8088/Spring/pi/");
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session["jwt"].ToString());
                HttpResponseMessage res = await client.PostAsync("forgot/"+username, null);
                if (res.IsSuccessStatusCode)
                {
                    TempData["forgotPassword"] = "Check your mail box";
                    return RedirectToAction("ResetPassword");
                }
            }
            TempData["forgotPasswordError"] = "User does not exists!";
            return RedirectToAction("ForgotPassword");
        }
        
        
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(string token,string password)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:8088/Spring/pi/");
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session["jwt"].ToString());
                HttpResponseMessage res = await client.PostAsync("reset/" + token+"/"+password, null);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            TempData["resetPasswordError"] = "Unvalid token!";
            return RedirectToAction("ResetPassword");
        }

    }
    
}