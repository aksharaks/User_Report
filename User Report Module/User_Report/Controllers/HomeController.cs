using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_Report.App_Code;
using User_Report.Entities;

namespace User_Report.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //string sql2 = "SELECT ROUND(SUM(amt_today_outstanding) / 10000000, 2) as GL_AUM FROM tbl_collateral_growth_report WHERE TRUNC(sys_date) = TRUNC(SYSDATE) - 1";
            //Dictionary<string, dynamic> parameter2 = new Dictionary<string, dynamic>
            //{

            //};
            //DashBoardCounts GLAUM = MaafinDbHelper.ExecuteFormQueryData<List<DashBoardCounts>>(sql2, parameter2)?.FirstOrDefault();
            //TempData["GL_AUM"] = GLAUM?.GL_AUM;

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
    }
}