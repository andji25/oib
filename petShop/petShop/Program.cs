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
        IPetRepository petRepository = new JsonPetRepository();
        IReceiptRepository receiptRepository = new JsonReceiptRepository();

        ILogService logService = new FileLogService();

        IPetService petService = new PetService(petRepository, logService);

        ISalesService salesService;
        try
        {
            salesService = SalesServiceFactory.Create(receiptRepository, petRepository, logService);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Working hours: 08–22");
            Console.ReadKey();
            return;
        }


        List<User> users = new List<User>
            {
                new User("manager", "man1", "Marko", "Markovic", Role.Manager),
                new User("seller", "sel1", "Jovan", "Jovanic", Role.Seller)
            };

        IAuthService authService = new AuthService(users);


        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== LOGIN ===");
            Console.WriteLine("X. Exit");

            Console.Write("Username: ");
            string username = Console.ReadLine();
            if (username.Equals("X", StringComparison.OrdinalIgnoreCase))
                return;

            Console.Write("Password: ");
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
            catch (Exception ex)
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

                        Species species;
                        while (true)
                        {
                            Console.Write("Species (0-Mammal, 1-Reptile, 2-Rodent): ");
                            string input = Console.ReadLine();

                            if (int.TryParse(input, out int value) && Enum.IsDefined(typeof(Species), value))
                            {
                                species = (Species)value;
                                break;
                            }

                            Console.WriteLine("Invalid species. Please enter 0, 1 or 2.");
                        }

                        Console.Write("Price: ");
                        decimal price;
                        while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
                        {
                            Console.WriteLine("Invalid price. Enter positive number:");
                        }


                        petService.AddPet(new Pet(latin, name, species, price));
                        Console.WriteLine("Pet added.");
                        break;

                    case "2":
                        List<Pet> pets = petService.GetAllPets().ToList();

                        if (!pets.Any())
                        {
                            Console.WriteLine("No pets available.");
                        }
                        else
                        {
                            foreach (Pet p in pets)
                                Console.WriteLine($"{p.Name} - {p.Species} - Sold: {p.Sold}");
                        }
                        break;

                    case "3":
                        List<Receipt> receipts = salesService.GetAllReceipts().ToList();

                        if (!receipts.Any())
                        {
                            Console.WriteLine("No receipts available.");
                        }
                        else
                        {
                            foreach (Receipt r in salesService.GetAllReceipts())
                                Console.WriteLine($"{r.Seller.Name} {r.Seller.Surname} | {r.TotalAmount} | {r.DateTimeSale}");
                        }
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
                        List<Pet> availablePets = petService.GetAvailablePets().ToList();

                        if (!availablePets.Any())
                        {
                            Console.WriteLine("No pets available.");
                            break;
                        }

                        Console.WriteLine("Available pets:");
                        for (int i = 0; i < availablePets.Count; i++)
                        {
                            Console.WriteLine($"{availablePets[i].Name} - {availablePets[i].SellingPrice}");
                        }
                        break;

                    case "2":
                        List<Pet> pets = petService.GetAvailablePets().ToList();

                        if (!pets.Any())
                        {
                            Console.WriteLine("No pets available.");
                            break;
                        }

                        Console.WriteLine("Available pets:");
                        for (int i = 0; i < pets.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {pets[i].Name} - {pets[i].SellingPrice}");
                        }

                        Console.Write("Choose pet number to sell: ");
                        if (!int.TryParse(Console.ReadLine(), out int choiceNumber))
                        {
                            Console.WriteLine("Invalid input.");
                            break;
                        }

                        if (choiceNumber < 1 || choiceNumber > pets.Count)
                        {
                            Console.WriteLine("Number out of range.");
                            break;
                        }

                        Pet pet = pets[choiceNumber - 1];
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