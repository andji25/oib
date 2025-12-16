using petShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petShop.Repository
{
    public interface IReceiptRepository
    {
        void Add(Receipt receipt);
        List<Receipt> GetAll();
    }
}
