using petShop.Model;
using petShop.Repository;
using System;
using System.Collections.Generic;

namespace petShop.Services
{
    public abstract class SalesService : ISalesService
    {
        protected readonly IReceiptRepository receiptRepository;
        protected readonly ILogService logService;
        protected SalesService(IReceiptRepository receiptRepository, ILogService logService)
        {
            this.receiptRepository = receiptRepository;
            this.logService = logService;
        }
        protected abstract decimal CalculateFinalAmount(decimal baseAmount);
        public Receipt SellPet(Pet pet)
        {
            if (Session.CurrentUser == null)
                throw new UnauthorizedAccessException();

            if (Session.CurrentUser.Role != Role.Seller)
                throw new UnauthorizedAccessException("Only seller can sell pets.");

            if (pet.Sold)
                throw new InvalidOperationException("Pet already sold.");

            pet.MarkAsSold();

            decimal finalAmount = CalculateFinalAmount(pet.SellingPrice);

            Receipt receipt = new Receipt(Session.CurrentUser, finalAmount);
            receiptRepository.Add(receipt);

            logService.Log(LogType.INFO, $"Pet sold for {finalAmount}");

            return receipt;
        }
        public IReadOnlyCollection<Receipt> GetAllReceipts()
        {
            if (Session.CurrentUser == null)
                throw new UnauthorizedAccessException();

            if (Session.CurrentUser.Role != Role.Manager)
                throw new UnauthorizedAccessException("Only manager can view receipts.");

            return receiptRepository.GetAll().AsReadOnly();
        }


    }
}
