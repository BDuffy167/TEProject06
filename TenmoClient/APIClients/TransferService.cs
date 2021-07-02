using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class TransferService
    {
        private readonly string API_BASE_URL;
        private readonly RestClient client;

        public TransferService() // Add argument to pass private URL?
        {
            this.API_BASE_URL = AuthService.API_BASE_URL;

            this.client = new RestClient();
        }

        public void UpdateToken(string token) //this is happening on login but probably shouldnt
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

        public List<API_User> GetAllUsers()
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}transfer/user/account");
            

            IRestResponse<List<API_User>> response = client.Get<List<API_User>>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                //What do we return here with failed Get
                Console.WriteLine("There is a problem with the Get");
                return null;
            }
        }

        public void BeginMoneyTransfer(int transferFromId, int transferToId)
        {
            if (transferFromId == UserService.UserId)
            {

            }
            else
            {

            }
        }
    }
}
