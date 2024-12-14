using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_Report.Models;
using User_Report.Entities;
using User_Report.App_Code;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Web.UI.WebControls;


namespace User_Report.Controllers
{
    public class TechController : Controller
    {
        // GET: Tech
        public ActionResult TechDashboard()
        {

            var userId = Session["userid"];
            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];

            string LoginUser = "select em.emp_name,p.post_name as dep_name from employee_master em, department_mst l,post_mst p where em.emp_code = :userId and em.department_id = l.dep_id and em.post_id = p.post_id";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  },
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
            Session["user_name"] = user?.emp_name;
            Session["departmentName"] = user?.dep_name;




            return View();
        }




        public ActionResult Developerassign()
        {
            ViewBag.Message = TempData["Message"];

            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];


            string LoginUser = "select em.emp_name,p.post_name as dep_name from employee_master em, department_mst l,post_mst p where em.emp_code = :userId and em.department_id = l.dep_id and em.post_id = p.post_id";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  },
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
            Session["user_name"] = user?.emp_name;
            Session["departmentName"] = user?.dep_name;



            // for fetching compliant id in dropdown

            string sql4 = "SELECT c.compliant_id, c.compliantname FROM Module_Complaint_Master c LEFT JOIN developer_assign m ON c.compliant_id = m.compliant_id WHERE m.compliant_id IS NULL";

            // Execute the SQL query and retrieve data
            List<ticket> Ticket = MaafinDbHelper.ExecuteFormQueryData<List<ticket>>(sql4, new Dictionary<string, dynamic>());

            // Create a list of SelectListItem
            List<SelectListItem> Entry = new List<SelectListItem>();

            // Populate the SelectListItem list
            foreach (var ticket in Ticket)
            {
                Entry.Add(new SelectListItem
                {
                    Text = $" {ticket.compliant_id} - {ticket.compliantname}",
                    Value = Convert.ToString(ticket.compliant_id)

                });
            }
            ViewBag.tickets = Entry;



            //developer dropdown

            string sql5 = "select l.emp_code, l.emp_name, l.password_int, em.branch_id, em.department_id, em.post_id from employee_master em, logininternal l where em.emp_code = l.emp_code and l.branch_id = 0 and em.department_id = 738 and em.post_id in (920,883) and em.status_id = 1";

            // Execute the SQL query and retrieve data
            List<developer> Developer = MaafinDbHelper.ExecuteFormQueryData<List<developer>>(sql5, new Dictionary<string, dynamic>());

            // Create a list of SelectListItem
            List<SelectListItem> Entry1 = new List<SelectListItem>();

            // Populate the SelectListItem list
            foreach (var developer in Developer)
            {
                Entry1.Add(new SelectListItem
                {
                    Text = $" {developer.emp_code} - {developer.emp_name}",
                    Value = Convert.ToString(developer.emp_code)

                });
            }
            ViewBag.developers = Entry1;



            string sql = "select d.compliant_id, c.compliantname, i.module_name, c.descriptions, c.created_date, case when c.priority = 1 then 'High' when c.priority = 2 then 'Medium' else 'Low' end as priority, d.developer_id, e.emp_name, case when c.status = 1 then 'Requested' when c.status = 2 then 'Assign' when c.status = 3 then 'development started' when c.status = 4 then 'development completed' when c.status = 5 then 'QA started' when c.status = 6 then 'QA completed' when c.status = 7 then 'Live' when c.status = 9 then 'compliant closed' else 'No_Action_required' end as status from developer_assign d, module_complaint_master c, internal_module_master i, employee_master e where i.module_id = c.module_id and d.compliant_id = c.compliant_id and e.emp_code = d.developer_id and c.status = d.status order by d.assign_date desc";



            // Create an empty dictionary to pass as parameters for the query
            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<AssignReport> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<AssignReport>>(sql, rptGen);

            // Initialize a new list to hold the processed ModuleSave objects
            List<AssignReport> Gview = new List<AssignReport>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                AssignReport ent = new AssignReport
                {
                    compliant_id = item.compliant_id,

                    compliantname = item.compliantname,
                    descriptions = item.descriptions,
                    module_name = item.module_name,
                    priority = item.priority,
                    created_date = item.created_date,
                    emp_name = item.emp_name,
                    status = item.status,



                };

                Gview.Add(ent);
            }

            // Return the populated list to be used in the view
            return View(Gview);


        }
        public ActionResult Ticket_search(TicketSearch t)
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


            string TicketSearchQuery = "select c.compliant_id,i.module_name,c.compliantname,case when c.priority = 1 then 'High' when c.priority = 2 then 'Medium' else 'Low' end as priority,c.descriptions,c.created_date from module_complaint_master c,internal_module_master i where c.status =1 and i.module_id = c.module_id and c.compliant_id = :compliantid";


            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@compliantid",Compliantid}

            };
            TicketSearch ticketDetails = MaafinDbHelper.ExecuteFormQueryData<List<TicketSearch>>(TicketSearchQuery, parameter)?.FirstOrDefault();


            TempData["complaint_id"] = ticketDetails?.compliant_id;
            TempData["module_name"] = ticketDetails?.module_name;
            TempData["compliant_name"] = ticketDetails?.compliantname;
            TempData["priority"] = ticketDetails?.priority;
            TempData["description"] = ticketDetails?.descriptions;
            TempData["date"] = ticketDetails?.created_date;


          Session["complaint_id"] = ticketDetails?.compliant_id;






            return RedirectToAction("Developerassign");


        }





        public ActionResult Ticket_Assign(ticket_Assign a)
        {
            ViewBag.Message = TempData["Message"];
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];


            var developerid = a.developer_id;
            var assign_date = a.assign_date;
            var compliantid = Session["complaint_id"];

   


                string LoginUser = "insert into developer_assign(assign_no, developer_id, status,compliant_id, assign_date) values(assign_no.nextval,:developerid,2,:complaintid,sysdate)";

                Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
                {
                    {"@developerid",developerid}, {"@complaintid",compliantid}

                };
                save Save = MaafinDbHelper.ExecuteFormQueryData<List<save>>(LoginUser, parameter)?.FirstOrDefault();



                string LoginUser1 = "update Module_Complaint_Master m set m.status =2 where m.compliant_id=:complaintid";

                Dictionary<string, dynamic> parameter2 = new Dictionary<string, dynamic>
                {
                     {"@complaintid",compliantid}

                };
                Update update = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(LoginUser1, parameter2)?.FirstOrDefault();






                TempData["Message"] = "Ticket assigned successfully";
                TempData["Success"] = "Success";





                return RedirectToAction("Developerassign");


            }
        public ActionResult TechReport()
        {



            return View();
        }

        //compliant report
        public ActionResult submitForm(DateTime fromDate, DateTime toDate)
        {
            var userId = Session["userid"];
            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];


            string LoginUser = "select em.emp_name,p.post_name as dep_name from employee_master em, department_mst l,post_mst p where em.emp_code = :userId and em.department_id = l.dep_id and em.post_id = p.post_id";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  },
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
            Session["user_name"] = user?.emp_name;
            Session["departmentName"] = user?.dep_name;


            //string sql = "select d.compliant_id, c.compliantname, i.module_name, c.descriptions, c.created_date, case when c.priority = 1 then 'High' when c.priority = 2 then 'Medium' else 'Low' end as priority, d.developer_id, e.emp_name, case when c.status = 1 then 'Requested' when c.status = 2 then 'Assign' when c.status = 3 then 'development started' when c.status = 4 then 'development completed' when c.status = 5 then 'QA started' when c.status = 6 then 'QA completed' when c.status = 7 then 'Live' else 'No_Action_required' end as status from developer_assign d, module_complaint_master c, internal_module_master i, employee_master e where i.module_id = c.module_id and d.compliant_id = c.compliant_id and e.emp_code = d.developer_id and c.status = d.status  and Trunc(d.assign_date) between :fromDate and :toDate order by d.assign_date desc";



            //            string sql = @"select d.compliant_id,
            //       c.compliantname,
            //       i.module_name,
            //       c.descriptions,
            //       c.created_date,
            //       case
            //         when c.priority = 1 then
            //          'High'
            //         when c.priority = 2 then
            //          'Medium'
            //         else
            //          'Low'
            //       end as priority,
            //       d.developer_id,
            //       e.emp_name,
            //       case
            //         when c.status = 1 then
            //          'Requested'
            //         when c.status = 2 then
            //          'Assign'
            //         when c.status = 3 then
            //          'development started'
            //         when c.status = 4 then
            //          'development completed'
            //         when c.status = 5 then
            //          'QA started'
            //         when c.status = 6 then
            //          'QA completed'
            //         when c.status = 7 then
            //          'Live'
            //         else
            //          'No_Action_required'
            //       end as status
            //  from developer_assign        d,
            //       module_complaint_master c,
            //       internal_module_master  i,
            //       employee_master         e
            // where i.module_id = c.module_id
            //   and d.compliant_id = c.compliant_id
            //   and e.emp_code = d.developer_id
            //   and c.status = d.status
            //   and Trunc(d.assign_date) between :fromDate and :toDate
            // order by d.assign_date desc
            //";


            string sql = @"SELECT c.compliant_id,
       i.module_name,
       c.compliantname,
       c.descriptions,
       
       CASE
         WHEN c.priority = 1 THEN
          'High'
         WHEN c.priority = 2 THEN
          'Medium'
         ELSE
          'Low'
       END AS priority,
       c.created_date,
       e1.emp_name AS created_by,
       e2.emp_name AS developer_name,
       CASE
         WHEN c.status = 1 THEN
          'Requested'
         WHEN c.status = 2 THEN
          'Assign'
         WHEN c.status = 3 THEN
          'Development Started'
         WHEN c.status = 4 THEN
          'Development Completed'
         WHEN c.status = 5 THEN
          'QA Started'
         WHEN c.status = 6 THEN
          'QA Completed'
         WHEN c.status = 7 THEN
          'Live'
         WHEN c.status = 8 THEN
          'Compliant Resubmitted'
         WHEN c.status = 9 THEN
          'Compliant Closed'
         ELSE
          'No_Action_Required'
       END AS status,
       d.start_date,
       d.end_date,
       c.comments
  FROM internal_module_master i
  JOIN Module_complaint_Master c
    ON i.module_id = c.module_id
  JOIN employee_master e1
    ON c.created_by = e1.emp_code
  LEFT JOIN DEVELOPER_ASSIGN d
    ON c.compliant_id = d.compliant_id
  LEFT JOIN employee_master e2
    ON d.developer_id = e2.emp_code
 and Trunc(d.assign_date) between :fromDate and :toDate
 order by d.assign_date desc
