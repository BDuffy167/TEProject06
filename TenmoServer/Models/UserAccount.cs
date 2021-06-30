using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class UserAccount
    {
        public int User_Id { get; set; }
        public int Account_Id { get; set; }
        public decimal Balance { get; set; }
    }
}
