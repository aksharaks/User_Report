using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_Report.App_Code;
using User_Report.Models;
using User_Report.Entities;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace User_Report.Controllers
{
    public class DeveloperController : Controller
    {
        // GET: Developer
        public ActionResult Developer_Dashboard()
        {


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






            return View();
        }


        public ActionResult User_compliant()
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




            // for fetching Ticket no. in dropdown



            string sql4 = "select l.compliant_id from module_complaint_master l,developer_assign d where l.status in (2,3,8) and l.compliant_id=d.compliant_id and d.developer_id = '" + userId + "'";


            //string sql4 = "SELECT compliant_id FROM ( SELECT t.compliant_id, t.assign_date FROM DEVELOPER_ASSIGN t WHERE t.developer_id = '" + userId + "' AND t.status != 4 UNION SELECT r.compliant_id, r.return_date AS assign_date FROM COMPLIANT_RETURN r WHERE r.status != 4 ) ORDER BY assign_date DESC";

            //string sql4 = " SELECT compliant_id FROM ( SELECT t.compliant_id, t.assign_date FROM DEVELOPER_ASSIGN t WHERE t.developer_id = '" + userId + "' AND t.status != 4 UNION SELECT h.compliant_id, h.created_date AS assign_date FROM MODULE_COMPLAINT_History h WHERE h.status = 8 ) ORDER BY assign_date DESC";



            // Execute the SQL query and retrieve data
            //select t.compliant_id from DEVELOPER_ASSIGN t

            List<assign> Assign = MaafinDbHelper.ExecuteFormQueryData<List<assign>>(sql4, new Dictionary<string, dynamic>());

            // Create a list of SelectListItem
            List<SelectListItem> Entry = new List<SelectListItem>();

            // Populate the SelectListItem list
            foreach (var assign in Assign)
            {
                Entry.Add(new SelectListItem
                {
                    Text = $" {assign.compliant_id}",
                    Value = Convert.ToString(assign.compliant_id)

                });
            }
            ViewBag.assigns = Entry;


            string sql = "SELECT d.compliant_id, c.compliantname, i.module_name, d.assign_date, d.start_date, d.end_date, CASE WHEN c.status = 1 THEN 'Requested' WHEN c.status = 2 THEN 'Assign' WHEN c.status = 3 THEN 'Development Started' WHEN c.status = 4 THEN 'Development Completed' WHEN c.status = 5 THEN 'QA Started' WHEN c.status = 6 THEN 'QA Completed' WHEN c.status = 7 THEN 'Live' ELSE 'No Action Required' END AS status, c.comments FROM developer_assign d JOIN module_complaint_master c ON d.compliant_id = c.compliant_id JOIN internal_module_master i ON c.module_id = i.module_id where d.developer_id ='" + userId + "' ORDER BY d.compliant_id";


            //string sql = "SELECT d.compliant_id, c.compliantname, i.module_name, d.assign_date, d.start_date, d.end_date, CASE WHEN c.status = 1 THEN 'Requested' WHEN c.status = 2 THEN 'Assign' WHEN c.status = 3 THEN 'Development Started' WHEN c.status = 4 THEN 'Development Completed' WHEN c.status = 5 THEN 'QA Started' WHEN c.status = 6 THEN 'QA Completed' WHEN c.status = 7 THEN 'Live' ELSE 'No Action Required' END AS status, h.comments FROM developer_assign d JOIN module_complaint_master c ON d.compliant_id = c.compliant_id JOIN internal_module_master i ON c.module_id = i.module_id LEFT JOIN module_complaint_history h ON d.compliant_id = h.compliant_id where d.developer_id ='" + userId + "'ORDER BY d.compliant_id";
            ///---------------------------------------------------------------------------------------------------------------------------------------

            //string sql = "WITH query1 AS ( SELECT m.compliant_id, m.compliantname, i.module_name,g.assign_date, g.start_date, g.end_date, CASE WHEN m.status = 1 THEN 'Requested' WHEN m.status = 2 THEN 'Assign' WHEN m.status = 3 THEN 'Development Started' WHEN m.status = 4 THEN 'Development Completed' WHEN m.status = 5 THEN 'QA Started' WHEN m.status = 6 THEN 'QA Completed' WHEN m.status = 7 THEN 'Live' ELSE 'No Action Required' END AS status, n.comments FROM module_complaint_master m LEFT JOIN COMPLIANT_RETURN n ON m.compliant_id = n.compliant_id JOIN developer_assign g ON m.compliant_id = g.compliant_id JOIN internal_module_master i ON m.module_id = i.module_id WHERE g.developer_id = '" + userId + "'), query2 AS ( SELECT v.compliant_id, v.comments FROM COMPLIANT_RETURN v ), query3 AS ( SELECT d.developer_id, d.compliant_id, d.assign_date, d.start_date, d.end_date FROM developer_assign d ) SELECT compliant_id, compliantname, module_name, assign_date, start_date, end_date, status, comments FROM query1 UNION ALL SELECT v.compliant_id, m.compliantname, i.module_name, d.assign_date, d.start_date, d.end_date, CASE WHEN m.status = 1 THEN 'Requested' WHEN m.status = 2 THEN 'Assign' WHEN m.status = 3 THEN 'Development Started' WHEN m.status = 4 THEN 'Development Completed' WHEN m.status = 5 THEN 'QA Started' WHEN m.status = 6 THEN 'QA Completed' WHEN m.status = 7 THEN 'Live' ELSE 'No Action Required' END AS status, v.comments FROM query2 v LEFT JOIN module_complaint_master m ON v.compliant_id = m.compliant_id LEFT JOIN internal_module_master i ON m.module_id = i.module_id LEFT JOIN developer_assign d ON m.compliant_id = d.compliant_id WHERE v.compliant_id NOT IN (SELECT compliant_id FROM query1)";

            //--------------------------------------------------------------------------------------------------------------------------------------


            //string sql = "WITH AssignedComplaints AS ( SELECT m.compliant_id, m.compliantname, i.module_name, g.assign_date, g.start_date, g.end_date, CASE WHEN m.status = 1 THEN 'Requested' WHEN m.status = 2 THEN 'Assign' WHEN m.status = 3 THEN 'Development Started' WHEN m.status = 4 THEN 'Development Completed' WHEN m.status = 5 THEN 'QA Started' WHEN m.status = 6 THEN 'QA Completed' WHEN m.status = 7 THEN 'Live' ELSE 'No Action Required' END AS status, n.comments FROM module_complaint_master m LEFT JOIN MODULE_COMPLAINT_History n ON m.compliant_id = n.compliant_id JOIN developer_assign g ON m.compliant_id = g.compliant_id JOIN internal_module_master i ON m.module_id = i.module_id WHERE g.developer_id = '" + userId + "' AND m.status != 4 ), ReturnedComplaints AS ( SELECT v.compliant_id, v.comments FROM MODULE_COMPLAINT_History v WHERE v.status = 8 ), HistoricalComplaints AS ( SELECT m.compliant_id, m.compliantname, i.module_name, d.assign_date, d.start_date, d.end_date, CASE WHEN m.status = 1 THEN 'Requested' WHEN m.status = 2 THEN 'Assign' WHEN m.status = 3 THEN 'Development Started' WHEN m.status = 4 THEN 'Development Completed' WHEN m.status = 5 THEN 'QA Started' WHEN m.status = 6 THEN 'QA Completed' WHEN m.status = 7 THEN 'Live' ELSE 'No Action Required' END AS status, v.comments FROM MODULE_COMPLAINT_History v LEFT JOIN module_complaint_master m ON v.compliant_id = m.compliant_id LEFT JOIN internal_module_master i ON m.module_id = i.module_id LEFT JOIN developer_assign d ON m.compliant_id = d.compliant_id WHERE v.status = 8 ) SELECT compliant_id, compliantname, module_name, assign_date, start_date, end_date, status, comments FROM AssignedComplaints UNION ALL SELECT compliant_id, compliantname, module_name, assign_date, start_date, end_date, status, comments FROM HistoricalComplaints WHERE compliant_id NOT IN (SELECT compliant_id FROM AssignedComplaints) ORDER BY assign_date DESC";












            //Dictionary<string, dynamic> parameter1 = new Dictionary<string, dynamic>
            //{
            //    {"@userId",userId  },
            //};
            //LoginUser users = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();



            // d.assign_date between :fromDate and :toDate




            // Create an empty dictionary to pass as parameters for the query
            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<TicketList> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<TicketList>>(sql, rptGen);

            // Initialize a new list to hold the processed ModuleSave objects
            List<TicketList> Gview = new List<TicketList>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                TicketList ent = new TicketList
                {
                    compliant_id = item.compliant_id,
                    module_name = item.module_name,
                    compliantname = item.compliantname,
                    assign_date = item.assign_date,
                    start_date = item.start_date,
                    end_date = item.end_date,
                    // created_date = item.created_date,
                    status = item.status,
                    comments = item.comments,



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
            Session["ticketNumber"] = Compliantid;

            string TicketSearchQuery = "select c.compliant_id,i.module_name,c.compliantname,case when c.priority = 1 then 'High' when c.priority = 2 then 'Medium' else 'Low' end as priority,c.descriptions,c.created_date,c.comments from module_complaint_master c,internal_module_master i where i.module_id = c.module_id and c.compliant_id = :compliantid";



            //string TicketSearchQuery = "SELECT c.compliant_id, i.module_name, c.compliantname, CASE WHEN c.priority = 1 THEN 'High' WHEN c.priority = 2 THEN 'Medium' ELSE 'Low' END AS priority, c.descriptions, c.created_date, cr.comments FROM module_complaint_master c JOIN internal_module_master i ON i.module_id = c.module_id LEFT JOIN COMPLIANT_RETURN cr ON c.compliant_id = cr.compliant_id WHERE c.compliant_id =:compliantid ";



            //string TicketSearchQuery = "SELECT c.compliant_id, i.module_name, c.compliantname, CASE WHEN c.priority = 1 THEN 'High' WHEN c.priority = 2 THEN 'Medium' ELSE 'Low' END AS priority, c.descriptions, c.created_date, h.comments FROM module_complaint_master c JOIN internal_module_master i ON i.module_id = c.module_id LEFT JOIN ( SELECT compliant_id, comments FROM MODULE_COMPLAINT_History WHERE (compliant_id, created_date) IN ( SELECT compliant_id, MAX(created_date) FROM MODULE_COMPLAINT_History GROUP BY compliant_id ) ) h ON c.compliant_id = h.compliant_id WHERE c.compliant_id = :compliantid";


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
            TempData["comments"] = ticketDetails?.comments;


            Session["complaint_id"] = ticketDetails?.compliant_id;


            return RedirectToAction("User_compliant");
        }


        public ActionResult Ticket_submit(ticket_submit s)
        {

            ViewBag.Message = TempData["Message"];
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            var departmentID = Convert.ToInt32(Session["departmentID"]);
            var postID = Session["postID"];
            // var compliantid = Session["compliant_id"];
            var TicketNumber = Session["ticketNumber"];
            var development = s.development;
            /// var comment = s.comments;
            var comment = string.IsNullOrWhiteSpace(s.comments) ? "No comment" : s.comments;

            var dev = Convert.ToString(development);






            //-----------------------------------------------------------------------------------------------------------
            string checkExistenceQuery = @"select COUNT(*) from DEVELOPER_ASSIGN t  where t.developer_id=:userId and t.compliant_id=:compliantid and t.status=3";
            Dictionary<string, dynamic> existenceParams = new Dictionary<string, dynamic>
        {
             {"@userId",userId},{"@compliantid",TicketNumber},{"@development",development}
        };
            Assigncounts asscount = MaafinDbHelper.ExecuteFormQueryData<List<Assigncounts>>(checkExistenceQuery, existenceParams)?.FirstOrDefault();
            var DevCount = asscount?.COUNT;



            // if the development started update the status start to the compliant table

            if (dev == "3")
            {

                //check development has already started

                if (DevCount == 0)
                {

                    string LoginUser = "update module_complaint_master l set l.status=3 where l.compliant_id=:compliantid";


                    Dictionary<string, dynamic> parameter1 = new Dictionary<string, dynamic>
                {
                    {"@userId",userId},{"@compliantid",TicketNumber}
                };
                    Update update1 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(LoginUser, parameter1)?.FirstOrDefault();



                    string assign = "update developer_assign d set d.start_date = sysdate, d.status = 3 where developer_id = :userid and compliant_id =:compliantid";


                    Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
                {
                    {"@userId",userId},{"@compliantid",TicketNumber}
                };
                    Update update = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assign, parameter)?.FirstOrDefault();




    //                string addHistoryDevelopmentStarted = @"
    //INSERT INTO MODULE_COMPLAINT_HISTORY (
    //    history_id,
    //    compliant_id,
    //    module_id,
    //    compliantname,
    //    descriptions,
    //    priority,
    //    status,
    //    created_by,
    //    created_date,
    //    department_id,
    //    comments,
    //    start_date
    //)
    //SELECT 
    //    History_Id.NEXTVAL,  -- Sequence to generate history_id
    //    compliant_id,
    //    module_id,
    //    compliantname,
    //    descriptions,
    //    priority,
    //    3 AS status,  -- Status 3 indicates Development Started
    //    :userId AS created_by,
    //    SYSDATE AS created_date,
    //    department_id,
    //    NVL(:comments, 'No comment') AS comments,  -- Use 'No comment' if :comments is null
    //    SYSDATE AS start_date
    //FROM module_complaint_master
    //WHERE compliant_id = :compliantid";

    //                Dictionary<string, dynamic> historyParamsDevelopmentStarted = new Dictionary<string, dynamic>
    //        {
    //            {"@userId", userId},
    //            {"@compliantid", TicketNumber},
    //            {"@comments", comment}
    //        };
    //                MaafinDbHelper.ExecuteFormQueryData<List<save>>(addHistoryDevelopmentStarted, historyParamsDevelopmentStarted);


                    //        string assign1 = "update developer_assign d set d.start_date = sysdate, d.status = 3 where developer_id = :userid and compliant_id =:compliantid";


                    //        Dictionary<string, dynamic> parameter2 = new Dictionary<string, dynamic>
                    //{
                    //    {"@userId",userId},{"@compliantid",TicketNumber}
                    //};
                    //        Update update2 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assign1, parameter2)?.FirstOrDefault();

                    //-------------------------




                    //string assign1 = "update compliant_return r set r.status = 3 where compliant_id =:compliantid";


                    //Dictionary<string, dynamic> parameter2 = new Dictionary<string, dynamic>
                    //    {
                    //       /* {"@userId",userId},*/{"@compliantid",TicketNumber}
                    //    };
                    //Update update2 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assign1, parameter2)?.FirstOrDefault();






                    TempData["Message"] = "Development started successfully";
                    TempData["Success"] = "Success";
                    //return RedirectToAction("User_compliant");


                }
                else
                {


                    TempData["Message"] = "This ticket development is already started.";
                    TempData["Success"] = "Warning";
                    ////return RedirectToAction("User_compliant");


                }
                return RedirectToAction("User_compliant");
            }

            // case where development completed
            else if (dev == "4")
            {
                //check development has already ended

                if (DevCount == 0)
                {
                    TempData["Message"] = "Development cannot be completed because it not yet started.";
                    TempData["Success"] = "Warning";
                    //  return RedirectToAction("User_compliant");
                }

                string LoginUser = "update module_complaint_master l set l.status=4 where l.compliant_id=:compliantid";


                Dictionary<string, dynamic> parameter1 = new Dictionary<string, dynamic>
                {
                    {"@userId",userId},{"@compliantid",TicketNumber}
                };
                Update update1 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(LoginUser, parameter1)?.FirstOrDefault();



                string assign = "update developer_assign d set d.end_date = sysdate, d.status = 4 where developer_id = :userid and compliant_id =:compliantid";

                //string assign = "INSERT INTO developer_assign (developer_id, compliant_id, assign_date, status) VALUES (@userId, @compliantid, sysdate, 3)";


                Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
                {
                    {"@userId",userId},{"@compliantid",TicketNumber}
                };
                Update update = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assign, parameter)?.FirstOrDefault();


        //        string updateHistoryCompletion = @"
        //    UPDATE MODULE_COMPLAINT_HISTORY
        //    SET 
        //        status = 4,
        //        comments = :comments,
        //        end_date = SYSDATE
        //    WHERE 
        //        compliant_id = :compliantid 
        //        AND status = 3
        //        AND end_date IS NULL";


        //        Dictionary<string, dynamic> historyParamsCompletion = new Dictionary<string, dynamic>
        //{
        //    {"@compliantid", TicketNumber},
        //    {"@comments", comment}
        //};
        //        MaafinDbHelper.ExecuteFormQueryData<List<save>>(updateHistoryCompletion, historyParamsCompletion);

                ////    string assign1 = "update compliant_return r set r.status = 4 where compliant_id =:compliantid";


                //    Dictionary<string, dynamic> parameter2 = new Dictionary<string, dynamic>
                //            {
                //               /* {"@userId",userId},*/{"@compliantid",TicketNumber}
                //            };
                //    Update update2 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assign1, parameter2)?.FirstOrDefault();



               




                TempData["Message"] = "Development completed  successfully";
                TempData["Success"] = "Success";

                return RedirectToAction("User_compliant");

            }









            return RedirectToAction("User_compliant");

        }



        //---------------------------------------------------------------------------------



        //public ActionResult Ticket_submit(ticket_submit s)
        //{
        //    ViewBag.Message = TempData["Message"];
        //    var userId = Session["userid"];
        //    var departmentID = Convert.ToInt32(Session["departmentID"]);
        //    var postID = Session["postID"];
        //    var TicketNumber = Session["ticketNumber"];
        //    var development = s.development;
        //    var dev = Convert.ToString(development);

        //    // Check if the compliant ID is returned with status 8
        //    string checkReturnedQuery = @"select COUNT(*) from COMPLIANT_RETURN r where r.compliant_id=:compliantid and r.status=8";
        //    Dictionary<string, dynamic> returnedParams = new Dictionary<string, dynamic>
        //{
        //    {"@compliantid", TicketNumber}
        //};
        //    var returnedCount = MaafinDbHelper.ExecuteFormQueryData<List<Assigncounts>>(checkReturnedQuery, returnedParams)?.FirstOrDefault()?.COUNT;

        //    if (returnedCount > 0) // Compliant ID is returned
        //    {
        //        if (dev == "3")
        //        {
        //            // Insert new entry into DEVELOPER_ASSIGN for the returned ticket (development started)
        //            string insertNewEntry = "insert into DEVELOPER_ASSIGN (compliant_id, developer_id, start_date, status) values (:compliantid, :userid, sysdate, :status)";
        //            Dictionary<string, dynamic> insertParams = new Dictionary<string, dynamic>
        //        {
        //            {"@userId", userId},
        //            {"@compliantid", TicketNumber},
        //            {"@status", dev}
        //        };
        //            MaafinDbHelper.ExecuteFormQueryData<Update>(insertNewEntry, insertParams);

        //            // Update the status in COMPLIANT_RETURN (development started)
        //            string updateReturnStatus = "update compliant_return r set r.status = 3 where compliant_id =:compliantid";
        //            MaafinDbHelper.ExecuteFormQueryData<Update>(updateReturnStatus, new Dictionary<string, dynamic> { { "@compliantid", TicketNumber } });

        //            TempData["Message"] = "Development started successfully for the returned ticket.";
        //            TempData["Success"] = "Success";
        //        }
        //        else if (dev == "4")
        //        {
        //            // Insert new entry into DEVELOPER_ASSIGN for the returned ticket (development completed)
        //            string insertNewEntry = "insert into DEVELOPER_ASSIGN (compliant_id, developer_id, end_date, status) values (:compliantid, :userid, sysdate, :status)";
        //            Dictionary<string, dynamic> insertParams = new Dictionary<string, dynamic>
        //        {
        //            {"@userId", userId},
        //            {"@compliantid", TicketNumber},
        //            {"@status", dev}
        //        };
        //            MaafinDbHelper.ExecuteFormQueryData<Update>(insertNewEntry, insertParams);

        //            // Update the status in COMPLIANT_RETURN (development completed)
        //            string updateReturnStatus = "update compliant_return r set r.status = 4 where r.compliant_id =:compliantid";
        //            MaafinDbHelper.ExecuteFormQueryData<Update>(updateReturnStatus, new Dictionary<string, dynamic> { { "@compliantid", TicketNumber } });

        //            TempData["Message"] = "Development completed successfully for the returned ticket.";
        //            TempData["Success"] = "Success";
        //        }
        //        return RedirectToAction("User_compliant");
        //    }


        //    ///---------------------------------------------------------------------------------------------------
        //    // Check if development has already started// Existing logic for non-returned compliant IDs
        //    string checkExistenceQuery = @"select COUNT(*) from DEVELOPER_ASSIGN t where t.developer_id=:userId and t.compliant_id=:compliantid and t.status=3";
        //    Dictionary<string, dynamic> existenceParams = new Dictionary<string, dynamic>
        //{
        //    {"@userId", userId},
        //    {"@compliantid", TicketNumber},
        //    {"@development", development}
        //};
        //    Assigncounts asscount = MaafinDbHelper.ExecuteFormQueryData<List<Assigncounts>>(checkExistenceQuery, existenceParams)?.FirstOrDefault();
        //    var DevCount = asscount?.COUNT;

        //    if (dev == "3") // Development started
        //    {
        //        if (DevCount == 0)
        //        {
        //            string LoginUser = "update module_complaint_master l set l.status=3 where l.compliant_id=:compliantid";
        //            Dictionary<string, dynamic> parameter1 = new Dictionary<string, dynamic>
        //        {
        //            {"@userId", userId},
        //            {"@compliantid", TicketNumber}
        //        };
        //            Update update1 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(LoginUser, parameter1)?.FirstOrDefault();

        //            string assign = "update developer_assign d set d.start_date = sysdate, d.status = 3 where developer_id = :userid and compliant_id =:compliantid";
        //            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
        //        {
        //            {"@userId", userId},
        //            {"@compliantid", TicketNumber}
        //        };
        //            Update update = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assign, parameter)?.FirstOrDefault();

        //            string assign1 = "update compliant_return r set r.status = 3 where compliant_id =:compliantid";
        //            Dictionary<string, dynamic> parameter2 = new Dictionary<string, dynamic>
        //        {
        //            {"@compliantid", TicketNumber}
        //        };
        //            Update update2 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assign1, parameter2)?.FirstOrDefault();

        //            TempData["Message"] = "Development started successfully";
        //            TempData["Success"] = "Success";
        //        }
        //        else
        //        {
        //            TempData["Message"] = "This ticket development is already started.";
        //            TempData["Success"] = "Warning";
        //        }
        //        return RedirectToAction("User_compliant");
        //    }
        //    else if (dev == "4") // Development completed
        //    {
        //        if (DevCount == 0)
        //        {
        //            TempData["Message"] = "Development cannot be completed because it has not yet started.";
        //            TempData["Success"] = "Warning";
        //        }
        //        else
        //        {
        //            string LoginUser = "update module_complaint_master l set l.status=4 where l.compliant_id=:compliantid";
        //            Dictionary<string, dynamic> parameter1 = new Dictionary<string, dynamic>
        //        {
        //            {"@userId", userId},
        //            {"@compliantid", TicketNumber}
        //        };
        //            Update update1 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(LoginUser, parameter1)?.FirstOrDefault();

        //            string assign = "update developer_assign d set d.end_date = sysdate, d.status = 4 where developer_id = :userid and compliant_id =:compliantid";
        //            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
        //        {
        //            {"@userId", userId},
        //            {"@compliantid", TicketNumber}
        //        };
        //            Update update = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assign, parameter)?.FirstOrDefault();

        //            string assign1 = "update compliant_return r set r.status = 4 where compliant_id =:compliantid";
        //            Dictionary<string, dynamic> parameter2 = new Dictionary<string, dynamic>
        //        {
        //            {"@compliantid", TicketNumber}
        //        };
        //            Update update2 = MaafinDbHelper.ExecuteFormQueryData<List<Update>>(assign1, parameter2)?.FirstOrDefault();

        //            TempData["Message"] = "Development completed successfully";
        //            TempData["Success"] = "Success";
        //        }
        //        return RedirectToAction("User_compliant");
        //    }

        //    return RedirectToAction("User_compliant");
        //}















        public ActionResult clear()
        {
            return RedirectToAction("User_compliant");
        }
    

        public ActionResult Report()
        {

            return View();
        }



        public ActionResult SubmitForm(DateTime fromDate, DateTime toDate)


        {
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


            //string sql = "select d.compliant_id, c.compliantname, i.module_name, d.assign_date,d.start_date,d.end_date, case when c.status = 1 then 'Requested' when c.status = 2 then 'Assign' when c.status = 3 then 'development started' when c.status = 4 then 'development completed' when c.status = 5 then 'QA started' when c.status = 6 then 'QA completed' when c.status = 7 then 'Live' else 'No_Action_required' end as status from developer_assign d, module_complaint_master c, internal_module_master i where i.module_id = c.module_id and d.compliant_id = c.compliant_id and d.developer_id = '" + userId + "' and c.status = d.status and  Trunc(d.assign_date) between :fromDate and :toDate order by d.assign_date desc";

            string sql = @"select d.compliant_id,
       c.compliantname,
       i.module_name,
       d.assign_date,
       d.start_date,
       d.end_date,
       case
         when c.status = 1 then
          'Requested'
         when c.status = 2 then
          'Assign'
         when c.status = 3 then
          'development started'
         when c.status = 4 then
          'development completed'
         when c.status = 5 then
          'QA started'
         when c.status = 6 then
          'QA completed'
         when c.status = 7 then
          'Live'
         else
          'No_Action_required'
       end as status
  from developer_assign        d,
       module_complaint_master c,
       internal_module_master  i
 where i.module_id = c.module_id
   and d.compliant_id = c.compliant_id
   and d.developer_id = :userId
   and c.status = d.status
   and Trunc(d.assign_date) between :fromDate and :toDate
 order by d.assign_date desc";


            // d.assign_date between :fromDate and :toDate,,TRUNC(d.assign_date) BETWEEN TO_DATE(:fromDate, 'DD-MON-YYYY') AND TO_DATE(:toDate, 'DD-MON-YYYY')
            Dictionary<string, dynamic> sqlParams = new Dictionary<string, dynamic>
            {
                { "@userId", userId },
                { "@fromDate",fromDate},
                { "@toDate",toDate}
            };



            // Create an empty dictionary to pass as parameters for the query
            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<TicketList> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<TicketList>>(sql, sqlParams);

            // Initialize a new list to hold the processed ModuleSave objects
            List<TicketList> Gview = new List<TicketList>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                TicketList ent = new TicketList
                {
                    compliant_id = item.compliant_id,
                    module_name = item.module_name,
                    compliantname = item.compliantname,
                    assign_date = item.assign_date,
                    start_date = item.start_date,
                    end_date = item.end_date,
                    // created_date = item.created_date,
                    status = item.status,



                };

                Gview.Add(ent);
            }

            // Return the populated list to be used in the view
            return PartialView("Report_PartialPage",Gview);
        }

    }

}



