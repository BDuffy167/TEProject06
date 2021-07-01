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
    [Route("[controller]/user/account/")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDAO transferDAO;

        public TransferController(ITransferDAO dao)
        {
            this.transferDAO = dao;
        }



        [HttpGet]
        //[AllowAnonymous]
        public ActionResult<List<User>> GetUsers()
        {
            //int userId = int.Parse(this.User.FindFirst("sub").Value);

            return Ok(this.transferDAO.GetUsersForTransfer());
        }
    }
}
