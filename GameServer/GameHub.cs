using GameShared;
using GameShared.Services;
using Microsoft.AspNetCore.SignalR;

namespace GameServer
{
    public class GameHub : Microsoft.AspNetCore.SignalR.Hub
    {
        // Dictionnaire contenant les joueurs connectés avec leur ID de connexion
        private static Dictionary<string, Player> _connectedPlayers = new Dictionary<string, Player>();
        private static GameState _gameState = new GameState();

        private ITurnServices TurnServices { get; set; }

        public GameHub(ITurnServices turnServices)
        {
            TurnServices = turnServices;
        }

        // Appelé quand un client se connecte
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connecté: {Context.ConnectionId}");
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
                Console.WriteLine($"Joueur déconnecté: {player.Name} ({Context.ConnectionId})");
            }

            await base.OnDisconnectedAsync(exception);
        }


        // Méthode appelée par le client pour rejoindre le jeu
        public async Task StepTurn()
        {
            var turnData = TurnServices.StepTurn();

            // Informer les autres joueurs de la connexion
            await Clients.Others.SendAsync("StepTurn", turnData);

            Console.WriteLine($"StepTurn");
        }

        // Méthode appelée par le client pour rejoindre le jeu
        public async Task JoinGame(string playerName)
        {
            Player newPlayer = new Player
            {
                Id = Context.ConnectionId,
                Name = playerName,
                Position = new Vector2(0, 0) // Position de départ
            };

            // Ajouter le joueur à la liste des joueurs connectés
            _connectedPlayers[Context.ConnectionId] = newPlayer;

            // Informer le joueur de son ID et de l'état actuel du jeu
            await Clients.Caller.SendAsync("JoinConfirmation", newPlayer.Id, _gameState);

            // Informer les autres joueurs de la connexion
            await Clients.Others.SendAsync("PlayerJoined", newPlayer);

            Console.WriteLine($"Joueur rejoint: {playerName} ({Context.ConnectionId})");
        }

        // Méthode appelée par le client pour mettre à jour sa position
        public async Task UpdatePosition(Vector2 position)
        {
            if (_connectedPlayers.TryGetValue(Context.ConnectionId, out Player? player))
            {
                player.Position = position;

                // Informer tous les autres clients de la nouvelle position
                await Clients.Others.SendAsync("PlayerMoved", Context.ConnectionId, position);
            }
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
