using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User_Report.Models
{
    public class TechModel
    {
    }


    public class TicketSearch
    {
        public double compliant_id { get; set; }
        public int compliant_idAsInteger => (int)compliant_id;

        public string module_name { get; set; }
        public string compliantname { get; set; }
        public string descriptions { get; set; }
        public string priority { get; set; }
        

        public DateTime created_date { get; set; }
        public double created_by { get; set; }
        public int? created_byAsInteger => (int)created_by;
        public string comments { get; set; }
    }


    //public class  ticket_Assign

    //{

    //    public double CA_id { get; set; }
    //    public int CA_idAsInteger => (int)CA_id;
    //    public double compliant_id { get; set; }
    //    public int compliant_idAsInteger => (int)compliant_id;
    //    public string module_name { get; set; }
    //    public string compliant_name { get; set;}
    //    public string descriptions { get; set; }
    //    public string priority { get; set; }

    //    public DateTime req_date { get; set;}
    //    public DateTime assign_date { get; set;}

    //    public string developer { get; set; }



    public class ticket_Assign
    {

        public double assign_no { get; set; }
        public int assign_noAsInteger => (int)assign_no;
        public double developer_id {  get; set; }
        public int developer_idAsInteger => (int)developer_id;
        public DateTime assign_date { get; set;}
        public double compliant_id { get;set;}

        public int compliant_idAsInt => (int)compliant_id;




    }
    public class Update
    {

    }
    public class ticket_submit
    {
        public double compliant_id { get; set; }
        public int compliant_idAsInteger => (int)compliant_id;
        public string development {  get; set; }
        public string status { get; set; }

        public string comments { get; set; }

        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set;}


    }

    public class AssignReport
    {
        public double compliant_id { get; set; }
        public int compliant_idAsInteger => (int)compliant_id;

        public string compliantname { get; set; }
        public string module_name { get; set; }
        public double developer_id { get; set; }
        public int developer_idAsInteger => (int)developer_id;
        public DateTime? created_date { get; set; }
        public string descriptions { get; set; }
        public string priority { get; set; }
        public string emp_name { get; set; }
        public string status { get; set; }
        public string comments { get; set; }    
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string created_by { get; set; }
        public string developer_name { get; set; }





    }

}
