// <copyright file="PilotService.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services
{
    using System;
    using GameShared.Game.OOB;
    using GameShared.Persistence;
    using GameShared.Services.Interfaces;

    public class PilotService : IPilotService
    {
        public PilotService(IRepositoryFactory repositoryFactory, IPaceService paceService)
        {
            Repository = repositoryFactory.Get<Pilot>();
            PaceService = paceService;

            FatiguePerPace = PaceService.ToPace(FatiguePerFlightHours, Pace.Hours);
            RestPerPace = PaceService.ToPace(RestPerNonFlightHours, Pace.Hours);
            ExperiencePerPace = PaceService.ToPace(ExperiencePerFlightHours, Pace.Hours);
        }

        private double FatiguePerFlightHours { get; } = 8.0; // %. Pilots should be full tired after a 18h flight (18?).

        private double FatiguePerPace { get; }

        private double ExperiencePerFlightHours { get; } = 1; // %. Pilots should be experienced after 100h of flight (100?).

        private double ExperiencePerPace { get; }

        private double RestPerNonFlightHours { get; } = 12.5; // %. Pilots should be fully rested after a 8h rest time (8?).

        private double RestPerPace { get; }

        private IRepository<Pilot> Repository { get; }

        private IPaceService PaceService { get; }

        private Random Rand { get; } = new Random(DateTime.UtcNow.Second);

        public void AddFatigue(Pilot pilot)
        {
            pilot.Stamina -= FatiguePerPace;
            pilot.Experience -= ExperiencePerPace;

            Repository.UpdateAsync(pilot).Wait();
        }

        public void AddRest(Pilot pilot)
        {
            pilot.Stamina += RestPerPace;

            Repository.UpdateAsync(pilot).Wait();
        }

        public void CrashPlane(Pilot pilot)
        {
            // New pilot status
            // depending on effectivness test, the pilot is alive or dead
            if (!GetEffectivnessTest(pilot))
            {
                pilot.Status = PilotStatus.Kia;
            }
            else // Pilot alive
            {
                // Pilot may be :
                // Active = able to come back to service. But an injured pilot can't become active. So current status is taken.
                // Injured = able come come back, but non combat status
                // Mia = May be dead, prisonner
                var status = new PilotStatus[] { pilot.Status, PilotStatus.Injured, PilotStatus.Mia };
                pilot.Status = status[Rand.Next(status.Length)];
            }

            pilot.Stamina = 0;
            pilot.Morale = 0;

            Repository.UpdateAsync(pilot).Wait();
        }

        public Pilot Get(Guid pilotId)
        {
            return Repository.GetByIdAsync(pilotId).Result;
        }

        /// <summary>
        /// Gets the effectivness test. To succeed, a random number should be lower than effectivness value.
        /// If pilot is an Ace, he got an advantage (2 check, the best is taken).
        /// </summary>
        /// <param name="pilot">The pilot.</param>
        /// <returns>True is test succeeded.</returns>
        private bool GetEffectivnessTest(Pilot pilot)
        {
            var test = pilot.IsAce ? Rand.NextDouble() : new double[] { Rand.NextDouble(), Rand.NextDouble() }.Min();
            test *= 100;

            return test <= pilot.Effectivness;
        }
    }
}
