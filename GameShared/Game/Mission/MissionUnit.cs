// <copyright file="MissionUnit.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Mission
{
    using GameShared.Game.OOB;
    using GameShared.Persistence;

    public class MissionUnit : BaseEntity
    {
        public IList<PlaneSquadron> Planes { get; set; } = new List<PlaneSquadron>();

        required public GPSPosition Position { get; set; }

        public double Orientation { get; set; }

        public ushort Altitude { get; set; }
    }
}
