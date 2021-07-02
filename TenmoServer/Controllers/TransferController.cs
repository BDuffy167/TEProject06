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
        private readonly ITransferDAO transferDAO;

        public TransferController(ITransferDAO dao)
        {
            this.transferDAO = dao;
        }



        [HttpGet("users")]
        public ActionResult<List<UserAccount>> GetUsers()
        {
            //int userId = int.Parse(this.User.FindFirst("sub").Value);

            return Ok(this.transferDAO.GetUsersForTransfer());
        }

        [HttpPost] // figure out URL
        public ActionResult<Transfer> PostNewTransfer(Transfer transfer)
        {
            Transfer newTransfer = this.transferDAO.PostNewTransfer(transfer);
            if (newTransfer.Status == 2001)
            {
                this.transferDAO.EnactTransferOfBlances(newTransfer);
            }
            return Created($"/transfer/{newTransfer.Id}", newTransfer);
        }
    }
}
