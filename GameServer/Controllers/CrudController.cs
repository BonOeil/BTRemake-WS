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

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<T>>> GetById(Guid id)
        {
            return Ok(await Repository.GetByIdAsync(id));
        }

        [HttpPost]
        public virtual async Task<ActionResult> Add(T itemToAdd)
        {
            await Repository.AddAsync(itemToAdd);

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid idToDelete)
        {
            await Repository.DeleteAsync(idToDelete);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update(T itemToUpdate)
        {
            await Repository.UpdateAsync(itemToUpdate);

            return Ok();
        }
    }
}
