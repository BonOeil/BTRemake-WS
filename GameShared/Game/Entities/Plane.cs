// <copyright file="Plane.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Entities
{
    using GameShared.Persistence;

    public class Plane : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public int MaxSpeed { get; set; }

        public int MaxAltitude { get; set; }
    }
}
