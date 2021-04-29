using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webAppV1.Models
{
    public class Appointment
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public String beginhour { get; set; }
        public string endhour { get; set; }
        public string description { get; set; }

    }
}