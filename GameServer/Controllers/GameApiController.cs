// <copyright file="GameApiController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Messages;
    using GameShared.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class GameApiController : ControllerBase
    {
        public GameApiController(ILogger<GameApiController> logger, IGameServices gameServices)
        {
            Logger = logger;
            GameServices = gameServices;
        }

        private IGameServices GameServices { get; }

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

            // Informer les autres joueurs de la connexion
            // await Clients.All.SendAsync(nameof(ScenarioLoaded), new ScenarioLoaded());

            // Logger.LogInformation($"Scenario loaded: {scenarioName} ({Context.ConnectionId})");
            Logger.LogInformation("Scenario loaded: {ScenarioName}", scenarioName);

            return new ActionResult<string>("Step");
        }
    }
}
