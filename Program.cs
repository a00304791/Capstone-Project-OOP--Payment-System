using System;
using System.Collections.Generic;
// Bakery Payment System -- Capstone Project -- Makayla Baker -- a00304791 -- IOT-1026 -- OOP Evan Poitras 
// This porject simulates a payment system at a bakery
// It allows you to select your bakery item, then select additional items
// It says the amount you owe, then you can select your payment method, cash, debit or credit card
// Credit card makes you put in a pin
// Error-handling by allowing you to put in correct response if input is invalid
namespace BakeryPaymentSystem
{
    abstract class PaymentMethod
    {
        public double Balance { get; set; }

        public PaymentMethod(double balance)
        {
            Balance = balance;
        }

        public abstract bool ProcessPayment(double amount);
    }

    class Cash : PaymentMethod
    {
        public Cash(double balance) : base(balance) { }

        public override bool ProcessPayment(double amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine("Cash payment successful.");
                return true;
            }
            Console.WriteLine("Not enough cash.");
            return false;
        }
    }

    class DebitCard : PaymentMethod
    {
        public DebitCard(double balance) : base(balance) { }

        public override bool ProcessPayment(double amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine("Debit payment successful.");
                return true;
            }
            Console.WriteLine("Insufficient debit funds.");
            return false;
        }
    }

    class CreditCard : PaymentMethod
    {
        private string Pin;

        public CreditCard(double limit, string pin) : base(limit)
        {
            Pin = pin;
        }

        public override bool ProcessPayment(double amount)
        {
            while (true)
            {
                Console.Write("Enter PIN: ");
                string input = Console.ReadLine();

                if (input == Pin)
                    break;

                Console.WriteLine("Incorrect PIN. Try again.");
            }

            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine("Credit payment approved.");
                return true;
            }

            Console.WriteLine("Credit limit exceeded.");
            return false;
        }
    }

    class BakeryItem
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public BakeryItem(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Sweet Crumbs Bakery\n");

            List<BakeryItem> menu = new List<BakeryItem>()
            {
                new BakeryItem("Bread", 5),
                new BakeryItem("Cake", 20),
                new BakeryItem("Cookies", 10)
            };

            double total = 0;
            bool addingItems = true;

            while (addingItems)
            {
                Console.WriteLine("Menu:");
                for (int i = 0; i < menu.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {menu[i].Name} - ${menu[i].Price}");
                }

                int choice;
                while (true)
                {
                    Console.Write("Select item: ");
                    if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= menu.Count)
                        break;

                    Console.WriteLine("Invalid input. Please enter a valid item number.");
                }

                total += menu[choice - 1].Price;
                Console.WriteLine($"Added {menu[choice - 1].Name}. Current total: ${total}\n");

                while (true)
                {
                    Console.Write("Would you like to add another item? (y/n): ");
                    string response = Console.ReadLine().ToLower();

                    if (response == "y")
                        break;
                    else if (response == "n")
                    {
                        addingItems = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                    }
                }
            }

            Console.WriteLine($"\nFinal total: ${total}\n");

            PaymentMethod cash = new Cash(15);
            PaymentMethod debit = new DebitCard(50);
            PaymentMethod credit = new CreditCard(100, "1234");

            bool success = false;
            int attempts = 0;

            while (!success && attempts < 3)
            {
                int payChoice;
                while (true)
                {
                    Console.WriteLine("Choose payment method:");
                    Console.WriteLine("1. Cash");
                    Console.WriteLine("2. Debit");
                    Console.WriteLine("3. Credit");

                    if (int.TryParse(Console.ReadLine(), out payChoice) && payChoice >= 1 && payChoice <= 3)
                        break;

                    Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
                }

                attempts++;

                switch (payChoice)
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

                if (!success && attempts < 3)
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
