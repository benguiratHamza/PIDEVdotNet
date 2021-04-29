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
    public class AppointmentController : Controller
    {
        HttpClient httpClient;
        string baseAddress;
        public AppointmentController()
        {
            baseAddress = "http://localhost:8081/appointment/";
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // var _AccessToken = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJtZWQuYWhlZEBlc3ByaXQudG4kMmEkMTAkWHJlSzdaZUkwZ2xMNFFsTFpKNm1lTzhxNTBUQzdwdFZidWk4OWhmZjZnUERiNWl2aTZkaS5bUk9MRV9BRE1JTl0iLCJpYXQiOjE2MTg2MjAyOTgsImV4cCI6MTYxMjcxNDM4M30.geYwLgC7H47ALR2JqQrG8u5pYcK88QgxB3TYVgQhlcs";
            // httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", _AccessToken));
        }
        // GET: Question_Satisfaction
        public ActionResult GetAllAppointments()
        {
            var tokenResponse = httpClient.GetAsync(baseAddress + "retrieveAllappointments").GetAwaiter().GetResult();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var appointment = tokenResponse.Content.ReadAsAsync<IEnumerable<Appointment>>().Result;

                return View(appointment);
            }
            else
            {

                var appointment = tokenResponse.Content.ReadAsAsync<IEnumerable<Appointment>>().Result;

                return View(appointment);
            }

        }
        public ActionResult getAppointmentById(int id)
        {
            var tokenResponse = httpClient.GetAsync(baseAddress + "retrieveAllAppointmentById/" + id).GetAwaiter().GetResult();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var appointment = tokenResponse.Content.ReadAsAsync<IEnumerable<Appointment>>().Result;

                return View(appointment);
            }
            else
            {
                var appointment = tokenResponse.Content.ReadAsAsync<IEnumerable<Appointment>>().GetAwaiter().GetResult();

                return View(appointment);
            }

        }
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment)
        {

            try
            {

                var APIResponse = httpClient.PostAsJsonAsync<Appointment>(baseAddress + "addappointment/", appointment).GetAwaiter().GetResult();
 ;
                if (APIResponse.IsSuccessStatusCode)
                {

                    return RedirectToAction("GetAllAppointments");
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
            var tokenResponse = httpClient.GetAsync(baseAddress + "retrieveAppointmentById/" + id).GetAwaiter().GetResult();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var appointment = tokenResponse.Content.ReadAsAsync<Appointment>().Result;

                return View(appointment);
            }
            else
            {

                return View(new Appointment());
            }
        }


        [HttpPost]
        public ActionResult Edit(int id, Appointment appointment)
        {
            try
            {

                // var APIresponse = httpClient.PutAsJsonAsync<Question_Satisfaction>(baseAddress+"Updatequestion/"+id, question_Satisfaction).GetAwaiter().GetResult();
                var APIresponse = httpClient.PutAsJsonAsync<Appointment>(baseAddress + "updateappointment/"+id, appointment).ContinueWith(putTask => putTask.Result.EnsureSuccessStatusCode());

                return RedirectToAction("GetAllAppointments");
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
            
            HttpResponseMessage response = httpClient.DeleteAsync(baseAddress+"deleteappointment/" + id).Result;
       

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllAppointments");
            }
            return RedirectToAction("GetAllAppointments");
        }
    }
}