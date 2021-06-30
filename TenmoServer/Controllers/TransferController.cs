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
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private readonly IUserDAO userDAO;

        //public TransferController(ITransferDAO dao)
        //{
        //    this.userDAO = dao;
        //}



        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            //int userId = int.Parse(this.User.FindFirst("sub").Value);

            return Ok(this.userDAO.GetUsers());
        }
    }
}
