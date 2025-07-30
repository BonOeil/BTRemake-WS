// <copyright file="Squadron.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.OOB
{
    using GameShared.Persistence;

    public class Squadron : BaseEntity
    {
        public string Name { get; set; }

        public IList<PlaneSquadron> Planes { get; set; } = new List<PlaneSquadron>();
    }
}
