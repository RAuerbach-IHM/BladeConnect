using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Services.LocalServices
{
    public interface ILocalCallService
    {
        Task<String> Call(long panelID, string callTo);

        Task<String> Call(long deviceID, int channel, string callTo);

        //Task<String> HangUp(SBButton sbButton);

        Task<String> Take(string source, string destination);   //From Custom XY Route Panel (iOS)
    }
}
