// <copyright file="TestHub.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Hubs
{
    using GameShared;
    using GameShared.Services;
    using Microsoft.AspNetCore.SignalR;

    public class TestHub : Hub
    {
        // Dictionnaire contenant les joueurs connectés avec leur ID de connexion
        private static Dictionary<string, Player> _connectedPlayers = new Dictionary<string, Player>();

        public TestHub(ITurnServices turnServices, ILogger<TestHub> logger)
        {
            TurnServices = turnServices;
            Logger = logger;
        }

        private ILogger<TestHub> Logger { get; }

        private ITurnServices TurnServices { get; set; }

        // Appelé quand un client se connecte
        public override async Task OnConnectedAsync()
        {
            Logger.LogInformation($"Client connecté: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        // Appelé quand un client se déconnecte
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Supprimer le joueur de la liste des joueurs connectés
            if (_connectedPlayers.ContainsKey(Context.ConnectionId))
            {
                Player player = _connectedPlayers[Context.ConnectionId];
                _connectedPlayers.Remove(Context.ConnectionId);

                // Informer les autres joueurs de la déconnexion
                await Clients.Others.SendAsync("PlayerDisconnected", player.Id);
                Logger.LogInformation($"Joueur déconnecté: {player.Name} ({Context.ConnectionId})");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Méthode appelée par le client pour rejoindre le jeu
        public async Task StepTurn()
        {
            var turnData = TurnServices.StepTurn();

            // Informer les autres joueurs de la connexion
            await Clients.Others.SendAsync("StepTurn", turnData);

            Logger.LogInformation($"StepTurn");
        }

        // Méthode appelée par le client pour rejoindre le jeu
        public async Task JoinGame(string playerName)
        {
            Player newPlayer = new Player
            {
                Id = Context.ConnectionId,
                Name = playerName,
            };

            // Ajouter le joueur à la liste des joueurs connectés
            _connectedPlayers[Context.ConnectionId] = newPlayer;

            // Informer le joueur de son ID et de l'état actuel du jeu
            await Clients.Caller.SendAsync("JoinConfirmation", newPlayer.Id, await TurnServices.GetTurn());

            // Informer les autres joueurs de la connexion
            await Clients.Others.SendAsync("PlayerJoined", newPlayer);

            Logger.LogInformation($"Joueur rejoint: {playerName} ({Context.ConnectionId})");
        }

        // Méthode appelée par le client pour envoyer une action
        public async Task SendAction(string actionType, object actionData)
        {
            // Traiter l'action côté serveur si nécessaire

            // Retransmettre l'action aux autres joueurs
            await Clients.Others.SendAsync("PlayerAction", Context.ConnectionId, actionType, actionData);
        }
    }
}
