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
            "VALUES(@TransferTypeId, @TransferStatusId, @AccountFrom, @AccountTo, @Amount) " +
            "SELECT @@IDENTITY AS 'id';";
        private readonly string UpdateBalanceOnTransferSql =
            "UPDATE accounts " +
            "SET balance -= @amount " +
            "WHERE user_id = @accountFrom " +
            "UPDATE accounts " +
            "SET balance += @amount " +
            "WHERE user_id = @accountTo "; 

        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<User> GetUsersForTransfer()
        {
            List<User> returnUsers = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT user_id, username FROM users", conn); // don't select self
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User u = GetUserFromReader(reader);
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

            }

        }
        private User GetUserFromReader(SqlDataReader reader)
        {
            return new User()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                Username = Convert.ToString(reader["username"]),
            };
        }
        //public bool InsertTransferToDatabase(Transfer transfer)
        //{

        //}
    }
}
