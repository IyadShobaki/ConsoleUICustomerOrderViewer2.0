using ConsoleUI.Models;
using ConsoleUI.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    class Program
    {
        private static string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CustomerOrderViewer;Integrated Security=True";
        private static readonly CustomerOrderCommand _customerOrderCommand = new CustomerOrderCommand(_connectionString);
        private static readonly CustomerCommand _customerCommand = new CustomerCommand(_connectionString);
        private static readonly ItemCommand _itemCommand = new ItemCommand(_connectionString);

        static void Main(string[] args)
        {
            try
            {
                var continueManaging = true;
                var userId = string.Empty;

                Console.WriteLine("What is your username?");
                userId = Console.ReadLine();

                do
                {
                    Console.WriteLine("1 - Show All | 2 - Upsert Customer Order | " +
                        "3 - Delete Customer Order | 4 - Exit");
                    int option = Convert.ToInt32(Console.ReadLine());

                    if (option == 1)
                    {
                        ShowAll();
                    }
                    else if (option == 2)
                    {
                        UpsertCustomerOrder(userId);
                    }
                    else if (option == 3)
                    {
                        DeleteCustomerOrder(userId);
                    }
                    else if (option == 4)
                    {
                        continueManaging = false;
                    }
                    else
                    {
                        Console.WriteLine("Option not found.");
                    }
                } while (continueManaging);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: {0}", ex.Message);
            }

            Console.ReadLine();
        }

        private static void ShowAll()
        {
            Console.WriteLine("{0}All Customer Orders: {1}", Environment.NewLine, Environment.NewLine);
            DisplayCustomerOrders();

            Console.WriteLine("{0}All Customers: {1}", Environment.NewLine, Environment.NewLine);
            DisplayCustomers();

            Console.WriteLine("{0}All Items: {1}", Environment.NewLine, Environment.NewLine);
            DisplayItems();

            Console.WriteLine();
        }

        private static void DisplayItems()
        {
            IList<ItemModel> items = _itemCommand.GetList();

            if (items.Any())
            {
                foreach (var item in items)
                {
                    Console.WriteLine($"{item.ItemId}: Description {item.Description}, Price {item.Price}");
                }
            }
        }

        private static void DisplayCustomers()
        {
            IList<CustomerModel> customers = _customerCommand.GetList();

            if (customers.Any())
            {
                foreach (var c in customers)
                {
                    Console.WriteLine($"{c.CustomerId}: First Name: {c.FirstName}, Middle Name: {c.MiddleName ?? "N/A"}, Last Name: {c.LastName}, Age {c.Age}");
                }
            }
        }

        private static void DisplayCustomerOrders()
        {
            IList<CustomerOrderDetailModel> customerOrderDetails = _customerOrderCommand.GetList();

            if (customerOrderDetails.Any())
            {
                foreach (var co in customerOrderDetails)
                {
                    Console.WriteLine($"{co.CustomerOrderId}: Fullname: {co.FirstName} {co.LastName} (Id : {co.CustomerId}) - purchased {co.Description} for {co.Price} (Id: {co.ItemId})");

                }
            }
        }

        private static void DeleteCustomerOrder(string userId)
        {
            Console.WriteLine("Enter CustomerOrderId:");
            int customerOrderId = Convert.ToInt32(Console.ReadLine());

            _customerOrderCommand.Delete(customerOrderId, userId);

        }

        private static void UpsertCustomerOrder(string userId)
        {
            Console.WriteLine("Note: For updating insert existing CustomerOrderId, for new entries enter -1.");
           
            Console.WriteLine("Enter CustomerOrderId:");
            int newCustomerOrderId = Convert.ToInt32(Console.ReadLine());
           
            Console.WriteLine("Enter CustomerId:");
            int newCustomerId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter ItemId:");
            int newItemId = Convert.ToInt32(Console.ReadLine());

            _customerOrderCommand.Upsert(newCustomerOrderId, newCustomerId, newItemId, userId);


        }

       
    }
}
