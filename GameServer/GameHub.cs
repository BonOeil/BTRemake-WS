// <copyright file="GameHub.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer
{
    using GameShared.Messages;
    using GameShared.Services.Interfaces;
    using Microsoft.AspNetCore.SignalR;

    public class GameHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public GameHub(IGameServices gameServices, ILogger<GameHub> logger)
        {
            GameServices = gameServices;
            Logger = logger;
        }

        private IGameServices GameServices { get; }

        private ILogger<GameHub> Logger { get; }

        public async Task LoadScenario(LoadScenario scenarioData)
        {
            await GameServices.StartScenario(scenarioData.ScenarioName, scenarioData.InstanceName);

            // Informer les autres joueurs de la connexion
            await Clients.All.SendAsync(nameof(ScenarioLoaded), new ScenarioLoaded());

            Logger.LogInformation($"Scenario loaded: {scenarioData.ScenarioName} ({Context.ConnectionId})");
        }
    }
}
