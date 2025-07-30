// <copyright file="PilotStatus.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.OOB
{
    public enum PilotStatus
    {
        None,
        Active,
        Injured,

        /// <summary>
        /// The inactive status. Removed from service. Due to injuries or other conditions.
        /// </summary>
        Inactive,
        Mia,
        Kia,
    }
}
