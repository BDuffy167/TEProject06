using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class UserAccount
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
    }
}
