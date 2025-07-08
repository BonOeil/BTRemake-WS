// <copyright file="GameHub.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Hubs
{
    using GameShared.Messages;
    using Microsoft.AspNetCore.SignalR;

    public class GameHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public GameHub(ILogger<GameHub> logger)
        {
            Logger = logger;
        }

        private ILogger<GameHub> Logger { get; }

        public async Task LoadScenario(LoadScenario scenarioData)
        {
            // Informer les autres joueurs de la connexion
            await Clients.All.SendAsync(nameof(ScenarioLoaded), new ScenarioLoaded());
        }
    }
}
