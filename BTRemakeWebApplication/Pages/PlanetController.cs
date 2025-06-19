using Microsoft.AspNetCore.Mvc;

namespace BTRemakeWebApplication.Pages
{
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
