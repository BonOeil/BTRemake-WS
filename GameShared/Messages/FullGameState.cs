// <copyright file="FullGameState.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Messages
{
    using GameShared.Game.Mission;
    using GameShared.Game.OOB;

    public class FullGameState
    {
        required public IList<Squadron> Squadrons { get; set; }
    }
}
