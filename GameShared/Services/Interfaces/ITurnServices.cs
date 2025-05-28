// <copyright file="ITurnServices.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GameShared.Game.Entities;

    public interface ITurnServices
    {
        Task<TurnData> StepTurn();

        Task<TurnData> GetTurn();
    }
}
