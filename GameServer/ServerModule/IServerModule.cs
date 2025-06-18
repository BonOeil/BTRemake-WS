// <copyright file="IServerModule.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.ServerModule
{
    public interface IServerModule
    {
        void PreBuild(WebApplicationBuilder builder);

        void PostBuild(WebApplication app);
    }
}
