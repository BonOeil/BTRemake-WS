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
    using GameShared.Game.OOB;
    using GameShared.Messages;
    using GameShared.Persistence;
    using GameShared.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;

    public class GameServices : IGameServices
    {
        public GameServices(IRepository<Location> locationRepository, IRepository<MissionUnit> missionUnitRepository, IRepository<Squadron> squadronsRepository, IRepository<MissionPlan> missionPlanRepository, ILogger<GameServices> logger)
        {
            Logger = logger;
            LocationRepository = locationRepository;
            SquadronsRepository = squadronsRepository;
            MissionPlanRepository = missionPlanRepository;
            MissionUnitRepository = missionUnitRepository;
        }

        private IRepository<Location> LocationRepository { get; }

        private IRepository<Squadron> SquadronsRepository { get; }

        private IRepository<MissionUnit> MissionUnitRepository { get; }

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

            // For all active missions, move and act
            Parallel.ForEach(allMissions, async (mission) =>
            {
                foreach (var unitId in mission.UnitIds)
                {
                    var unit = await MissionUnitRepository.GetByIdAsync(unitId);

                    // Move all units
                    unit.Orientation = GPSTools.CalculateBearing(unit.Position, mission.Target);
                    unit.Position = GPSTools.GetNewPosition(unit.Position, unit.Orientation, unit.Altitude);
                    await MissionUnitRepository.UpdateAsync(unit);

                    // Act all units
                    // Attack, bomb, reco...
                    // If hit point reach 0, ennemi squadron gain moral
                }
            });

            // For all AA, fire

            // For all active missions, handle damages and maintenance
            // Crash planes if damaged, Loose moral to squadron
            // Add maintenance
            // Add fatigue

            // For all inactive planes and squadrons
            // Do maintenance
            // Do repairs

            // For all inactive pilotes
            // Rest
            // Gain moral

            Logger.LogTrace($"Step {allMissions.Count()} missions");

            return new FullGameState()
            {
                Squadrons = (await SquadronsRepository.GetAllAsync()).ToList(),
            };
        }
    }
}
