using petShop.Model;
using petShop.Services;
using System;
using System.Collections.Generic;

public static class SalesServiceFactory
{
    public static ISalesService Create(List<Receipt> receipts)
    {
        var now = DateTime.Now.TimeOfDay;

        if (now >= new TimeSpan(8, 0, 0) && now < new TimeSpan(16, 0, 0))
            return new DaySalesService(receipts);

        if (now >= new TimeSpan(16, 0, 0) && now < new TimeSpan(22, 0, 0))
            return new NightSalesService(receipts);

        throw new InvalidOperationException("Store is closed.");
    }
}