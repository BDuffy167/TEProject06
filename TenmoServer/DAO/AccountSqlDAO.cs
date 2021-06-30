using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        private string GetAccountBalanceSQL = "SELECT user_id, account_id, balance FROM accounts WHERE user_id = @User_Id";

        public UserAccount GetAccountBalance(int userId)
        {
            //UserAccount result = new UserAccount();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(GetAccountBalanceSQL, conn);
                cmd.Parameters.AddWithValue("@User_Id", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return GetAccountFromReader(reader);
                }

            }
            return null;

        }

        private UserAccount GetAccountFromReader(SqlDataReader reader)
        {
            return new UserAccount()
            {
                User_Id = Convert.ToInt32(reader["user_id"]),
                Account_Id = Convert.ToInt32(reader["account_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };
        }
    }
}
