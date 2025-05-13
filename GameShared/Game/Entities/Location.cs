using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace GameShared.Game.Entities
{
    public class Location : BaseEntity
    {
        public string Name;
        public GPSPosition Position;
        public Faction ControllingFaction;

        public Location(string name, GPSPosition position, Faction controllingFaction)
        {
            Name = name;
            Position = position;
            ControllingFaction = controllingFaction;
        }
    }
}
