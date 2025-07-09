// <copyright file="MapUnit.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Mission
{
    using GameShared.Persistence;

    public class MapUnit : BaseEntity
    {
        public string Name { get; set; }

        public GPSPosition Position { get; set; }
    }
}
