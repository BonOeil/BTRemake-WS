// <copyright file="CrudController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Persistence;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public abstract class CrudController<T>
        where T : BaseEntity
    {
        public CrudController(ILogger<CrudController<T>> logger, IRepository<T> repository)
        {
            Logger = logger;
            Repository = repository;
        }

        private ILogger<CrudController<T>> Logger { get; set; }

        private IRepository<T> Repository { get; set; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            return new ActionResult<IEnumerable<T>>(await Repository.GetAllAsync());
        }
    }
}
