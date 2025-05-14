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
using MongoDB.Driver;

namespace GameShared.Services
{
    public class GameManagement: IGameManagement
    {
        private IMongoClient MongoClient { get; }
        private IRepository<Location> LocationRepository { get; }

        public GameManagement(IMongoClient mongoClient, IRepository<Location> locationRepository) 
        {
            MongoClient = mongoClient;
            LocationRepository = locationRepository;
        }

        public async Task StartScenario(string scenarioName, string gameName)
        {
            try
            {
                // Checks
                ArgumentException.ThrowIfNullOrWhiteSpace(gameName);

                // Clear DB ? or create new DB as gameName
                //var dataBase = MongoClient.GetDatabase(gameName);

                // Read locations

                // Read OoB

                // Save into DB
                //await dataBase.CreateCollectionAsync(nameof(Location));
                await AddLocation("London", new GPSPosition(51.5074f, -0.1278f), Faction.Allies);
                await AddLocation("Berlin", new GPSPosition(52.5200f, 13.4050f), Faction.Axis);

            }
            catch (Exception ex)
             {
            }
        }

        public async Task AddLocation(string name, GPSPosition position, Faction controllingFaction)
        {
            Location location = new Location(name, position, controllingFaction);
            await LocationRepository.AddAsync(location);

            Debug.Write($"Added location {name} (ID: {location.Id}) at {position}");
        }
    }
}
