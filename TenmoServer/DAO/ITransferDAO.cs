using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        public List<UserAccount> GetUsersForTransfer();
        Transfer PostNewTransfer(Transfer transfer);
        void EnactTransferOfBlances(Transfer transfer);

        List<Transfer> GetAllTransfers();
    }
}
