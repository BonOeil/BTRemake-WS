using GameShared.Messages;
using GameShared.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace GameServer
{
    public class GameHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private IGameManagement GameManagement { get; }

        public GameHub(IGameManagement gameManagement)
        {
            GameManagement = gameManagement;
        }

        public async Task StartScenario(string scenarioName, string gameName)
        {
            await GameManagement.StartScenario(scenarioName, gameName);

            // Informer les autres joueurs de la connexion
            await Clients.All.SendAsync(nameof(ScenarioLoaded), new ScenarioLoaded());

            Console.WriteLine($"Scenario loaded: {scenarioName} ({Context.ConnectionId})");
        }
    }
}
