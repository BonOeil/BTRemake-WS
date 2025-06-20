﻿// <copyright file="GameServices.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using GameShared.Game;
    using GameShared.Game.Entities;
    using GameShared.Persistence;
    using GameShared.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;

    public class GameServices : IGameServices
    {
        public GameServices(IMongoClient mongoClient, IRepository<Location> locationRepository, ILogger<GameServices> logger)
        {
            Logger = logger;
            MongoClient = mongoClient;
            LocationRepository = locationRepository;
        }

        private IMongoClient MongoClient { get; }

        private IRepository<Location> LocationRepository { get; }

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
                // await dataBase.CreateCollectionAsync(nameof(Location));
                // await AddLocation("London", new GPSPosition(51.5074f, -0.1278f), Faction.Allies);
                // await AddLocation("Berlin", new GPSPosition(52.5200f, 13.4050f), Faction.Axis);
                string resourcePath = Environment.GetEnvironmentVariable("RESOURCE_PATH") ?? Path.Combine(Directory.GetCurrentDirectory(), "Ressources");

                var path = Path.Combine(resourcePath, "Scenarios", "Scenario1", "Locations.json");
                // string jsonString = File.ReadAllText(path.ToString());
                // var documentOptions = new JsonDocumentOptions
                // {
                //    CommentHandling = JsonCommentHandling.Skip,
                // };
                // using JsonDocument document = JsonDocument.Parse(jsonString, documentOptions);

                await LocationRepository.AddAsync(path);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading a scenario");
            }
        }

        public async Task AddLocation(string name, GPSPosition position, Faction controllingFaction)
        {
            Location location = new Location(name, position, controllingFaction);
            await LocationRepository.AddAsync(location);

            Logger.LogTrace($"Added location {name} (ID: {location.Id}) at {position}");
        }
    }
}
