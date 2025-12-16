using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using petShop.Model;

namespace petShop.Services
{
    public interface ILogService
    {
        void Log(LogType logType, string message);
    }
}
