﻿using Microsoft.AspNetCore.Authorization;
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
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDAO;
        //private readonly IUserDAO userDAO;

        public AccountController (IAccountDAO dao)
        {
            this.accountDAO = dao;

        }

        [HttpGet] 
        public ActionResult<UserAccount> GetAccountBalance()
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);

            return Ok(this.accountDAO.GetAccountBalance(userId));
        }

    }
}
