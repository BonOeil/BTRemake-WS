// <copyright file="GameApiController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameServer.Hubs;
    using GameShared.Messages;
    using GameShared.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Route("api/[controller]")]
    [ApiController]
    public class GameApiController : ControllerBase
    {
        public GameApiController(ILogger<GameApiController> logger, IGameServices gameServices, IHubContext<GameHub> hubContext)
        {
            Logger = logger;
            GameServices = gameServices;
            GameHubContext = hubContext;
        }

        private IHubContext<GameHub> GameHubContext { get; }

        private IGameServices GameServices { get; }

        private ILogger<GameApiController> Logger { get; set; }

        [HttpPost("Step")]
        public async Task<ActionResult> Step()
        {
            Logger.LogTrace($"{nameof(Step)}");

            await GameHubContext.Clients.All.SendAsync(nameof(FullGameState), new FullGameState());

            return Ok();
        }

        [HttpPost("LoadScenario/{scenarioName}")]
        public async Task<ActionResult> LoadScenario(string scenarioName)
        {
            await GameServices.StartScenario(scenarioName, scenarioName);

            await GameHubContext.Clients.All.SendAsync(nameof(ScenarioLoaded), new ScenarioLoaded());

            Logger.LogInformation("Scenario loaded: {ScenarioName})", scenarioName);

            return Ok();
        }
    }
}
