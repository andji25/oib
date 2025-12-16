using petShop.Model;
using petShop.Repository;
using petShop.Services;
using System;
using System.Collections.Generic;

public static class SalesServiceFactory
{
    public static ISalesService Create(IReceiptRepository receiptRepository, ILogService logService)
    {
        var now = DateTime.Now.TimeOfDay;

        if (now >= new TimeSpan(8, 0, 0) && now < new TimeSpan(16, 0, 0))
            return new DaySalesService(receiptRepository, logService);

        if (now >= new TimeSpan(16, 0, 0) && now < new TimeSpan(22, 0, 0))
            return new NightSalesService(receiptRepository, logService);

        throw new InvalidOperationException("Store is closed.");
    }
}