// <copyright file="Location.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Bson;

    public class Location : BaseEntity
    {
        public Location(string name, GPSPosition position, Faction controllingFaction)
        {
            Name = name;
            Position = position;
            ControllingFaction = controllingFaction;
        }

        public string Name { get; set; }

        public GPSPosition Position { get; set; }

        public Faction ControllingFaction { get; set; }
    }
}
