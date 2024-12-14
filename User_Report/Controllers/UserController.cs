using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_Report.Models;
using User_Report.App_Code;
using User_Report.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using User_Report.Filters;

namespace User_Report.Controllers
{
 
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserDashboard()
        {
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];

            //string LoginUser = "select em.emp_name from employee_master em where em.emp_code = :userId";
            string LoginUser = "select em.emp_name,l.dep_name from employee_master em, department_mst l where em.emp_code =:userId and em.department_id=l.dep_id";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  },
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
            Session["user_name"] = user?.emp_name;
            Session["departmentName"] = user?.dep_name;






            return View();

        }

        public ActionResult feedbackform()
        {

            ViewBag.Message = TempData["Message"];

            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];


            string LoginUser = "select em.emp_name,l.dep_name from employee_master em, department_mst l where em.emp_code =:userId and em.department_id=l.dep_id";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  },
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
            Session["user_name"] = user?.emp_name;
            Session["departmentName"] = user?.dep_name;


            // for fetching module name in dropdown

            string sql4 = "select i.module_name from MODULE_ASSIGN_MASTER t, internal_module_master i, department_mst d where d.firm_id = 4 and d.status = 1 and i.module_id = t.module_id and d.dep_id = t.department_id and t.department_id ='" + departmentID + "' order by t.created_date desc";

            // Execute the SQL query and retrieve data
            List<moduleName> Modulename = MaafinDbHelper.ExecuteFormQueryData<List<moduleName>>(sql4, new Dictionary<string, dynamic>());

            // Create a list of SelectListItem
            List<SelectListItem> Entry = new List<SelectListItem>();

            // Populate the SelectListItem list
            foreach (var modulename in Modulename)
            {
                Entry.Add(new SelectListItem
                {
                    Text = $" {modulename.module_name}",
                    Value = Convert.ToString(modulename.module_name)
                });
            }
            ViewBag.modulenames = Entry;


            // Define the SQL query to fetch data from the database for display report  of compliant
            string sql = "SELECT i.module_name, m.feedbackname, m.descriptions, m.enhancement, m.rating, m.created_by, m.created_date, e.emp_name FROM Module_feedback_Master m JOIN internal_module_master i ON i.module_id = m.module_id JOIN employee_master e ON m.created_by = e.emp_code WHERE m.created_by = '" + userId + "' order by m.created_date desc";



            // Create an empty dictionary to pass as parameters for the query
            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<Feedbacksave> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<Feedbacksave>>(sql, rptGen);

            // Initialize a new list to hold the processed ModuleSave objects
            List<Feedbacksave> Gview = new List<Feedbacksave>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                Feedbacksave ent = new Feedbacksave
                {
                  //  compliant_id = item.compliant_i
                    module_name = item.module_name,
                    feedbackname = item.feedbackname,
                   // compliantname = item.compliantname,
                   // priority = item.priority,
                    descriptions = item.descriptions,
                    enhancement = item.enhancement,
                    rating = item.rating,
                    //created_by = item.created_by,
                    emp_name = item.emp_name,
                    created_date = item.created_date,
                   // status = item.status,
                    //comments = item.comments,


                };

                Gview.Add(ent);
            }

            // Return the populated list to be used in the view
            return View(Gview);


        }
        public ActionResult feedbacksave(Feedbacksave f)
        {
            ViewBag.Message = TempData["Message"];
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];

            var feedbackname = f.feedbackname;
            var Descriptions = f.descriptions;
            var enhancement = f.enhancement;
            var Rating = f.rating;
            var modulename = f.module_name;




            string mod = "select v.module_id from Internal_module_Master v where v.module_name=:modulename";
            Dictionary<string, dynamic> parameter1 = new Dictionary<string, dynamic>
            {
                {"@modulename",modulename  },
            };
            Feedbacksave moduleid = MaafinDbHelper.ExecuteFormQueryData<List<Feedbacksave>>(mod, parameter1)?.FirstOrDefault();


            var moduleId = moduleid?.module_id;


            string LoginUser = "insert into Module_feedback_Master (Feedback_Id, Module_Id, Feedbackname, Descriptions, Enhancement, Status, Created_By, Created_Date, Rating) values (feedback_id.nextval,:moduleid,:feedbackname,:descriptions,:enhancement,1,:userId,sysdate,:rating)";


            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  }, {"moduleid", moduleId }, {"feedbackname",feedbackname  },{"@descriptions", Descriptions},{"@enhancement",  enhancement},{"@rating", Rating}


            };
            save Save = MaafinDbHelper.ExecuteFormQueryData<List<save>>(LoginUser, parameter)?.FirstOrDefault();




            TempData["Message"] = "Submitted successfully";
            TempData["Success"] = "Success";








            return RedirectToAction("feedbackform");


        }


        public ActionResult clear()
        {
            return RedirectToAction("feedbackform");
        }


        public ActionResult complaint()
        {
            ViewBag.Message = TempData["Message"];
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];

            //department name in drop
            string LoginUser = "select em.emp_name,l.dep_name from employee_master em, department_mst l where em.emp_code =:userId and em.department_id=l.dep_id";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  },
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
            Session["user_name"] = user?.emp_name;
            Session["departmentName"] = user?.dep_name;


            // for fetching module name in dropdown

            string sql4 = "select i.module_name from MODULE_ASSIGN_MASTER t, internal_module_master i, department_mst d where d.firm_id = 4 and d.status = 1 and i.module_id = t.module_id and d.dep_id = t.department_id and t.department_id ='" + departmentID + "' order by t.created_date desc";

            // Execute the SQL query and retrieve data
            List<moduleName> Modulename = MaafinDbHelper.ExecuteFormQueryData<List<moduleName>>(sql4, new Dictionary<string, dynamic>());

            // Create a list of SelectListItem
            List<SelectListItem> Entry = new List<SelectListItem>();

            // Populate the SelectListItem list
            foreach (var modulename in Modulename)
            {
                Entry.Add(new SelectListItem
                {
                    Text = $" {modulename.module_name}",
                    Value = Convert.ToString(modulename.module_name)
                });
            }
            ViewBag.modulenames = Entry;

            // Define the SQL query to fetch data from the database for display report  of compliant
            //string sql = "select c.compliant_id, i.module_name, c.compliantname, c.descriptions, case when c.priority = 1 then 'High' when c.priority = 2 then 'Medium' else 'Low' end as priority, c.created_by, c.created_date, e.emp_name, case when c.status = 1 then 'Requested' when c.status = 2 then 'Assign' when c.status = 3 then 'development started' when c.status = 4 then 'development completed' when c.status = 5 then 'QA started' when c.status = 6 then 'QA completed' when c.status =7 then 'Live'  when c.status =8 then 'compliant resubmitted'  when c.status = 9 then 'compliant closed' else 'No_Action_required' end as status,c.comments from internal_module_master i, Module_complaint_Master c, employee_master e where i.module_id = c.module_id and c.created_by = e.emp_code and c.department_id = '" + departmentID + "' order by c.created_date desc";


            string sql = "SELECT c.compliant_id, i.module_name, c.compliantname, c.descriptions, CASE WHEN c.priority = 1 THEN 'High' WHEN c.priority = 2 THEN 'Medium' ELSE 'Low' END AS priority,c.created_date, e1.emp_name AS created_by, e2.emp_name AS developer_name, CASE WHEN c.status = 1 THEN 'Requested' WHEN c.status = 2 THEN 'Assign' WHEN c.status = 3 THEN 'Development Started' WHEN c.status = 4 THEN 'Development Completed' WHEN c.status = 5 THEN 'QA Started' WHEN c.status = 6 THEN 'QA Completed' WHEN c.status = 7 THEN 'Live' WHEN c.status = 8 THEN 'Compliant Resubmitted' WHEN c.status = 9 THEN 'Compliant Closed' ELSE 'No_Action_Required' END AS status, c.comments FROM internal_module_master i JOIN Module_complaint_Master c ON i.module_id = c.module_id JOIN employee_master e1 ON c.created_by = e1.emp_code LEFT JOIN DEVELOPER_ASSIGN d ON c.compliant_id = d.compliant_id LEFT JOIN employee_master e2 ON d.developer_id = e2.emp_code WHERE c.department_id = '" + departmentID + "' ORDER BY c.created_date DESC";


            // Create an empty dictionary to pass as parameters for the query
            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<listcompliant> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<listcompliant>>(sql, rptGen);

            // Initialize a new list to hold the processed ModuleSave objects
            List<listcompliant> Gview = new List<listcompliant>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                listcompliant ent = new listcompliant
                {
                    compliant_id = item.compliant_id,
                    module_name = item.module_name,
                    compliantname = item.compliantname,
                    priority = item.priority,
                    descriptions = item.descriptions,
                    emp_name = item.emp_name,
                    created_by = item.created_by,
                    developer_name = item.developer_name,
                    //developer_id = item.developer_id,
                    created_date = item.created_date,
                    status = item.status,
                  
                   comments = item.comments,


                };

                Gview.Add(ent);
            }

            // Return the populated list to be used in the view
            return View(Gview);


        }
        public ActionResult compliantsave(Compliantsave c)
        {
            ViewBag.Message = TempData["Message"];
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];

            var modulename = c.module_name;
            var compliantname = c.compliantname;
            var descriptions = c.descriptions;
            var priority = c.priority;
             //var compliantid = c.compliant_id;



            //to fetch moduleid

            string mod = "select v.module_id from Internal_module_Master v where v.module_name=:modulename";
            Dictionary<string, dynamic> parameter1 = new Dictionary<string, dynamic>
            {
                {"@modulename",modulename},
            };
            Compliantsave moduleid = MaafinDbHelper.ExecuteFormQueryData<List<Compliantsave>>(mod, parameter1)?.FirstOrDefault();


            var moduleId = moduleid?.module_id;








            // to save compliant


            string LoginUser = "insert into Module_complaint_Master(compliant_id,Module_Id,Compliantname,Descriptions,Priority,Status,Created_By,Created_Date,department_id) values( compliant_id.nextval,:moduleid,:compliantname,:descriptions,:priority,1,:userId,sysdate,:departid)";

            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
                {
                    {"@userId",userId},{"@moduleid",moduleId}, {"@compliantname",compliantname},{"@descriptions",descriptions},{"@priority",priority},{"@departid",departmentID}


                };
            save Save = MaafinDbHelper.ExecuteFormQueryData<List<save>>(LoginUser, parameter)?.FirstOrDefault();

            //---------------------



    //        string HistoryUser = "insert into MODULE_COMPLAINT_History(history_id, compliant_id, module_id, compliantname, descriptions, priority, status, created_by, created_date, department_id, comments) " +
    //                     "values(history_id.nextval, compliant_id.currval, :moduleid, :compliantname, :descriptions, :priority, 1, :userId, sysdate, :departid, NULL)";
    //        Dictionary<string, dynamic> historyParameter = new Dictionary<string, dynamic>
    //{
    //    {"@userId", userId},
    //    {"@moduleid", moduleId},
    //    {"@compliantname", compliantname},
    //    {"@descriptions", descriptions},
    //    {"@priority", priority},
    //    {"@departid", departmentID}
    //};
    //        save HistorySave = MaafinDbHelper.ExecuteFormQueryData<List<save>>(HistoryUser, historyParameter)?.FirstOrDefault();




            TempData["Message"] = "Compliant Submitted successfully";
            TempData["Success"] = "Success";







            return RedirectToAction("complaint");


        }
        // compliant resubmission
        public ActionResult EditTicket(TicketSearch t)
        {
            ViewBag.Message = TempData["Message"];
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];
            var Compliantid = t.compliant_id;
            Session["ticketNumber"] = Compliantid;

            // search query for resubmitt compliant

            string TicketSearchQuery = "select c.compliant_id,i.module_name,c.compliantname,c.descriptions from module_complaint_master c,internal_module_master i where  i.module_id = c.module_id and c.compliant_id = :compliantid";
            //c.status = 1 and

            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@compliantid",Compliantid}

            };





            TicketSearch ticketDetails = MaafinDbHelper.ExecuteFormQueryData<List<TicketSearch>>(TicketSearchQuery, parameter)?.FirstOrDefault();


            TempData["compliant_id"] = ticketDetails?.compliant_id;
            TempData["module_name"] = ticketDetails?.module_name;
            TempData["compliant_name"] = ticketDetails?.compliantname;
            // TempData["priority"] = ticketDetails?.priority;
            TempData["description"] = ticketDetails?.descriptions;
            //  TempData["date"] = ticketDetails?.created_date;


            Session["complaint_id"] = ticketDetails?.compliant_id;












            return View();

        }

        public ActionResult SaveEditTicket(editTicket e, ticket_submit s)
        {
            ViewBag.Message = TempData["Message"];
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];
            //var compliantid = t.compliant_id;
            var TicketNumber = Session["ticketNumber"];
            var comment = e.comments;
            Session["ticketNumber"] = TicketNumber;

           // var start_date = s.start_date;
           

            /// for fetching start_date and end_date

            string selectDatesQuery = "select d.start_date,d.end_date from developer_assign d WHERE compliant_id = :compliantid";

            Dictionary<string, dynamic> selectDateParameters = new Dictionary<string, dynamic>
{
    {"@compliantid", TicketNumber}
};
            //var dateResult = MaafinDbHelper.ExecuteFormQueryData<List<dynamic>>(selectDatesQuery, selectDateParameters)?.FirstOrDefault();

            ticket_submit dateResult = MaafinDbHelper.ExecuteFormQueryData<List<ticket_submit>>(selectDatesQuery, selectDateParameters)?.FirstOrDefault();



            var startDate = dateResult?.start_date;
            var endDate = dateResult?.end_date;






            string countCommentsQuery = "select count(*) from MODULE_COMPLAINT_History where compliant_id = :compliantid";
            Dictionary<string, dynamic> countCommentsParameters = new Dictionary<string, dynamic>
    {
        {"@compliantid", TicketNumber}
    };

            //int commentCount = MaafinDbHelper.ExecuteFormQueryData<List<int>>(countCommentsQuery, countCommentsParameters)?.FirstOrDefault() ?? 0;

            Assigncounts commentCount = MaafinDbHelper.ExecuteFormQueryData<List<Assigncounts>>(countCommentsQuery, countCommentsParameters)?.FirstOrDefault();
            var countComment = commentCount?.COUNT;



            if (countComment == 0) {




                string insertHistoryQuery1 = "insert into module_complaint_history(history_id, compliant_id, created_date,  start_date, end_date,status) values(history_id.nextval,:compliantid,sysdate,:start_date,:end_date,8)";
                Dictionary<string, dynamic> historyParameters1 = new Dictionary<string, dynamic>
    {
        {"@compliantid", TicketNumber},{"@start_date",startDate},{"@end_date",endDate},
    };
                save SaveHistory1 = MaafinDbHelper.ExecuteFormQueryData<List<save>>(insertHistoryQuery1, historyParameters1)?.FirstOrDefault();


            }








            else {


                string insertHistoryQuery = "insert into module_complaint_history(history_id, compliant_id, created_date, comments, start_date, end_date,status) values(history_id.nextval,:compliantid,sysdate,:comments,:start_date,:end_date,8)";
                Dictionary<string, dynamic> historyParameters = new Dictionary<string, dynamic>
    {
        {"@compliantid", TicketNumber},{"@comments", comment},{"@start_date",startDate},{"@end_date",endDate},
    };
                save SaveHistory = MaafinDbHelper.ExecuteFormQueryData<List<save>>(insertHistoryQuery, historyParameters)?.FirstOrDefault();
            }



            // Update the status of the complaint in module_complaint_master table


            string updateMasterQuery = "update module_complaint_master j set j.status = 8, j.created_by = :userId, j.created_date = sysdate, j.comments = :comments where j.compliant_id = :compliantid";


            Dictionary<string, dynamic> updateParameters = new Dictionary<string, dynamic>
    {
        {"@compliantid", TicketNumber},  {"@userId",userId  },{"@comments", comment}
    };

            Update updateMaster = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(updateMasterQuery, updateParameters)?.FirstOrDefault();


            // set developer assign table start and end date null
            string updateAssign = "UPDATE developer_assign SET start_date = NULL, end_date = NULL WHERE compliant_id = :compliantid";



            Dictionary<string, dynamic> updateParameters1 = new Dictionary<string, dynamic>
    {
        {"@compliantid", TicketNumber}
    };




            Update updateMaster1 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(updateAssign, updateParameters)?.FirstOrDefault();












            // Update the status of the complaint in module_complaint_master table
            //        string updateMasterQuery = "update module_complaint_master set status=8 and created_date = sysdate where  compliant_id=:compliantid";
            //        Dictionary<string, dynamic> updateParameters = new Dictionary<string, dynamic>
            //{
            //    {"@compliantid", TicketNumber}
            //};
            //        Update updateMaster = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(updateMasterQuery, updateParameters)?.FirstOrDefault();

            //        // Update the developer_assign table based on the development status
            //        if (dev == "3")
            //        {
            //            string assignQuery = "update developer_assign set start_date = sysdate, status = 3 where developer_id = :userid and compliant_id =:compliantid";
            //            Dictionary<string, dynamic> assignParameters = new Dictionary<string, dynamic>
            //    {
            //        {"@userid", userId},
            //        {"@compliantid", TicketNumber}
            //    };
            //            Update updateAssign = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assignQuery, assignParameters)?.FirstOrDefault();
            //        }
            //        else if (dev == "4")
            //        {
            //            string assignQuery = "update developer_assign set end_date = sysdate, status = 4 where developer_id = :userid and compliant_id =:compliantid";
            //            Dictionary<string, dynamic> assignParameters = new Dictionary<string, dynamic>
            //    {
            //        {"@userid", userId},
            //        {"@compliantid", TicketNumber}
            //    };
            //            Update updateAssign = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assignQuery, assignParameters)?.FirstOrDefault();
            //        }





















            TempData["Message"] = "Ticket resubmitted successfully";
            TempData["Success"] = "Success";




            return RedirectToAction("complaint");
        }

        public ActionResult TicketHistory(tickethistory h)
        {


            ViewBag.Message = TempData["Message"];
            var userId = Session["userid"];
            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];
            //var TicketNumber = h.compliant_id;
            var TicketNumber = Session["ticketNumber"];


            string LoginUser = "select  m.compliant_id,m.compliantname,m.descriptions,m.created_date,c.comments,c.return_date from Module_complaint_Master m, Compliant_return c where m.compliant_id = c.compliant_id and c.compliant_id = :compliantid";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@compliantid",TicketNumber},
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();



            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<tickethistory> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<tickethistory>>(LoginUser, rptGen);

            // Initialize a new list to hold the processed ModuleSave objects
            List<tickethistory> Gview = new List<tickethistory>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                tickethistory ent = new tickethistory
                {
                    compliant_id = item.compliant_id,
                    //module_name = item.module_name,
                    compliantname = item.compliantname,
                    
                    descriptions = item.descriptions,
                    comments = item.comments,
                    created_date = item.created_date,
                    return_date = item.return_date,
                    



                };

                Gview.Add(ent);
            }

            // Return the populated list to be used in the view
            return View(Gview);


        }
        public ActionResult DeleteTicket(TicketSearch t)
        {

            ViewBag.Message = TempData["Message"];
            var Compliantid = t.compliant_id;
            Session["ticketNumber"] = Compliantid;

            string deleteQuery = "DELETE FROM MODULE_COMPLAINT_MASTER WHERE compliant_id = :compliant_id";

            // Define parameters
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>

    {
        {"@compliant_id",Compliantid}
    };
            TicketSearch ticketDetails = MaafinDbHelper.ExecuteFormQueryData<List<TicketSearch>>(deleteQuery, parameter)?.FirstOrDefault();





            TempData["Message"] = "Ticket Deleted successfully";
            TempData["Success"] = "Success";



            return RedirectToAction("complaint");
        }
        public ActionResult CloseTicket(TicketSearch t)
        {

            ViewBag.Message = TempData["Message"];
            var Compliantid = t.compliant_id;
            Session["ticketNumber"] = Compliantid;
            var userId = Session["userid"];

            string LoginUser = "update module_complaint_master l set l.status=9, l.created_by =:userId where l.compliant_id=:compliantid ";


            Dictionary<string, dynamic> parameter1 = new Dictionary<string, dynamic>
                {
                    {"@userId",userId},{"@compliantid", Compliantid}
                };
            Update update1 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(LoginUser, parameter1)?.FirstOrDefault();





            TempData["Message"] = "Ticket closed successfully";
            TempData["Success"] = "Success";



            return RedirectToAction("complaint");
        }


    }

}