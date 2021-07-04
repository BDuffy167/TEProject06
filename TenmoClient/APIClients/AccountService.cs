﻿using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class AccountService
    {
        private readonly string API_BASE_URL;
        private readonly RestClient client;

        public AccountService() // Add argument to pass private URL?
        {
            this.API_BASE_URL = AuthService.API_BASE_URL;

            this.client = new RestClient();
        }

        public void UpdateToken(string token) 
        {
            if (string.IsNullOrEmpty(token))
            {
                this.client.Authenticator = null;
            }
            else
            {
                this.client.Authenticator = new JwtAuthenticator(token);
            }
        }

        public decimal ShowAccountBalance()
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}account");
            //request.AddHeader("Authorization", "bearer " + API_User.user.Token); //Use manually until we can find a shortcut


            IRestResponse<UserAccount> response = client.Get<UserAccount>(request);
            if (response.IsSuccessful)
            {
                //API_UserAccount usersAccount = new API_UserAccount();
                UserAccount userAccount = response.Data;
                return userAccount.Balance;
            }
            else
            {
                //What do we return here with failed Get
                Console.WriteLine($"{(int)response.StatusCode} error occurred getting request"); // make more explicit later
                return 0;
            }
        }

        //public API_UserAccount GetAccountInfo()
        //{

        //}
    }
}
