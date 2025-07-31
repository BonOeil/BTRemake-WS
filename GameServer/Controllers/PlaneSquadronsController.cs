// <copyright file="PlaneSquadronsController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Game.OOB;
    using GameShared.Persistence;
    using Microsoft.Extensions.Logging;

    public class PlaneSquadronsController : CrudController<PlaneSquadron>
    {
        public PlaneSquadronsController(ILogger<PlaneSquadronsController> logger, IRepository<PlaneSquadron> repository)
            : base(logger, repository)
        {
        }
    }
}
