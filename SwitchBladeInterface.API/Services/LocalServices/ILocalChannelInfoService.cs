using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Services.LocalServices
{
    public interface ILocalChannelInfoService
    {
        Task<String> RequestUpdateFromDeviceAllChannels();
    }
}
