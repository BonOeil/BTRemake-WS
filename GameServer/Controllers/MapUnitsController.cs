// <copyright file="MapUnitsController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Game.Mission;
    using GameShared.Persistence;
    using Microsoft.Extensions.Logging;

    public class MapUnitsController : CrudController<MissionUnit>
    {
        public MapUnitsController(ILogger<MapUnitsController> logger, IRepository<MissionUnit> repository)
            : base(logger, repository)
        {
        }
    }
}
