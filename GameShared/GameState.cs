using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShared
{
    public class GameState
    {
        public List<Player> Players { get; set; } = new List<Player>();
        // Autres informations sur l'état du jeu (carte, objets, etc.)
    }
}
