﻿using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using TenmoServer.Models;

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
            RestRequest request = new RestRequest($"{API_BASE_URL}transfer/users");


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

        public void BeginMoneyTransfer(int transferFromId, int transferToId, decimal amount) // account is valid for transfer $
        {
            Transfer transfer = new Transfer();
            transfer.AccountFrom = transferFromId;
            transfer.AccountTo = transferToId;
            transfer.Amount = amount;

            if (transferFromId == UserService.UserId) //path for sending money
            {
                transfer.Type = 1001; // code for "send"
                transfer.Status = 2001; // code for "approved"

                // Post new transfer
                PostNewTransfer(transfer);
                // Update both user's balances
            }
            else //path for requesting money
            {
                transfer.Type = 1000; // code for "request"
                transfer.Status = 2000; // code for "pending"
            }
        }

        private Transfer PostNewTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{API_BASE_URL}transfer");
            request.AddJsonBody(transfer);

            IRestResponse<Transfer> response = client.Post<Transfer>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred creating a new transfer.");

                return null;
            }

        }
    }
}