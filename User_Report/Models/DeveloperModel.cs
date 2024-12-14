using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User_Report.Models
{
    public class DeveloperModel
    {
    }



    public class Ticketcount
    {
        public double counts { get; set; }
        public int countsAsInteger => (int)counts;
    }

    public class TicketList 
    
    
    {
        public double compliant_id { get; set; }
        public int compliant_idAsInteger => (int)compliant_id;
       
        public string compliantname { get; set; }
        public string module_name { get; set; }

        public DateTime? assign_date { get; set; }
       
        public string status { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }

        public string comments { get; set; }
        public string created_by { get; set; }



    }
}
