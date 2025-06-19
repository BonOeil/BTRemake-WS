fetch('/Planet/GetPlanetData')
    .then(response => response.json())
    .then(data => {
        // Utiliser les données pour configurer la planète
        planet.rotation.y += data.rotation_speed;
    });