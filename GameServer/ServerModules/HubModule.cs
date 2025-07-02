// <copyright file="HubModule.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.ServerModules
{
    using GameServer.Hubs;

    public class HubModule : IServerModule
    {
        public void PostBuild(WebApplication app)
        {
            app.MapHub<TestHub>($"/{nameof(TestHub)}");
            app.MapHub<GameHub>($"/{nameof(GameHub)}");
        }

        public void PreBuild(WebApplicationBuilder builder)
        {
            builder.Services.AddSignalR();
        }
    }
}
