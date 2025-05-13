using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShared.Game;
using GameShared.Game.Entities;
using GameShared.Persistance;
using GameShared.Services.Interfaces;

namespace GameShared.Services
{
    public class GameManagement: IGameManagement
    {
        private IRepository<Location> LocationRepository { get; }

        public GameManagement(IRepository<Location> locationRepository) 
        {
            LocationRepository = locationRepository;
        }

        public async Task StartScenario(string scenarioName, string gameName)
        {
            // Clear DB ? or create new DB as gameName

            // Read locations

            // Read OoB

            // Save into DB
            await AddLocation("London", new GPSPosition(51.5074f, -0.1278f), Faction.Allies);
            await AddLocation("Berlin", new GPSPosition(52.5200f, 13.4050f), Faction.Axis);
        }

        public async Task AddLocation(string name, GPSPosition position, Faction controllingFaction)
        {
            Location location = new Location(name, position, controllingFaction);
            await LocationRepository.AddAsync(location);

            Debug.Write($"Added location {name} (ID: {location.Id}) at {position}");
        }
    }
}
