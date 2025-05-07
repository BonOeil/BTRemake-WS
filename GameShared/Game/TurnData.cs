using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShared.Game
{
    public class TurnData
    {
        public int CurrentTurn { get; set; } = 0;

        public GamePhase CurrentPhase { get; set; } = GamePhase.Planning;
    }
}
