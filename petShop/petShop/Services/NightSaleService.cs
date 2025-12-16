using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using petShop.Model;
using petShop.Services;

public class NightSalesService : ISalesService
{
    private readonly List<Receipt> receipts;

    public NightSalesService(List<Receipt> receipts)
    {
        this.receipts = receipts;
    }

    public IReadOnlyCollection<Receipt> GetAllReceipts()
    {
        throw new NotImplementedException();
    }

    public Receipt SellPet(Pet pet)
    {
        if (Session.CurrentUser == null || Session.CurrentUser.Role != Role.Seller)
            throw new UnauthorizedAccessException();

        pet.MarkAsSold();

        int taxedAmount = (int)(pet.SellingPrice * 1.10);

        var receipt = new Receipt(Session.CurrentUser, taxedAmount);
        receipts.Add(receipt);

        return receipt;
    }

    Receipt ISalesService.SellPet(Pet pet)
    {
        throw new NotImplementedException();
    }
}