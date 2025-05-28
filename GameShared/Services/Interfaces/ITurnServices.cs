// <copyright file="ScenarioLoaded.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShared.Game.Entities;

namespace GameShared.Services
{
    public interface ITurnServices
    {
        Task<TurnData> StepTurn();

        Task<TurnData> GetTurn();
    }
}
