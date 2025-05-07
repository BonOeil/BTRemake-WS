using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShared.Game
{
    public class TurnManager
    {
        public int CurrentTurn { get; set; } = 0;
        public GamePhase CurrentPhase { get; set; } = GamePhase.Planning;
    }
}
