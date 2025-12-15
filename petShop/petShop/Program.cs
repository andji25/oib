using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

class Program
{
    static void Main()
    {
        try
        {
            var logger = new LoggingService();

            var userRepo = new JsonRepository<User>("Data/users.json");
            var petRepo = new JsonRepository<Pet>("Data/pets.json");
            var receiptRepo = new JsonRepository<FiscalReceipt>("Data/receipts.json");

            InitDataService.InitUsers(userRepo);
            InitDataService.InitPets(petRepo);

            var auth = new AuthService(userRepo, logger);
            var petService = new PetService(petRepo, logger);
            var fiscal = new FiscalService(receiptRepo, petRepo, logger);

            Console.Write("Username: ");
            var u = Console.ReadLine();
            Console.Write("Password: ");
            var p = Console.ReadLine();

            var user = auth.Login(u, p);
            if (user == null)
            {
                Console.WriteLine("❌ Pogrešni podaci.");
                return;
            }

            if (user.Role == Role.Manager)
                ManagerMenu(petService);
            else
                SellerMenu(petService, fiscal, user.Username);
        }
        catch (Exception ex)
        {
            File.AppendAllText("Data/log.txt", ex + "\n");
            Console.WriteLine("❌ Greška u radu aplikacije.");
        }
    }

    static void ManagerMenu(PetService petService)
    {
        try
        {
            Console.Write("Ime ljubimca: ");
            string name = Console.ReadLine();
            Console.Write("Cena: ");
            decimal price = decimal.Parse(Console.ReadLine());

            petService.AddPet(new Pet
            {
                Name = name,
                Price = price,
                Type = PetType.Mammal
            });

            Console.WriteLine("✅ Ljubimac dodat.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ " + ex.Message);
        }
    }

    static void SellerMenu(PetService petService, FiscalService fiscal, string seller)
    {
        try
        {
            var pets = petService.GetAvailable().ToList();
            if (!pets.Any())
            {
                Console.WriteLine("Nema dostupnih ljubimaca.");
                return;
            }

            for (int i = 0; i < pets.Count; i++)
                Console.WriteLine($"{i}. {pets[i].Name} - {pets[i].Price}");

            Console.Write("Izaberi: ");
            int choice = int.Parse(Console.ReadLine());

            fiscal.Sell(
                pets[choice].Id,
                seller,
                SalesFactory.GetCurrent()
            );

            Console.WriteLine("✅ Prodaja uspešna.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ " + ex.Message);
        }
    }
}
