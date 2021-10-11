
using SwitchBladeInterface.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface IStationsRepository
    {
        Task<IEnumerable<Station>> GetStationsByAccount(long accountId);
        Task<IEnumerable<Station>> GetStations();

        Task<Station> GetStation(String callLetters);

        Task<bool> SaveStation(Station station);
        Task<string> DeleteStation(long stationId);
    }
}
