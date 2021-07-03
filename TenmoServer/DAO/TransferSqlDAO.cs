using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO
    {
        private readonly string connectionString;

        private readonly string insertNewTransferSql =  
            "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
            "VALUES(@TransferTypeId, @TransferStatusId, @AccountFrom, @AccountTo, @Amount);";
        private readonly string UpdateBalanceOnTransferSql =
            "UPDATE accounts " +
            "SET balance -= @amount " +
            "WHERE account_id = @accountFrom; " +
            "UPDATE accounts " +
            "SET balance += @amount " +
            "WHERE account_id = @accountTo; ";
        private string GetUserAccountSql = "SELECT a.user_id, a.account_id, a.balance, u.username " +
            "FROM accounts a " +
            "JOIN users u ON a.user_id = u.user_id";
            

        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<UserAccount> GetUsersForTransfer()
        {
            List<UserAccount> returnUsers = new List<UserAccount>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(GetUserAccountSql, conn); // don't select self
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserAccount u = GetUserFromReader(reader);
                        returnUsers.Add(u);
                    }

                }
            }

            return returnUsers;
        }

        public Transfer PostNewTransfer(Transfer transfer) //change name?
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(insertNewTransferSql, conn);

                cmd.Parameters.AddWithValue("@TransferTypeId", transfer.Type);
                cmd.Parameters.AddWithValue("@TransferStatusId", transfer.Status);
                cmd.Parameters.AddWithValue("@AccountFrom", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@AccountTo", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@Amount", transfer.Amount);
                cmd.ExecuteNonQuery();
                //SqlDataReader reader = cmd.ExecuteReader();
                //reader.Read();
                //transfer.Id = Convert.ToInt32(reader["id"]);


                //transfer.Id = Convert.ToInt32(cmd.ExecuteScalar());


            }
                

            return transfer;
        }
        public void EnactTransferOfBlances(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(UpdateBalanceOnTransferSql, conn);
                cmd.Parameters.AddWithValue("@AccountFrom", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@AccountTo", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@Amount", transfer.Amount);
                cmd.ExecuteNonQuery();
            }

        }
        private UserAccount GetUserFromReader(SqlDataReader reader)
        {
            return new UserAccount()
            {
                Username = Convert.ToString(reader["username"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                AccountId = Convert.ToInt32(reader["account_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };
        }
        //public bool InsertTransferToDatabase(Transfer transfer)
        //{

        //}
    }
}
