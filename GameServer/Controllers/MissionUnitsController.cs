// <copyright file="MissionUnitsController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Game.Mission;
    using GameShared.Persistence;
    using Microsoft.Extensions.Logging;

    public class MissionUnitsController : CrudController<MissionUnit>
    {
        public MissionUnitsController(ILogger<MissionUnitsController> logger, IRepository<MissionUnit> repository)
            : base(logger, repository)
        {
        }
    }
}
