using System;
using System.Collections.Generic;
using TenmoClient.Data;
using TenmoServer.Models;

namespace TenmoClient
{
    public class ConsoleService
    {
        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Console.WriteLine("");
            Console.Write($"Please enter transfer ID to {action} (0 to cancel): ");

            if (!int.TryParse(Console.ReadLine(), out int accountId))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0; //make this loop over prompt / response.
            }

            return accountId;
        }

        public LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            return new LoginUser
            {
                Username = username,
                Password = password
            };
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }

            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");

            return pass;
        }

        public void ListAllUsers(List<UserAccount> users)
        {
            foreach (UserAccount user in users)
            {
                Console.WriteLine($"{user.UserId})  {user.Username}");
            }
        }

        public void ShowPastTransfers(List<Transfer> transfers)
        {
            Console.WriteLine("Transfers");
            Console.WriteLine("Id    From/To    Amount");

            foreach (Transfer t in transfers)
            {
                if (t.AccountFrom == UserService.UserId + 1000) // future proof way to get AccountId? Also format
                {
                    Console.WriteLine($"{t.Id}  To: {t.UserNameTo}  ${t.Amount}");
                }
                else if (t.AccountTo == UserService.UserId + 1000)
                {
                    Console.WriteLine($"{t.Id}  From: {t.UserNameTo}  ${t.Amount}");
                }

            }
                ShowSpecificTransfer(PromptForTransferID("view."), transfers);
        }

        public void ShowSpecificTransfer(int id, List<Transfer> transfers)
        {
            foreach (Transfer t in transfers)
            {
                if (t.Id == id)
                {
                    Console.WriteLine("Transfer Details");
                    Console.WriteLine("");
                    Console.WriteLine($"Id:     {t.Id}");
                    Console.WriteLine($"From:   {t.UserNameFrom}");
                    Console.WriteLine($"To:     {t.UserNameTo}");
                    Console.WriteLine($"Type:   {t.TypeName}");
                    Console.WriteLine($"Status: {t.StatusName}");
                    Console.WriteLine($"Amount: ${t.Amount}");
                }
            }
        }

        //public int FindTransferUserId(int transferId, List<API_User> users)
        //{
        //    foreach (API_User user in users)
        //    {
        //        if (user.UserId == transferId)
        //        {

        //        }
        //    }
        //}


    }

    //public decimal PromptForTransferAmount(string message)
    //{

    //}
}


