using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_Report.App_Code;
using User_Report.Entities;
using User_Report.Models;

namespace User_Report.Controllers
{
    public class AdminController : Controller
    {

        // GET: Admin
        public ActionResult DashBoard()
        {
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

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





        public ActionResult Add_Module()
        {
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }


            string LoginUser = "select em.emp_name from employee_master em where em.emp_code = :userId";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  },
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
            Session["user_name"] = user?.emp_name;

            // Define the SQL query to fetch data from the database for display report
            string sql = "SELECT t.module_name,  t.module_description, t.created_date FROM INTERNAL_MODULE_MASTER t order by t.created_date desc";

            // Create an empty dictionary to pass as parameters for the query
            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<ListModule> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<ListModule>>(sql, rptGen);

            // Initialize a new list to hold the processed ModuleSave objects
            List<ListModule> Gview = new List<ListModule>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                ListModule ent = new ListModule
                {
                    module_name = item.module_name,
                    module_description = item.module_description,
                    //created_by =item.created_by,
                    created_date = item.created_date
                };

                Gview.Add(ent);
            }

            // Return the populated list to be used in the view
            return View(Gview);





        }



        public ActionResult moduleSave(ModuleSave m)
        {
            var userId = Session["userid"];
            var moduleName = m.module_name;
            var description = m.module_description;


            string LoginUser = "insert into Internal_module_Master (Module_Id, Module_Name, Module_Status, Module_Description, Created_By, Created_Date) values(moduleid.nextval,:moduleName,1,:description,:userId,sysdate)";



            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  }, {"@description",description  }, {"moduleName",moduleName  },


            };
            save Save = MaafinDbHelper.ExecuteFormQueryData<List<save>>(LoginUser, parameter)?.FirstOrDefault();


            return RedirectToAction("Add_Module");
        }

        public ActionResult clear()
        {
            return RedirectToAction("Add_Module");
        }
        




        public ActionResult AssignModule()
        {
            var userId = Session["userid"];
            if (userId == null)
            {

                return RedirectToAction("Login", "Login");
            }

            ViewBag.Message = TempData["Message"];
          
            string LoginUser = "select em.emp_name from employee_master em where em.emp_code = :userId";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  },
            };
            LoginUser user = MaafinDbHelper.ExecuteFormQueryData<List<LoginUser>>(LoginUser, parameter)?.FirstOrDefault();
            Session["user_name"] = user?.emp_name;

            //-------------------------------for fetching module name in dropdown

            string sql4 = "select i.module_id,i.module_name from Internal_module_Master i where i.module_status = 1";

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
            //-----------------------------------------------------------------------------------------------------------
            //for fetching department name in dropdown


            string sql5 = "select d.dep_id,d.dep_name from department_mst d where d.firm_id = 4 and d.status =1";

            // Execute the SQL query and retrieve data
            List<departmentName> Department = MaafinDbHelper.ExecuteFormQueryData<List<departmentName>>(sql5, new Dictionary<string, dynamic>());

            // Create a list of SelectListItem
            List<SelectListItem> Entry1 = new List<SelectListItem>();

            // Populate the SelectListItem list
            foreach (var department in Department)
            {
                Entry1.Add(new SelectListItem
                {
                    Text = $" {department.dep_name}",
                    Value = Convert.ToString(department.dep_name)
                });
            }

            // Assign the SelectListItem list to ViewBag.entryid
            ViewBag.depnames = Entry1;





            // Define the SQL query to fetch data from the database for display report  of assign module
            string sql = "select i.module_name,d.dep_name from MODULE_ASSIGN_MASTER t, internal_module_master i, department_mst d where d.firm_id = 4 and d.status =1 and i.module_id = t.module_id and d.dep_id = t.department_id order by t.created_date desc ";

            // Create an empty dictionary to pass as parameters for the query
            Dictionary<string, dynamic> rptGen = new Dictionary<string, dynamic>();

            // Execute the query and get the result as a list of ModuleSave objects
            List<ListAssignmodule> rpt_gridView = MaafinDbHelper.ExecuteFormQueryData<List<ListAssignmodule>>(sql, rptGen);

            // Initialize a new list to hold the processed ModuleSave objects
            List<ListAssignmodule> Gview = new List<ListAssignmodule>();

            // Iterate through the fetched list and populate the new list
            foreach (var item in rpt_gridView)
            {
                ListAssignmodule ent = new ListAssignmodule
                {
                    module_name = item.module_name,
                    dep_name = item.dep_name
                    //created_by =item.created_by,

                };

                Gview.Add(ent);
            }

            // Return the populated list to be used in the view
            return View(Gview);


        }
        public ActionResult assignSave(assignSave a)
        {
            var userId = Session["userid"];
            var moduleName = a.modulename;
            var departmentName = a.depart;
            ///var departmentName = a.departname;



            string moduleid = "select m.module_id from Internal_module_Master m where m.module_name=:moduleName";
            Dictionary<string, dynamic> parameter1 = new Dictionary<string, dynamic>
            {
                {"@moduleName",moduleName  },
            };
            moduleName user = MaafinDbHelper.ExecuteFormQueryData<List<moduleName>>(moduleid, parameter1)?.FirstOrDefault();

            var moduleId = user?.module_id;

            //--------------------------------------------------------------------------------------------------------
            string departid = "select d.dep_id from department_mst d where d.firm_id =4 and d.status=1 and d.dep_name=:departmentName";
            Dictionary<string, dynamic> parameter2 = new Dictionary<string, dynamic>
            {
                {"@departmentName",a.depart },
            };
            departmentName user1 = MaafinDbHelper.ExecuteFormQueryData<List<departmentName>>(departid, parameter2)?.FirstOrDefault();

            var departId = user1?.dep_id;


            //

            string checkExistenceQuery = "select count(*) as counts from MODULE_ASSIGN_MASTER where Module_Id = :moduleId and Department_Id = :departId";
            Dictionary<string, dynamic> existenceParams = new Dictionary<string, dynamic>
    {
        {"@moduleId", moduleId},
        {"@departId", departId},
    };
            Assigncount asscount = MaafinDbHelper.ExecuteFormQueryData<List<Assigncount>>(checkExistenceQuery, existenceParams)?.FirstOrDefault();
            //int count = existenceResult != null ? Convert.ToInt32(existenceResult.count) : 0;
            var count = asscount?.counts;


            if (count > 0)
            {
                // Handle the case where the combination already exists
                TempData["Message"] = "This module is already assigned to the selected department.";

                TempData["Success"] = "Warning";
                return RedirectToAction("AssignModule");
            }
            else
            {








                string LoginUser = "insert into Module_Assign_Master(Assignid, Module_Id,Department_Id,Created_By,Created_Date,Assignstatus)values(Assignid.nextval,:moduleId,:Department_Id,:userId,sysdate,'1')";



                Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@userId",userId  }, {"@Department_Id",departId  },{"@moduleId",moduleId  },


            };
                save Save = MaafinDbHelper.ExecuteFormQueryData<List<save>>(LoginUser, parameter)?.FirstOrDefault();

                TempData["Message"] = "Assigned successfully";
                TempData["Success"] = "Success";
                return RedirectToAction("AssignModule");
            }

           

    }


        public ActionResult SessionTimeout()
        {
            return View();
        }

    }
}