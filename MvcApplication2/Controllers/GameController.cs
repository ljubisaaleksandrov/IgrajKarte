using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgrajKarte.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        //
        // GET: /Game/

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult GetTitle(string val)
        {
            return Json(new { status = true, tri = "tri" });
        }

        [HttpPost]
        public String Stats()
        {
            
            return "Partial View";
        }

    }
}
