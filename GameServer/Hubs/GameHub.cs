// <copyright file="GameHub.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Hubs
{
    using GameShared.Messages;
    using Microsoft.AspNetCore.SignalR;

    public class GameHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public GameHub(ILogger<GameHub> logger)
        {
            Logger = logger;
        }

        private ILogger<GameHub> Logger { get; }
    }
}
