// <copyright file="IGameServices.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GameShared.Messages;

    public interface IGameServices
    {
        Task StartScenario(string scenarioName, string gameName);

        Task<FullGameState> Step();
    }
}
