﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int Id { get; set; }
        public string UserNameTo { get; set; }
        public string UserNameFrom { get; set; }
        public int AccountTo { get; set; }
        public int AccountFrom { get; set; }
        public decimal Amount { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }

    }
}
