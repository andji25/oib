using petShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop.Services
{
    public interface ISalesService
    {
        Receipt SellPet(Pet pet);
        IReadOnlyCollection<Receipt> GetAllReceipts();
    }
}
