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

        public async Task LoadScenario(LoadScenario scenarioData)
        {
            await GameManagement.StartScenario(scenarioData.ScenarioName, scenarioData.InstanceName);

            // Informer les autres joueurs de la connexion
            await Clients.All.SendAsync(nameof(ScenarioLoaded), new ScenarioLoaded());

            Console.WriteLine($"Scenario loaded: {scenarioData.ScenarioName} ({Context.ConnectionId})");
        }
    }
}
