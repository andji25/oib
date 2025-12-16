using petShop.Model;
using petShop.Repository;
using petShop.Services;
using System;
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
        User manager = new User("manager1", "pass123", "Milan", "Markovic", Role.Manager);
        User seller = new User("seller1", "pass123", "Jovana", "Jovic", Role.Seller);

        Session.CurrentUser = manager;

        try
        {
            petService.AddPet(new Pet("Felis catus", "Maca", Species.Mammal, 100));
            petService.AddPet(new Pet("Canis lupus familiaris", "Pas", Species.Mammal, 200));
            Console.WriteLine("Manager added pets successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Session.CurrentUser = seller;

        try
        {
            var availablePets = petService.GetAvailablePets();
            foreach (var pet in availablePets)
            {
                var receipt = salesService.SellPet(pet);
                Console.WriteLine($"Pet sold: {pet.Name}, Final price: {receipt.TotalAmount}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Session.CurrentUser = manager;

        try
        {
            var allReceipts = salesService.GetAllReceipts();
            Console.WriteLine("\nAll receipts:");
            foreach (var r in allReceipts)
            {
                Console.WriteLine($"{r.Seller.Name} sold pet for {r.TotalAmount} at {r.DateTimeSale}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nSimulation finished.");
    }
}
