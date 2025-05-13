using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace GameShared.Game
{
    public class TurnData
    {
        public ObjectId Id { get; set; }

        public int CurrentTurn { get; set; } = 0;

        public GamePhase CurrentPhase { get; set; } = GamePhase.Planning;
    }
}
