using petShop.Model;
using petShop.Services;
using System;
using System.Collections.Generic;

public class DaySalesService : ISalesService
{
    private readonly List<Receipt> receipts;

    public DaySalesService(List<Receipt> receipts)
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

        int discountedAmount = (int)(pet.SellingPrice * 0.85);

        var receipt = new Receipt(Session.CurrentUser, discountedAmount);
        receipts.Add(receipt);

        return receipt;
    }

    Receipt ISalesService.SellPet(Pet pet)
    {
        throw new NotImplementedException();
    }
}