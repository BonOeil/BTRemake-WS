using GameShared.Messages;
using Microsoft.AspNetCore.SignalR;

namespace GameServer
{
    public class GameHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task StartScenario(string scenarioName, string gameName)
        {
            // Clear DB ? or create new DB as gameName

            // Read locations

            // Read OoB

            // Save into DB

            // Informer les autres joueurs de la connexion
            await Clients.All.SendAsync(nameof(ScenarioLoaded), new ScenarioLoaded());

            Console.WriteLine($"Scenario loaded: {scenarioName} ({Context.ConnectionId})");
        }
    }
}
