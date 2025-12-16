using petShop.Model;
using System;
using System.Collections.Generic;

namespace petShop.Services
{
    public class SalesService : ISalesService
    {
        private readonly List<Receipt> receipts = new List<Receipt>();
        public Receipt SellPet(Pet pet)
        {
            if (Session.CurrentUser == null)
                throw new UnauthorizedAccessException();

            if (Session.CurrentUser.Role != Role.Seller)
                throw new UnauthorizedAccessException("Only seller can sell pets.");

            pet.MarkAsSold();

            Receipt receipt = new Receipt(Session.CurrentUser, pet.SellingPrice);
            receipts.Add(receipt);

            return receipt;
        }
        public IReadOnlyCollection<Receipt> GetAllReceipts()
        {
            if (Session.CurrentUser == null)
                throw new UnauthorizedAccessException();

            if (Session.CurrentUser.Role != Role.Manager)
                throw new UnauthorizedAccessException("Only manager can view receipts.");

            return receipts.AsReadOnly();
        }


    }
}
