using System;
using System.Collections.Generic;
using System.Linq;
using TenmoClient.APIClients;
using TenmoClient.Data;
using TenmoServer.Models;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly AccountService accountService = new AccountService();
        private readonly TransferService transferService = new TransferService();

        //int test = UserService.UserId;

        private bool quitRequested = false;

        public void Start()
        {
            while (!quitRequested)
            {
                while (!authService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1: // View Balance

                            Console.WriteLine(accountService.ShowAccountBalance().ToString("C2")); // TODO: Implement me
                            break;
                        case 2: // View Past Transfers
                            List<Transfer> transfers = transferService.RequestUserTransfersFromServer();
                            consoleService.ShowPastTransfers(transfers);

                            break;
                        case 3: // View Pending Requests
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;
                        case 4: // Send TE Bucks
                            List<UserAccount> users = transferService.GetAllUsers(); //don't list current user
                            consoleService.ListAllUsers(users);
                            int transferId = consoleService.PromptForTransferID("send money.");

                            foreach (UserAccount user in users)
                            {
                                if (user.UserId == transferId)
                                {
                                    decimal userAmount = PromptForAmount();

                                    transferService.BeginMoneyTransfer((UserService.UserId + 1000), user.AccountId, userAmount);
                                }
                            }
                            break;
                        case 5: // Request TE Bucks
                            users = transferService.GetAllUsers(); //don't list current user
                            consoleService.ListAllUsers(users);
                            transferId = consoleService.PromptForTransferID("request money.");

                            foreach (UserAccount user in users)
                            {
                                if (user.UserId == transferId)
                                {
                                    decimal userAmount = 0;
                                    transferService.BeginMoneyTransfer(transferId, UserService.UserId, userAmount);
                                }
                            }
                            break;
                        case 6: // Log in as someone else
                            Console.WriteLine();
                            UserService.ClearLoggedInUser(); //wipe out previous login info
                            return; // Leaves the menu and should return as someone else
                        case 0: // Quit
                            Console.WriteLine("Goodbye!");
                            quitRequested = true;
                            return;
                        default:
                            Console.WriteLine("That doesn't seem like a valid choice.");
                            break;
                    }
                }
            } while (menuSelection != 0);
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();
                API_User user = authService.Login(loginUser);
                if (user != null)
                {
                    UserService.SetLogin(user);
                    this.accountService.UpdateToken(user.Token);
                    this.transferService.UpdateToken(user.Token);

                }
            }
        }
        public decimal PromptForAmount()
        {

            bool valid = false;
            while (!valid)
            {

                Console.WriteLine("How much would you like to transfer?");
                //decimal.TryParse(Console.ReadLine(), out decimal userInput);
                //Console.WriteLine($"{userInput}");
                //decimal userBalance = Convert.ToDecimal(accountService.ShowAccountBalance());
                //decimal userdecimal = Convert.ToDecimal(userBalance);
                decimal userBalance = accountService.ShowAccountBalance();
                if (!decimal.TryParse(Console.ReadLine(), out decimal userInput))
                {
                    Console.WriteLine("Invalid input. Only input a number.");
                    continue; ; //make this loop over prompt / response.
                }
                if (userInput <= userBalance)
                {
                    return userInput;
                }
                else
                {
                    Console.WriteLine("error");
                    continue;
                }
            }
            return 0;
        }
    }
}
