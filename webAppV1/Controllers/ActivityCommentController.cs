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
    public class ActivityCommentController : Controller
    {
        HttpClient httpClient;
        string baseAddress;
        public ActivityCommentController()
        {
            baseAddress = "http://localhost:8081/ActivityComment/";
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // var _AccessToken = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJtZWQuYWhlZEBlc3ByaXQudG4kMmEkMTAkWHJlSzdaZUkwZ2xMNFFsTFpKNm1lTzhxNTBUQzdwdFZidWk4OWhmZjZnUERiNWl2aTZkaS5bUk9MRV9BRE1JTl0iLCJpYXQiOjE2MTg2MjAyOTgsImV4cCI6MTYxMjcxNDM4M30.geYwLgC7H47ALR2JqQrG8u5pYcK88QgxB3TYVgQhlcs";
            // httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", _AccessToken));
        }
        // GET: Question_Satisfaction
        public ActionResult GetAllActivityComment()
        {
            var tokenResponse = httpClient.GetAsync(baseAddress + "retrieveAllActivityComment").GetAwaiter().GetResult();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var activitycomments = tokenResponse.Content.ReadAsAsync<IEnumerable<ActivityComment>>().Result;

                return View(activitycomments);
            }
            else
            {
                var activitycomments = tokenResponse.Content.ReadAsAsync<IEnumerable<ActivityComment>>().GetAwaiter().GetResult();

                return View(activitycomments);
            }

        }
        public ActionResult getActivityCommentById(int id)
        {
            var tokenResponse = httpClient.GetAsync(baseAddress + "retrieveActivityCommentById/" + id).GetAwaiter().GetResult();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var activitycomment = tokenResponse.Content.ReadAsAsync<ActivityComment>().Result;

                return View(activitycomment);
            }
            else
            {
                var activitycomment = tokenResponse.Content.ReadAsAsync<ActivityComment>().GetAwaiter().GetResult();

                return View(activitycomment);
            }

        }
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActivityComment activitycomment)
        {

            try
            {

                var APIResponse = httpClient.PostAsJsonAsync<ActivityComment>(baseAddress + "AddactivityComment/", activitycomment).GetAwaiter().GetResult();
                if (APIResponse.IsSuccessStatusCode)
                {

                    return RedirectToAction("GetAllActivityComment");
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
            var tokenResponse = httpClient.GetAsync(baseAddress+"retrieveActivityCommentById/"+id).GetAwaiter().GetResult();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var activitycomment = tokenResponse.Content.ReadAsAsync<ActivityComment>().Result;

                return View(activitycomment);
            }
            else
            {

                return View(new ActivityComment());
            }
        }


        [HttpPost]
        public ActionResult Edit(int id, ActivityComment activitycomment)
        {
            try
            {

                // var APIresponse = httpClient.PutAsJsonAsync<Question_Satisfaction>(baseAddress+"Updatequestion/"+id, question_Satisfaction).GetAwaiter().GetResult();
                var APIresponse = httpClient.PutAsJsonAsync<ActivityComment>(baseAddress+"updateActivityComment/"+id, activitycomment).ContinueWith(putTask => putTask.Result.EnsureSuccessStatusCode());

                return RedirectToAction("GetAllActivityComment");
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

            HttpResponseMessage response = httpClient.DeleteAsync(baseAddress + "deleteActivityCommentById/" + id).Result;


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllActivityComment");
            }
            return RedirectToAction("GetAllActivityComment");
        }
    }
}