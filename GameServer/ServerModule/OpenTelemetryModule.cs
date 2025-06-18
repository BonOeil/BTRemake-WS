// <copyright file="OpenTelemetryModule.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.ServerModule
{
    public class OpenTelemetryModule : IServerModule
    {
        public void PostBuild(WebApplication app)
        {
            // Void
        }

        public void PreBuild(WebApplicationBuilder builder)
        {
            builder.ConfigureOpenTelemetry();
        }
    }
}
