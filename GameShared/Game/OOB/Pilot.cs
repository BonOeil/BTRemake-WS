// <copyright file="Pilot.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.OOB
{
    using GameShared.Persistence;

    public class Pilot : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;

        public PilotStatus Status { get; set; } = PilotStatus.Active;

        public PiloteQualifications Qualification { get; set; } = PiloteQualifications.None;

        /// <summary>
        /// Gets or sets the morale. [0-100]%.
        /// </summary>
        public double Morale { get; set; }

        /// <summary>
        /// Gets or sets the fatigue. [0-100]%.
        /// </summary>
        public double Fatigue { get; set; }

        /// <summary>
        /// Gets or sets the experience. [0-100]%.
        /// </summary>
        public double Experience { get; set; }

        /// <summary>
        /// Gets or sets the victories. More than 5 victories makes a pilot an Ace.
        /// </summary>
        public int Victories { get; set; }
    }
}
