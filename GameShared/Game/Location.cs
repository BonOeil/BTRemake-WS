using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShared.Game
{
    public class Location
    {
        public string Id;
        public string Name;
        public GPSPosition Position;
        public Faction ControllingFaction;

        public Location(string id, string name, GPSPosition position, Faction controllingFaction)
        {
            Id = id;
            Name = name;
            Position = position;
            ControllingFaction = controllingFaction;
        }
    }
}
