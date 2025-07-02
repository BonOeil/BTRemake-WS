// <copyright file="ControllerSupportModule.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.ServerModules
{
    public class ControllerSupportModule : IServerModule
    {
        public void PostBuild(WebApplication app)
        {
            app.MapControllers();
        }

        public void PreBuild(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
        }
    }
}
