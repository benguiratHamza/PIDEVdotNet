﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webAppV1.Models
{
    public class ActivityComment
    {
        public long id { get; set; }

        public string content { get; set; }

        public DateTime creationDate { get; set; }
    }
}