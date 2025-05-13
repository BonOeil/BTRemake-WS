using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShared.Game;
using GameShared.Game.Entities;

namespace GameShared
{
    public class GameState
    {
        public World World { get; set; } = new World();

        public Dictionary<string, List<Order>> PendingOrders { get; set; } = new Dictionary<string, List<Order>>();
    }
}
