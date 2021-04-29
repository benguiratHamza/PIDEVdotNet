using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using webAppV1.Models;

namespace webAppV1.Controllers
{
    public class ActivityController : Controller
    {
        HttpClient httpClient;
        string baseAddress;
        public ActivityController()
        {
            baseAddress = "http://localhost:8081/activity/";
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // var _AccessToken = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJtZWQuYWhlZEBlc3ByaXQudG4kMmEkMTAkWHJlSzdaZUkwZ2xMNFFsTFpKNm1lTzhxNTBUQzdwdFZidWk4OWhmZjZnUERiNWl2aTZkaS5bUk9MRV9BRE1JTl0iLCJpYXQiOjE2MTg2MjAyOTgsImV4cCI6MTYxMjcxNDM4M30.geYwLgC7H47ALR2JqQrG8u5pYcK88QgxB3TYVgQhlcs";
            // httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", _AccessToken));
        }
        // GET: Question_Satisfaction
        public ActionResult GetAllActivitys()
        {
            var tokenResponse = httpClient.GetAsync(baseAddress+"retrieveAllActivity").GetAwaiter().GetResult();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var activitys = tokenResponse.Content.ReadAsAsync<IEnumerable<Activity>>().Result;

                return View(activitys);
            }
            else
            {
                var activitys = tokenResponse.Content.ReadAsAsync<IEnumerable<Activity>>().GetAwaiter().GetResult();

                return View(activitys);
            }

        }
        public ActionResult getActivityById(int id)
        {
            var tokenResponse = httpClient.GetAsync(baseAddress + "getActivityById/" + id).GetAwaiter().GetResult();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var activity = tokenResponse.Content.ReadAsAsync<Activity>().Result;

                return View(activity);
            }
            else
            {
                var activity = tokenResponse.Content.ReadAsAsync<Activity>().GetAwaiter().GetResult();

                return View(activity);
            }

        }
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Activity activity)
        {

            try
            {

                var APIResponse = httpClient.PostAsJsonAsync<Activity>(baseAddress + "addactivty/", activity).GetAwaiter().GetResult();
                TempData["seccussmessage"] = "save seccuss";
                var message = APIResponse.ToString();
                if (APIResponse.IsSuccessStatusCode)
                {

                    return RedirectToAction("GetAllActivitys");
                }
                else
                {
                    return RedirectToAction("Create");
                }

            }
            catch
            {
                return View();
            }

        }




        public ActionResult Edit(int id)
        {
            var tokenResponse = httpClient.GetAsync(baseAddress + "retrieveActivityById/" + id).GetAwaiter().GetResult();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var activity = tokenResponse.Content.ReadAsAsync<Activity>().Result;

                return View(activity);
            }
            else
            {

                return View(new Activity());
            }
        }


        [HttpPost]
        public ActionResult Edit(int id, Activity activity)
        {
            try
            {

                // var APIresponse = httpClient.PutAsJsonAsync<Question_Satisfaction>(baseAddress+"Updatequestion/"+id, question_Satisfaction).GetAwaiter().GetResult();
                var APIresponse = httpClient.PutAsJsonAsync<Activity>(baseAddress+"updateactivity/"+id,activity).ContinueWith(putTask => putTask.Result.EnsureSuccessStatusCode());

                return RedirectToAction("GetAllActivitys");
            }
            catch
            {
                return View();
            }
        }

        // GET: Message/DeleteById/5
        public ActionResult Delete()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {

            HttpResponseMessage response = httpClient.DeleteAsync(baseAddress + "deleteActivity/" + id).Result;


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllActivitys");
            }
            return RedirectToAction("GetAllActivitys");
        }
    }
}