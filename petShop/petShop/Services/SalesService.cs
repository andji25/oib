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
            logService.Log(LogType.INFO, "Attempt to sell pet");

            if (Session.CurrentUser == null)
            {
                logService.Log(LogType.ERROR, "Sell failed - no user logged in");
                throw new UnauthorizedAccessException();
            }

            if (Session.CurrentUser.Role != Role.Seller)
            {
                logService.Log(LogType.WARNING, "Sell failed - non seller logged in");
                throw new UnauthorizedAccessException("Only seller can sell pets.");
            }

            if (pet.Sold)
            {
                logService.Log(LogType.WARNING, "Sell failed - pet already sold");
                throw new InvalidOperationException("Pet already sold.");
            }

            pet.MarkAsSold();

            decimal finalAmount = CalculateFinalAmount(pet.SellingPrice);

            Receipt receipt = new Receipt(Session.CurrentUser, finalAmount);
            receiptRepository.Add(receipt);

            logService.Log(LogType.INFO, $"Pet sold for {finalAmount}.");

            return receipt;
        }
        public IReadOnlyCollection<Receipt> GetAllReceipts()
        {
            logService.Log(LogType.INFO, $"Attempt to get all receipts");

            if (Session.CurrentUser == null)
            {
                logService.Log(LogType.ERROR, "Get receipts failed - no user logged in");
                throw new UnauthorizedAccessException();
            }

            if (Session.CurrentUser.Role != Role.Manager)
            {
                logService.Log(LogType.WARNING, "Get receipts failed - non manager user");
                throw new UnauthorizedAccessException("Only manager can view receipts.");
            }

            return receiptRepository.GetAll().AsReadOnly();
        }


    }
}
