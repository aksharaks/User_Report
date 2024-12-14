using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User_Report.Models
{
    public class UserModel
    {
    }

    public class Feedbacksave
    {
        public int feedback_id { get; set; }
        public double module_id { get; set; }
        public int module_idAsInteger => (int)module_id;

        public string feedbackname { get; set; }
        public string descriptions { get; set; }
        public string enhancement { get; set; }
        public double rating { get; set; }
        public int ratingAsInteger => (int)rating;
        public DateTime created_date { get; set; }
        public double created_by { get; set; }
        public int? created_byAsInteger => (int)created_by;
        public string module_name { get; set; }
        public string emp_name { get; set; }


    }
  


    public class Compliantsave
    {
       public double compliant_id {  get; set; }

        public int compliant_idAsInteger => (int)compliant_id;
        public double module_id { get; set; }
     public int module_idAsInteger => (int)module_id;
        public string module_name { get; set;}
        public string compliantname { get; set;}
        public string descriptions { get; set;}
        public double priority { get; set; }
        public int priorityAsInteger =>(int)priority;

        public DateTime created_date { get; set; }
        public double created_by { get; set; }
        public int? created_byAsInteger => (int)created_by;
        public int department_id {  get; set; }



    }

    public class listcompliant
    {
        //public double compliant_id { get;}
        //public int compliant_idAsInteger =>(int)compliant_id
        public double compliant_id { get; set; }
        public int compliant_idAsInteger => (int)compliant_id;
        public string module_name { get; set; }
        public string compliantname { get; set; }
        public string descriptions { get; set; }
        public string priority { get; set; }
        //public int priorityAsInteger => (int)priority;
        public DateTime created_date { get; set; }
        public double created_by { get; set; }
        public int? created_byAsInteger => (int)created_by;

        public int department_id {  get; set; }
        public string emp_name { get; set; }
        public string status { get; set; }
        //public double status {  get; set; }
        //public int statusAsInteger =>(int)status;


       public string comments { get; set; }

       

    }


    public class editTicket {
        public double compliant_id { get; set; }
        public int compliant_idAsInteger => (int)compliant_id;
    public string Status { get; set; }
        public DateTime return_date { get; set;}
        

        public string comments { get; set; }


    }
    public class tickethistory
    {
        public double compliant_id { get; set; }
        public int compliant_idAsInteger => (int)compliant_id;
        public string compliantname { get; set; }
        public string descriptions { get; set; }

        public DateTime created_date { get; set; }
        public string comments { get; set; }
        public DateTime return_date { get; set; }




    }
}