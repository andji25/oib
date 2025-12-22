using petShop.Model;
using petShop.Repository;
using petShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        IPetRepository petRepo = new JsonPetRepository();
        IReceiptRepository receiptRepo = new JsonReceiptRepository();

        ILogService logService = new FileLogService();

        IPetService petService = new PetService(petRepo, logService);

        ISalesService salesService = SalesServiceFactory.Create(receiptRepo, logService);

        List<User> users = new List<User>
            {
                new User("manager", "man1", "Menadzer", "Antonijevic", Role.Manager),
                new User("seller", "sel1", "Prodavac", "Antonijevic", Role.Seller)
            };

        IAuthService authService = new AuthService(users);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== LOGIN ===");

            Console.WriteLine("Username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            try
            {
                authService.Login(username, password);

                if (Session.CurrentUser.Role == Role.Manager)
                    ManagerMenu(petService, salesService);
                else
                    SellerMenu(petService, salesService);
                authService.Logout();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

    }

    static void ManagerMenu(IPetService petService, ISalesService salesService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== MANAGER MENU ===");
            Console.WriteLine("1. Add pet");
            Console.WriteLine("2. View all pets");
            Console.WriteLine("3. View receipts");
            Console.WriteLine("0. Logout");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Latin name: ");
                        string latin = Console.ReadLine();

                        Console.Write("Name: ");
                        string name = Console.ReadLine();

                        Console.Write("Species (0-Mammal,1-Reptile,2-Rodent): ");
                        Species species = (Species)int.Parse(Console.ReadLine());

                        Console.Write("Price: ");
                        decimal price = decimal.Parse(Console.ReadLine());

                        petService.AddPet(new Pet(latin, name, species, price));
                        Console.WriteLine("Pet added.");
                        break;

                    case "2":
                        foreach (Pet p in petService.GetAllPets())
                            Console.WriteLine($"{p.Name} - {p.Species} - Sold: {p.Sold}");
                        break;

                    case "3":
                        foreach (Receipt r in salesService.GetAllReceipts())
                            Console.WriteLine($"{r.Seller.Name} | {r.TotalAmount} | {r.DateTimeSale}");
                        break;

                    case "0":
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
    static void SellerMenu(IPetService petService, ISalesService salesService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SELLER MENU ===");
            Console.WriteLine("1. View available pets");
            Console.WriteLine("2. Sell pet");
            Console.WriteLine("0. Logout");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        foreach (Pet p in petService.GetAvailablePets())
                            Console.WriteLine($"{p.Id} - {p.Name} - {p.SellingPrice}");
                        break;

                    case "2":
                        List<Pet> pets = petService.GetAvailablePets().ToList();
                        if (!pets.Any())
                        {
                            Console.WriteLine("No pets available.");
                            break;
                        }

                        Pet pet = pets.First();
                        Receipt receipt = salesService.SellPet(pet);

                        Console.WriteLine($"Sold {pet.Name} for {receipt.TotalAmount}");
                        break;

                    case "0":
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}