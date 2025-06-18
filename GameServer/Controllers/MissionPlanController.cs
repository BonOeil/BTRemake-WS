// <copyright file="MissionPlanController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Game.Mission;
    using GameShared.Persistence;
    using Microsoft.Extensions.Logging;

    public class MissionPlanController : CrudController<MissionPlan>
    {
        public MissionPlanController(ILogger<MissionPlanController> logger, IRepository<MissionPlan> repository)
            : base(logger, repository)
        {
        }
    }
}
