using System;

namespace petShop.Model
{
    public class Receipt
    {
        public Guid Id { get; private set; }
        public User Seller { get; private set; }
        public DateTime DateTimeSale { get; private set; }
        public int TotalAmount { get; private set; }

        public Receipt(User seller, int totalAmount)
        {
            if (seller.Role != Role.Seller)
                throw new InvalidOperationException("Only seller can issue fiscal receipt.");

            Id = Guid.NewGuid();
            Seller = seller;
            DateTimeSale = DateTime.Now;
            TotalAmount = totalAmount;
        }

    }
}
