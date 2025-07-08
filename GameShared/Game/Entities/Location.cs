// <copyright file="Location.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Entities
{
    using GameShared.Persistence;

    public class Location : BaseEntity
    {
        public Location(string name, GPSPosition position, Faction controllingFaction)
        {
            Name = name;
            Position = position;
            ControllingFaction = controllingFaction;
        }

        public Location()
        {
            Name = string.Empty;
            ControllingFaction = Faction.Allies;
            Position = default(GPSPosition);
        }

        public string Name { get; set; }

        public GPSPosition Position { get; set; }

        public Faction ControllingFaction { get; set; }
    }
}