";




            Dictionary<string, dynamic> sqlParams = new Dictionary<string, dynamic>
            {
               /// { "@userId", userId },
                { "@fromDate",fromDate},
                { "@toDate",toDate}
            };




            // Create an empty dictionary to pass as parameters for the query
            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<AssignReport> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<AssignReport>>(sql, sqlParams);

            // Initialize a new list to hold the processed ModuleSave objects
            List<AssignReport> Gview = new List<AssignReport>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                AssignReport ent = new AssignReport
                {
                    compliant_id = item.compliant_id,
                    
                    compliantname = item.compliantname,
                    descriptions = item.descriptions,
                    module_name = item.module_name,
                    priority = item.priority,
                    created_date = item.created_date,
                    //emp_name = item.emp_name,
                    status = item.status,
                    start_date = item.start_date,
                    end_date = item.end_date,
                    developer_name = item.developer_name,
                    created_by = item.created_by,
                    comments = item.comments,



                };

                Gview.Add(ent);
            }

            // Return the populated list to be used in the view
            return PartialView("Tech_report_partialpage", Gview);
        }


        public ActionResult Feedback_Report()
        {



            return View();
        }
        // feeedback report
        public ActionResult submitReport(DateTime fromDate, DateTime toDate)
        {
            var userId = Session["userid"];
            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];


            string LoginUser = "select em.emp_name,p.post_name as dep_name from employee_master em, department_mst l,post_mst p where em.emp_code = :userId and em.department_id = l.dep_id and em.post_id = p.post_id";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  },
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
            Session["user_name"] = user?.emp_name;
            Session["departmentName"] = user?.dep_name;


            //string sql = "select d.compliant_id, c.compliantname, i.module_name, c.descriptions, c.created_date, case when c.priority = 1 then 'High' when c.priority = 2 then 'Medium' else 'Low' end as priority, d.developer_id, e.emp_name, case when c.status = 1 then 'Requested' when c.status = 2 then 'Assign' when c.status = 3 then 'development started' when c.status = 4 then 'development completed' when c.status = 5 then 'QA started' when c.status = 6 then 'QA completed' when c.status = 7 then 'Live' else 'No_Action_required' end as status from developer_assign d, module_complaint_master c, internal_module_master i, employee_master e where i.module_id = c.module_id and d.compliant_id = c.compliant_id and e.emp_code = d.developer_id and c.status = d.status  and Trunc(d.assign_date) between :fromDate and :toDate order by d.assign_date desc";



            string sql = @"SELECT 
    i.module_name, 
    m.feedbackname, 
    m.descriptions, 
    m.enhancement, 
    m.rating, 
    m.created_by, 
    m.created_date, 
    e.emp_name 
FROM 
    Module_feedback_Master m, 
    internal_module_master i, 
    employee_master e 
WHERE 
    i.module_id = m.module_id 
    AND m.created_by = e.emp_code 
    and Trunc(m.created_date) between :fromDate and :toDate
ORDER BY  m.created_date DESC
";




            Dictionary<string, dynamic> sqlParams = new Dictionary<string, dynamic>
            {
               /// { "@userId", userId },
                { "@fromDate",fromDate},
                { "@toDate",toDate}
            };




            // Create an empty dictionary to pass as parameters for the query
            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<Feedbacksave> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<Feedbacksave>>(sql, sqlParams);

            // Initialize a new list to hold the processed ModuleSave objects
            List<Feedbacksave> Gview = new List<Feedbacksave>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                Feedbacksave ent = new Feedbacksave
                {


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



                };

                Gview.Add(ent);
            }

            // Return the populated list to be used in the view
            return PartialView("feedback_report_partialpage", Gview);
        }



    }




}


    















