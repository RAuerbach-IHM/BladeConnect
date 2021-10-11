using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Services.LocalServices
{
    public interface ILocalDisplayEventTakeService
    {

        Task<String> Take(long roomID, long displayEventId, int option);
    }
}
