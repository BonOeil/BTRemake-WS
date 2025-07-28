// <copyright file="PlanesController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Game.Entities;
    using GameShared.Persistence;
    using Microsoft.Extensions.Logging;

    public class PlanesController : CrudController<Plane>
    {
        public PlanesController(ILogger<PlanesController> logger, IRepository<Plane> repository)
            : base(logger, repository)
        {
        }
    }
}
