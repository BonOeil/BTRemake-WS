// <copyright file="CrudController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameServer.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public abstract class CrudController<T>
    {
        public CrudController(ILogger<CrudController<T>> logger)
        {
            Logger = logger;
        }

        public ILogger<CrudController<T>> Logger { get; set; }

        [HttpGet]
        public ActionResult<T> GetAll()
        {
            return new ActionResult<T>(default(T));
        }
    }
}
