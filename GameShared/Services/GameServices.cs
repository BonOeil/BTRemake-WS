// <copyright file="GameServices.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services
{
    using System;
    using System.Threading.Tasks;
    using GameShared.Game;
    using GameShared.Game.Entities;
    using GameShared.Game.Mission;
    using GameShared.Messages;
    using GameShared.Persistence;
    using GameShared.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;

    public class GameServices : IGameServices
    {
        public GameServices(IRepository<Location> locationRepository, IRepository<MapUnit> mapUnitsRepository, IRepository<MissionPlan> missionPlanRepository, ILogger<GameServices> logger)
        {
            Logger = logger;
            LocationRepository = locationRepository;
            MapUnitsRepository = mapUnitsRepository;
            MissionPlanRepository = missionPlanRepository;
        }

        private IRepository<Location> LocationRepository { get; }

        private IRepository<MapUnit> MapUnitsRepository { get; }

        private IRepository<MissionPlan> MissionPlanRepository { get; }

        private ILogger<GameServices> Logger { get; }

        public async Task StartScenario(string scenarioName, string gameName)
        {
            try
            {
                // Checks
                ArgumentException.ThrowIfNullOrWhiteSpace(gameName);

                // Clear DB ? or create new DB as gameName
                // var dataBase = MongoClient.GetDatabase(gameName);

                // Read locations

                // Read OoB

                // Save into DB
                string resourcePath = Environment.GetEnvironmentVariable("RESOURCE_PATH") ?? Path.Combine(AppContext.BaseDirectory, "Ressources");

                var path = Path.Combine(resourcePath, "Scenarios", "Scenario1", "Locations.csv");

                await LocationRepository.AddAsync(path);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading a scenario");
            }
        }

        public async Task<FullGameState> Step()
        {
            var allMissions = await MissionPlanRepository.GetAllAsync();

            Parallel.ForEach(allMissions, async (mission) =>
            {
                foreach (var unitId in mission.UnitIds)
                {
                    var unit = await MapUnitsRepository.GetByIdAsync(unitId);

                    var bearing = GPSTools.CalculateBearing(unit.Position, mission.Target);
                    unit.Position = GPSTools.GetNewPosition(unit.Position, bearing, 50000);
                    await MapUnitsRepository.UpdateAsync(unit);
                }
            });

            Logger.LogTrace($"Step {allMissions.Count()} missions");

            return new FullGameState()
            {
                Units = (await MapUnitsRepository.GetAllAsync()).ToList(),
            };
        }
    }
}
