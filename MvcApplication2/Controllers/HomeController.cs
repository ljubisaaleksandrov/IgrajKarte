using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IgrajKarte.Helpers;

namespace IgrajKarte.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewData["Session"] = Session["CurrentUserId"];

            if (Session["CurrentUserId"] == null)
            {
                Session["CurrentUserId"] = -1;
                Session["isMember"] = false;
                Session["isAdministrator"] = false;
                ViewBag.IsAuthenticated = false;
                ViewBag.IsMember = false;
                ViewBag.IsAdministrator = false;
            }
            else if ((int)Session["CurrentUserId"] == -1)
            {
                Session["isMember"] = false;
                Session["isAdministrator"] = false;
                ViewBag.IsAuthenticated = false;
                ViewBag.IsMember = false;
                ViewBag.IsAdministrator = false;
            }
            return View();
        }
    }
}
