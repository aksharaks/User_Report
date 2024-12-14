using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User_Report.Models
{
    public class AdminModel
    {
      
    }
    public class ModuleSave
    {
        public string module_name { get; set; }
        public string module_description { get; set; }
        public double created_by { get; set; }
        public int? created_byAsInteger =>(int)created_by;
        public DateTime created_date { get; set;}
    }
    public class assignSave 
    {
    public string modulename { get; set; }
        public string depart { get; set; }
        public string departname { get; set; }
    
    
    }

    public class ListModule 
    {

        public string module_name { get; set; }
        public string module_description { get; set; }
        public string created_by { get; set; }
        //public int? created_byAsInteger => (int)created_by;
        public DateTime created_date { get; set; }

    }


    public class ListAssignmodule 
    
    {

        public string module_name { get; set; }


        public string dep_name { get; set; }



    }

    public class Assigncount
    {
       
        public double counts { get; set; }
        public int countsAsInteger => (int)counts;

    }


    public class Assigncounts
    {
        [JsonProperty("COUNT(*)")]
        public double COUNT { get; set; }
        public int COUNTAsInteger =>(int)COUNT;

    }
}
