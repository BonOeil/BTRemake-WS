// <copyright file="TurnData.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Game.Entities
{
    using GameShared.Persistance;

    public class TurnData : BaseEntity
    {
        public int CurrentTurn { get; set; } = 0;

        public GamePhase CurrentPhase { get; set; } = GamePhase.Planning;
    }
}
