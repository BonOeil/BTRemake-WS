// <copyright file="PlanetController.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace BTRemakeWebApplication.Pages
{
    using Microsoft.AspNetCore.Mvc;

    public class PlanetController : Controller
    {
        public IActionResult GetPlanetData()
        {
            var planetData = new
            {
                rotation_speed = 0.01f,
                texture_url = "/textures/earth-diffuse.jpg",
                radius = 2.0f,
            };
            return Json(planetData);
        }
    }
}
