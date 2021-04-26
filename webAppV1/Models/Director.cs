using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webAppV1.Models
{
    public class Director:User
    {
        public float Salary { get; set; }
        public DateTime HiringDate { get; set; }
    }
}