using System;
using System.Collections.Generic;
// Bakery Payment System -- Capstone Project -- Makayla Baker -- a00304791 -- IOT-1026 -- OOP Evan Poitras 
// This project simulates a payment system at a bakery
// It allows you to select your bakery item, then select additional items
// It says the amount you owe, then you can select your payment method, cash, debit or credit card
// Credit card makes you put in a pin
// Error-handling by allowing you to put in correct response if input is invalid
namespace BakeryPaymentSystem
{
    abstract class PaymentMethod // common base
    {
        public double Balance { get; set; } // current account balance

        public PaymentMethod(double balance) // initializes the PaymentMethod with a starting balance
        {
            Balance = balance; // sets the initial balance
        }

        public abstract bool ProcessPayment(double amount); // it processes the payment of the specified amount and returns true if successful otherwise false
    }

    class Cash : PaymentMethod // Cash payment method 
    {
        public Cash(double balance) : base(balance) { } // passes the initial balance to the parent PaymentMethod class

        public override bool ProcessPayment(double amount)
        {
            if (Balance >= amount) 
            {
                Balance -= amount; // deduct payment from available cash
                Console.WriteLine("Cash payment successful."); // transaction success
                return true;
            }
            Console.WriteLine("Not enough cash."); // failed transaction, insufficient funds
            return false;
        }
    }

    class DebitCard : PaymentMethod // Debit payment method
    {
        public DebitCard(double balance) : base(balance) { } // passes the initial balance to the parent PaymentMethod class

        public override bool ProcessPayment(double amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount; // deduct payment from available cash
                Console.WriteLine("Debit payment successful."); // transaction success
                return true;
            }
            Console.WriteLine("Insufficient debit funds."); // not success
            return false;
        }
    }

    class CreditCard : PaymentMethod // Credit payment method
    {
        private string Pin; // pin for security measures on credit card

        public CreditCard(double limit, string pin) : base(limit) //initializes the credit limit using the base class and sets the card PIN
        {
            Pin = pin; // assign the provided pin to the card
        }

        public override bool ProcessPayment(double amount) //this method will handle charging an amount to the credit card and return whether it was successful
        {
            while (true)
            {
                Console.Write("Enter PIN: "); // prompts user to enter pin
                string input = Console.ReadLine();

                if (input == Pin)
                    break;

                Console.WriteLine("Incorrect PIN. Try again."); // security measure
            }

            if (Balance >= amount) // verifies if there is enough credit balance to cover the payment
            {
                Balance -= amount; // deduct the payment amount from available credit
                Console.WriteLine("Credit payment approved."); // sufficient funds on credit card
                return true;
            }

            Console.WriteLine("Credit limit exceeded."); // simulates not having enough money
            return false;
        }
    }

    class BakeryItem // bakery item class
    {
        public string Name { get; set; } // for item name
        public double Price { get; set; } // for item cost

        public BakeryItem(string name, double price)
        {
            Name = name; // set name
            Price = price; // set price
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Sweet Crumbs Bakery\n"); // simulation introduction

            List<BakeryItem> menu = new List<BakeryItem>() // menu list
            {
                new BakeryItem("Bread", 5),
                new BakeryItem("Cake", 20),
                new BakeryItem("Cookies", 10) // items you can purchase and their amount $$
            };

            double total = 0; // total cost
            bool addingItems = true; // loop control

            while (addingItems) // loop for ordering
            {
                Console.WriteLine("Menu:");
                // displays menu items
                for (int i = 0; i < menu.Count; i++) 
                {
                    Console.WriteLine($"{i + 1}. {menu[i].Name} - ${menu[i].Price}");
                }

                int choice; // user input
                while (true) // input validation loop
                {
                    Console.Write("Select item: ");
                    if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= menu.Count) //validate # + range
                        break;

                    Console.WriteLine("Invalid input. Please enter a valid item number."); // error-handling
                }

                total += menu[choice - 1].Price; // add the item price to total
                Console.WriteLine($"Added {menu[choice - 1].Name}. Current total: ${total}\n"); // confirmation method

                while (true)
                {
                    Console.Write("Would you like to add another item? (y/n): "); // if the user wants more items
                    string response = Console.ReadLine().ToLower();

                    if (response == "y") // continue adding items
                        break;
                    else if (response == "n") // do not add more items
                    {
                        addingItems = false;
                        break;
                    } 
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");// invalid input
                    }
                }
            }

            Console.WriteLine($"\nFinal total: ${total}\n"); // final cost displayed
            // amounts available in each payment form + credit card pin
            PaymentMethod cash = new Cash(15);
            PaymentMethod debit = new DebitCard(50);
            PaymentMethod credit = new CreditCard(100, "1234");
            // payment tracking
            bool success = false;
            int attempts = 0;

            while (!success && attempts < 3)
            {
                int payChoice;
                while (true)
                {
                    Console.WriteLine("Choose payment method:");// prompts user to choose an option: 1,2 or 3.
                    Console.WriteLine("1. Cash");
                    Console.WriteLine("2. Debit");
                    Console.WriteLine("3. Credit");

                    // validate payment choice input
                    if (int.TryParse(Console.ReadLine(), out payChoice) && payChoice >= 1 && payChoice <= 3)
                        break;

                    Console.WriteLine("Invalid input. Please enter 1, 2, or 3."); // error handling
                }

                attempts++; // increase attempt count for payment

                switch (payChoice) // choose choice of payment 
                {
                    case 1:
                        success = cash.ProcessPayment(total);
                        break;
                    case 2:
                        success = debit.ProcessPayment(total);
                        break;
                    case 3:
                        success = credit.ProcessPayment(total);
                        break;
                }

                if (!success && attempts < 3) // check if retry is needed
                {
                    Console.WriteLine($"Attempt {attempts} failed. Try again.\n");
                }
            }

            if (success)
            {
                Console.WriteLine("Transaction complete. Thank you for your purchase.");
            }
            else
            {
                Console.WriteLine("Transaction failed after 3 attempts.");
            }
        }
    }
}
