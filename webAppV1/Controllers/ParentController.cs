using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webAppV1.Models;

namespace webAppV1.Controllers
{
    public class ParentController : Controller
    {
        // GET: Parent
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateInformation()
        {
            Parent parent = (Parent)Session["loggedInUser"];
            return View(parent);
        }
    }
}