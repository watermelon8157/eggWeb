using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eggWeb.Controllers
{
    public class CountyController : Controller
    {
        [ChildActionOnly]
        // GET: County
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}