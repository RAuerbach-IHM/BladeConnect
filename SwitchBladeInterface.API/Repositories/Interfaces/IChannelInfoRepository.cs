using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IChannelInfoRepository
    {
        Task<ChannelInfo> GetChannelInfo(long deviceID, int channelNumber);

        Task<List<ChannelInfo>> GetChannelInfo();

        Task<List<ChannelInfo>> GetChannelInfoByDevice(long deviceID);
        Task<List<ChannelInfo>> GetChannelInfoByDevices(long[] deviceIDs);
        Task<bool> SaveChannelInfoStatus(long deviceID, int channel, int status);

        Task<bool> SaveChannelInfoName(long deviceID, int channel, string name);

        Task<bool> SaveChannelInfoDetails(long deviceID, int channel, string[] details);

        //bool InsertOrUpdateIntoChannelInfo(List<ChannelInfo> channelInfo, string parameter);
        //bool DeleteFromChannelInfo(ChannelInfo channelInfo);
    }
}
