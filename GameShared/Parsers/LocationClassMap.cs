// <copyright file="LocationClassMap.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Parsers
{
    using CsvHelper.Configuration;
    using GameShared.Game.Entities;

    public class LocationClassMap : ClassMap<Location>
    {
        public LocationClassMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.Name).Name("Name");
            Map(m => m.ControllingFaction).Name("ControllingFaction");
            Map(m => m.Position.Latitude).Name("Latitude");
            Map(m => m.Position.Longitude).Name("Longitude");
        }
    }
}
