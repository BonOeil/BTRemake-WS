// <copyright file="SwaggerModule.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.ServerModules
{
    public class SwaggerModule : IServerModule
    {
        public void PostBuild(WebApplication app)
        {
            // if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        public void PreBuild(WebApplicationBuilder builder)
        {
            // if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSwaggerGen();
            }
        }
    }
}
