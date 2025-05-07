using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShared.Game
{
    public class World
    {
        public List<Location> Locations { get; set; } = new List<Location>();

        public World()
        {
            // Exemple d'initialisation de quelques localités
            AddLocation("London", new GPSPosition(51.5074f, -0.1278f), Faction.Allies);
            AddLocation("Berlin", new GPSPosition(52.5200f, 13.4050f), Faction.Axis);
        }

        public void AddLocation(string name, GPSPosition position, Faction controllingFaction)
        {
            string id = Guid.NewGuid().ToString();
            Location location = new Location(id, name, position, controllingFaction);
            Locations.Add(location);

            Debug.Write($"Added location {name} (ID: {id}) at {position}");
        }
    }
}
