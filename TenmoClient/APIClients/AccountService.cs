using RestSharp;
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

        //public void UpdateToken(string token) //this is happening on login but probably shouldnt
        //{
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        this.client.Authenticator = null;
        //    }
        //    else
        //    {
        //        this.client.Authenticator = new JwtAuthenticator(token);
        //    }
        //}

        public string ShowAccountBalance()
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}user/account");
            //request.AddHeader("Authorization", "bearer " + API_User.user.Token); //Use manually until we can find a shortcut

            IRestResponse<API_UserAccount> response = client.Get<API_UserAccount>(request);
            if (response.IsSuccessful)
            {
                //API_UserAccount usersAccount = new API_UserAccount();
                API_UserAccount usersAccount = response.Data;
                return usersAccount.Balance.ToString("C2");
            }
            else
            {
                //What do we return here with failed Get
                Console.WriteLine("There is a problem with the Get"); // make more explicit later
                return null;
            }
        }

        
    }
}
