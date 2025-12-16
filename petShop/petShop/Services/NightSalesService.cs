using petShop.Repository;
using petShop.Services;

public class NightSalesService : SalesService
{
    public NightSalesService(IReceiptRepository receiptRepository, ILogService logService) : base(receiptRepository, logService) { }
    protected override decimal CalculateFinalAmount(decimal baseAmount)
    {
        return baseAmount * 1.10m;
    }
}