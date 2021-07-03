using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public string UserNameTo { get; set; }
        public string UserNameFrom { get; set; }
        public int AccountTo { get; set; }
        public int AccountFrom { get; set; }
        public decimal Amount { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }

        public string TypeName
        {
            get
            {
                switch (this.Type)
                {
                    case 1000:
                        return "Request";
                    case 1001:
                        return "Send";
                    default:
                        return null;
                }
            }

        }
        public string StatusName
        {
            get
            {
                switch (this.Status)
                {
                    case 2000:
                        return "Pending";
                    case 2001:
                        return "Approved";
                    case 2002:
                        return "Rejected";
                    default:
                        return null;
                }
            }
        }

    }
}

