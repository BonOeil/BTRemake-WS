// <copyright file="GameApiController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class GameApiController : ControllerBase
    {
        public GameApiController(ILogger<GameApiController> logger)
        {
            Logger = logger;
        }

        private ILogger<GameApiController> Logger { get; set; }

        [HttpGet("Step")]
        public async Task<ActionResult<string>> Step()
        {
            Logger.LogTrace($"{nameof(Step)}");

            return new ActionResult<string>("Step");
        }
    }
}
