// <copyright file="GameApiController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameServer.Hubs;
    using GameShared.Messages;
    using GameShared.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class GameApiController : ControllerBase
    {
        public GameApiController(ILogger<GameApiController> logger, IGameServices gameServices, GameHub gameHub)
        {
            Logger = logger;
            GameServices = gameServices;
            GameHub = gameHub;
        }

        private IGameServices GameServices { get; }

        private GameHub GameHub { get; }

        private ILogger<GameApiController> Logger { get; set; }

        [HttpGet("Step")]
        public async Task<ActionResult<string>> Step()
        {
            Logger.LogTrace($"{nameof(Step)}");

            return new ActionResult<string>("Step");
        }

        [HttpGet("LoadScenario/{scenarioName}")]
        public async Task<ActionResult<string>> LoadScenario(string scenarioName)
        {
            await GameServices.StartScenario(scenarioName, scenarioName);

            _ = GameHub.LoadScenario(new LoadScenario()
            {
                ScenarioName = scenarioName,
                InstanceName = scenarioName,
            });

            Logger.LogInformation("Scenario loaded: {ScenarioName} ({ConnectionId})", scenarioName, GameHub.Context.ConnectionId);

            return new ActionResult<string>("Step");
        }
    }
}
