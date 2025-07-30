// <copyright file="PlaneSquadron.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.OOB
{
    using System;
    using GameShared.Persistence;

    public class PlaneSquadron : BaseEntity
    {
        public Guid PlaneModelId { get; set; }

        /// <summary>
        /// Gets or sets the pilot identifier.
        /// Nullable : Can have no assigned pilot.
        /// </summary>
        public Guid? PilotId { get; set; }

        public double HitPoints { get; set; }

        public double Maintenance { get; set; }
    }
}