//        if (!fromDate.HasValue || !toDate.HasValue)
//        {
//            // Handle the case where one or both dates are not provided
//            ModelState.AddModelError("", "Please provide both From Date and To Date.");
//            return View( new List<TicketList>());
//        }
//        var userId = Session["userid"];
//        var departmentID = Convert.ToInt32(Session["departmentID"]);
//        var postID = Session["postID"];



//        string LoginUser = "select em.emp_name,l.dep_name from employee_master em, department_mst l where em.emp_code =:userId and em.department_id=l.dep_id";
//        Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
//        {
//            {"@userId",userId  },
//        };
//        LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
//        Session["user_name"] = user?.emp_name;
//        Session["departmentName"] = user?.dep_name;


//        string sql = "select d.compliant_id, c.compliantname, i.module_name, d.assign_date,d.start_date,d.end_date, case when c.status = 1 then 'Requested' when c.status = 2 then 'Assign' when c.status = 3 then 'development started' when c.status = 4 then 'development completed' when c.status = 5 then 'QA started' when c.status = 6 then 'QA completed' when c.status = 7 then 'Live' else 'No_Action_required' end as status from developer_assign d, module_complaint_master c, internal_module_master i where i.module_id = c.module_id and d.compliant_id = c.compliant_id and d.developer_id = '" + userId + "' and c.status = d.status and d.assign_date between @fromDate and @toDate order by d.assign_date desc";

//        // d.assign_date between :fromDate and :toDate
//        Dictionary<string, dynamic> sqlParams = new Dictionary<string, dynamic>
//{
//    { "@userId", userId },
//    { "@fromDate", fromDate.Value },
//    { "@toDate", toDate.Value }
//};



//        // Create an empty dictionary to pass as parameters for the query
//        Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

//        // Execute the query and get the result as a list of ModuleSave objects
//        List<TicketList> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<TicketList>>(sql, rptGen);

//        // Initialize a new list to hold the processed ModuleSave objects
//        List<TicketList> Gview = new List<TicketList>();

//        // Iterate through the fetched list and populate the new list
//        foreach (var item in rpt_gridView)
//        {
//            TicketList ent = new TicketList
//            {
//                compliant_id = item.compliant_id,
//                module_name = item.module_name,
//                compliantname = item.compliantname,
//                assign_date = item.assign_date,
//                start_date = item.start_date,
//                end_date = item.end_date,
//                // created_date = item.created_date,
//                status = item.status,



//            };

//            Gview.Add(ent);
//        }

//        // Return the populated list to be used in the view
//        return View(Gview);