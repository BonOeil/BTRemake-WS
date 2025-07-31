// <copyright file="IPilotService.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services.Interfaces
{
    using System;
    using GameShared.Game.OOB;

    public interface IPilotService
    {
        Pilot Get(Guid pilotId);

        void AddFatigue(Pilot pilot);

        void CrashPlane(Pilot pilot);
    }
}
