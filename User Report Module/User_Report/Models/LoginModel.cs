using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace User_Report.Models
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Please enter username")]
        public int UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string PassWord { get; set; }


        public int UserId { get; set; }
    }

}