// <copyright file="PilotsController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Game.OOB;
    using GameShared.Persistence;
    using Microsoft.Extensions.Logging;

    public class PilotsController : CrudController<Pilot>
    {
        public PilotsController(ILogger<PilotsController> logger, IRepository<Pilot> repository)
            : base(logger, repository)
        {
        }
    }
}
