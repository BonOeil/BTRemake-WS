// <copyright file="FullGameState.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Messages
{
    using GameShared.Game.Mission;

    public class FullGameState
    {
        required public IList<MapUnit> Units { get; set; }
    }
}
