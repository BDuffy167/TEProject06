using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDAO;
        public AccountController (IAccountDAO dao)
        {
            this.accountDAO = dao;
        }

        [HttpGet]
        public ActionResult<UserAccount> GetAccountBalance(User user)
        {
            return Ok(this.accountDAO.GetAccountBalance(user));
        }

    }
}
