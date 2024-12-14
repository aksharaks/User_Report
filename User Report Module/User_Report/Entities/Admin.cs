using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User_Report.Entities
{
    public class Admin
    {
    }
    public class DashBoardCounts
    {
        public double? GL_AUM { get; set; }
        public int? GL_AUMAsInteger => (int)GL_AUM;
    }
    public class LogInCount
    {
        [JsonProperty("count")]
        public double COUNT { get; set; }

        [JsonIgnore]
        public int CountAsInteger => (int)COUNT;
    }

    public class User
    {
        public double EMP_CODE { get; set; }
        public int EMP_CODEAsInteger => (int)EMP_CODE;
        public string EMP_NAME { get; set; }
        public string PASSWORD_INT { get; set; }
        public double department_id { get; set; }
        public int department_idAsInteger => (int)department_id;
        public double post_id { get; set; }
        public int post_idAsInteger => (int)post_id;

    }
    public class LoginUser 
    { 
    public string emp_name { get; set; }
        public string dep_name { get; set; }
        public string post_name { get; set; }


    }

    public class LoginTech
    {
        public string emp_name { get; set; }
        public string dep_name { get; set; }
        public string post_name { get; set; }


    }




    public class save
    {
    }

    public class moduleName
    {
        public string module_name { get; set; }

        public double module_id { get; set; }
        public int module_idAsInteger => (int)module_id;
    }

    public class departmentName
    {
        public string dep_name { get; set;}
        public double dep_id { get; set;}

        public int dep_idAsInteger => (int)dep_id;
    }
    public class ticket
    {
     public double compliant_id { get; set; }
        public int compliant_idASInteger => (int)compliant_id;
        public string compliantname { get; set; }
    }

    public class developer
    {
        public double emp_code { get; set; }
        public int emp_codeAsInteger => (int)emp_code;
        public string emp_name { get; set; }
    }


    //assigned ticket of developer
    public class assign

    {
        public double compliant_id { get; set; }
        public int compliant_idASInteger => (int)compliant_id;

        public double developer_id { get; set; }
        public int developer_idASInteger { get; set; }


    }


}