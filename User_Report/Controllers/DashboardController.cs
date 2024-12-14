using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace User_Report.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Admin_Dashboard()
        {
            return View();
        }
        public ActionResult operation_Dashboard()
        {
            return View();
        }

        public ActionResult SME_Dashboard()
        {
            return View();
        }

        public ActionResult Accounts_Dashboard()
        {
            return View();
        }
        public ActionResult Legal_Dashboard()
        {
            return View();
        }

        public ActionResult User_Dashboard()
        {
            return View();
        }

    }
}