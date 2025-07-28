// <copyright file="MissionPlanController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using System.Threading.Tasks;
    using GameShared.Game;
    using GameShared.Game.Mission;
    using GameShared.Persistence;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class MissionPlanController : CrudController<MissionPlan>
    {
        public MissionPlanController(ILogger<MissionPlanController> logger, IRepository<MissionPlan> repository, IRepository<MapUnit> mapUnitRepository)
            : base(logger, repository)
        {
            MapUnitRepository = mapUnitRepository;
        }

        private IRepository<MapUnit> MapUnitRepository { get; set; }

        public override async Task<ActionResult<MissionPlan>> Add(MissionPlan itemToAdd)
        {
            var units = await MapUnitRepository.GetAllAsync();

            itemToAdd.UnitIds = units.Select(x => x.Id).ToList();

            var fromPosition = units.First().Position;
            itemToAdd.Target = new GPSPosition(fromPosition.Latitude, fromPosition.Longitude + 30);

            return await base.Add(itemToAdd);
        }
    }
}
