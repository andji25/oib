using System;
using System.Linq;
using petShop.Model;

public class FiscalService
{
    private IRepository<FiscalReceipt> _receipts;
    private IRepository<Pet> _pets;
    private LoggingService _logger;

    public FiscalService(
        IRepository<FiscalReceipt> receipts,
        IRepository<Pet> pets,
        LoggingService logger
    )
    {
        _receipts = receipts;
        _pets = pets;
        _logger = logger;
    }

    public void Sell(Guid petId, string seller, ISalesService salesService)
    {
        var pet = _pets.GetAll().First(p => p.Id == petId);
        if (pet.Sold)
            throw new Exception("Ljubimac je već prodat.");

        pet.Sold = true;
        _pets.Update(pet);

        var total = salesService.CalculateTotal(pet.Price);

        _receipts.Add(new FiscalReceipt
        {
            SellerName = seller,
            TotalAmount = total
        });

        _logger.Log($"Sold {pet.Name} for {total}");
    }
}
