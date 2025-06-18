// <copyright file="LocationController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Game.Entities;
    using GameShared.Persistence;
    using Microsoft.Extensions.Logging;

    public class LocationController : CrudController<Location>
    {
        public LocationController(ILogger<LocationController> logger, IRepository<Location> repository)
            : base(logger, repository)
        {
        }
    }
}
