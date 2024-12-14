using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_Report.App_Code;
using User_Report.Entities;
using User_Report.Models;

namespace User_Report.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login(LoginModel t)
        {

            ViewBag.Message = TempData["Message"];
            var id = t.UserName;
            Session["userid"] = t.UserName;


            string check = "select count(*) as count from logininternal ad where ad.emp_code=:user_id and ad.password_int =:lg_password";
            Dictionary<string, dynamic> parameter = new Dictionary<string, dynamic>
            {
                {"@user_id", t.UserName  },
                {"@lg_password", t.PassWord  },
            };


            LogInCount temp = MaafinDbHelper.ExecuteFormQueryData<List<LogInCount>>(check, parameter)?.FirstOrDefault();
            Session["user_id"] = t.UserName;
            var LogInCounts = temp?.COUNT;
            //admin

            if (id != 0)
            {
                if (temp?.COUNT == 1)
                {
                    //check = "select lm.user_id,lm.adv_name,lm.lg_password from legal_login_maafin lm where lm.user_id= :user_id and lm.role_id=3";
                    check = "select l.emp_code, l.emp_name, l.password_int, em.branch_id, em.department_id, em.post_id from employee_master em, logininternal l where em.emp_code = l.emp_code and l.branch_id = 0 and em.department_id = 738 and em.post_id=520 and em.status_id=1 and l.emp_code=:id";
                    parameter = new Dictionary<string, dynamic>
                {
                    {"@id", id  },
                };
                    User user = MaafinDbHelper.ExecuteFormQueryData<List<User>>(check, parameter)?.FirstOrDefault();

                    TempData["user"] = user?.EMP_CODE;

                    if (user != null)
                    {
                        return RedirectToAction("DashBoard", "Admin");
                    }


                }

                //techle

                if (temp?.COUNT == 1)
                {
                    //check = "select lm.user_id,lm.adv_name,lm.lg_password from legal_login_maafin lm where lm.user_id= :user_id and lm.role_id=3";
                    check = "select l.emp_code, l.emp_name, l.password_int, em.branch_id, em.department_id, em.post_id from employee_master em, logininternal l where em.emp_code = l.emp_code and l.branch_id = 0 and em.department_id = 738 and em.post_id=886 and em.status_id=1 and l.emp_code=:id";
                    parameter = new Dictionary<string, dynamic>
                {
                    {"@id", id  },
                };
                    User user = MaafinDbHelper.ExecuteFormQueryData<List<User>>(check, parameter)?.FirstOrDefault();

                    TempData["user"] = user?.EMP_CODE;

                    if (user != null)
                    {
                        return RedirectToAction("TechDashboard", "Tech");
                    }

                }

                //Developer


                if (temp?.COUNT == 1)
                {
                    //check = "select lm.user_id,lm.adv_name,lm.lg_password from legal_login_maafin lm where lm.user_id= :user_id and lm.role_id=3";
                    check = "select l.emp_code, l.emp_name, l.password_int, em.branch_id, em.department_id, em.post_id from employee_master em, logininternal l where em.emp_code = l.emp_code and l.branch_id = 0 and em.department_id = 738 and em.post_id = 920 and em.status_id = 1 and l.emp_code =:id";
                    parameter = new Dictionary<string, dynamic>
                {
                    {"@id", id  },
                };
                    User user = MaafinDbHelper.ExecuteFormQueryData<List<User>>(check, parameter)?.FirstOrDefault();

                    TempData["user"] = user?.EMP_CODE;

                    if (user != null)
                    {
                        return RedirectToAction("Developer_Dashboard", "Developer");
                    }

                }




                // User Login

                if (temp?.COUNT == 1)
                {
                    //check = "select lm.user_id,lm.adv_name,lm.lg_password from legal_login_maafin lm where lm.user_id= :user_id and lm.role_id=3";
                    check = "select l.emp_code, l.emp_name, l.password_int, em.branch_id, em.department_id, em.post_id from employee_master em, logininternal l where em.emp_code = l.emp_code and l.branch_id = 0 and  em.status_id=1 and l.emp_code=:id";
                    parameter = new Dictionary<string, dynamic>
                {
                    {"@id", id  },
                };
                    User user = MaafinDbHelper.ExecuteFormQueryData<List<User>>(check, parameter)?.FirstOrDefault();

                    TempData["user"] = user?.EMP_CODE;
                    Session["departmentID"] = user?.department_id;
                    Session["postID"] = user?.post_id;

                    if (user != null)
                    {
                        return RedirectToAction("UserDashboard", "User");
                    }

                }

                //techlead login
                //if (temp?.COUNT == 1)
                //{
                //    //check = "select lm.user_id,lm.adv_name,lm.lg_password from legal_login_maafin lm where lm.user_id= :user_id and lm.role_id=3";
                //    check = "select l.emp_code, l.emp_name, l.password_int, em.branch_id, em.department_id, em.post_id, p.post_name from employee_master em, logininternal l, post_mst p where em.emp_code = l.emp_code and l.branch_id = 0 and em.department_id = 738 and em.post_id = 886 and em.status_id = 1 and em.post_id = p.post_id; and l.emp_code = :id";
                //    parameter = new Dictionary<string, dynamic>
                //{
                //    {"@id", id  },
                //};
                //    User user = MaafinDbHelper.ExecuteFormQueryData<List<User>>(check, parameter)?.FirstOrDefault();

                //    TempData["user"] = user?.EMP_CODE;

                //    if (user != null)
                //    {
                //        return RedirectToAction("TechDashboard", "Tech");
                //    }

                //}



                //finance and accounts
                //if (temp?.COUNT == 1)
                //{
                //    //check = "select lm.user_id,lm.adv_name,lm.lg_password from legal_login_maafin lm where lm.user_id= :user_id and lm.role_id=3";
                //    check = "select l.emp_code, l.emp_name, l.password_int, em.branch_id, em.department_id, em.post_id from employee_master em, logininternal l where em.emp_code = l.emp_code and l.branch_id = 0 and em.department_id = 454 and  em.status_id=1 and l.emp_code=:id";
                //    parameter = new Dictionary<string, dynamic>
                //{
                //    {"@id", id  },
                //};
                //    User user = MaafinDbHelper.ExecuteFormQueryData<List<User>>(check, parameter)?.FirstOrDefault();

                //    TempData["user"] = user?.EMP_CODE;

                //    if (user != null)
                //    {
                //        return RedirectToAction("User_Dashboard", "Dashboard");
                //    }

                //}

                //Legal
                //if (temp?.COUNT == 1)
                //{
                //    //check = "select lm.user_id,lm.adv_name,lm.lg_password from legal_login_maafin lm where lm.user_id= :user_id and lm.role_id=3";
                //    check = "select l.emp_code, l.emp_name, l.password_int, em.branch_id, em.department_id, em.post_id from employee_master em, logininternal l where em.emp_code = l.emp_code and l.branch_id = 0 and em.department_id = 434 and  em.status_id=1 and l.emp_code=:id";
                //    parameter = new Dictionary<string, dynamic>
                //{
                //    {"@id", id  },
                //};
                //    User user = MaafinDbHelper.ExecuteFormQueryData<List<User>>(check, parameter)?.FirstOrDefault();

                //    TempData["user"] = user?.EMP_CODE;

                //    if (user != null)
                //    {
                //        return RedirectToAction("User_Dashboard", "Dashboard");
                //    }

                //}








                else
                    TempData["Message"] = "Invalid Username or Password !";
                return View("Login");
            }
            return View();

        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login", "Login");
        }
    }
}