using GameShared.Messages;
using GameShared.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace GameServer
{
    public class GameHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private IGameManagement GameManagement { get; }
        private ILogger<GameHub> Logger { get; }

        public GameHub(IGameManagement gameManagement, ILogger<GameHub> logger)
        {
            GameManagement = gameManagement;
            Logger = logger;
        }

        public async Task LoadScenario(LoadScenario scenarioData)
        {
            await GameManagement.StartScenario(scenarioData.ScenarioName, scenarioData.InstanceName);

            // Informer les autres joueurs de la connexion
            await Clients.All.SendAsync(nameof(ScenarioLoaded), new ScenarioLoaded());

            Logger.LogInformation($"Scenario loaded: {scenarioData.ScenarioName} ({Context.ConnectionId})");
        }
    }
}
