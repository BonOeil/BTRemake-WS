// <copyright file="CrudController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using GameShared.Persistence;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public abstract class CrudController<T> : ControllerBase
        where T : BaseEntity
    {
        protected CrudController(ILogger<CrudController<T>> logger, IRepository<T> repository)
        {
            Logger = logger;
            Repository = repository;
        }

        private ILogger<CrudController<T>> Logger { get; set; }

        private IRepository<T> Repository { get; set; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            return Ok(await Repository.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Add(T itemToAdd)
        {
            await Repository.AddAsync(itemToAdd);

            return Ok();
        }
    }
}
