// <copyright file="TurnServices.cs" company="BTRemake">
// Copyright (c) BTRemake. All rights reserved.
// </copyright>

namespace GameShared.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GameShared.Game;
    using GameShared.Game.Entities;
    using GameShared.Persistance;
    using Microsoft.Extensions.Logging;

    public class TurnServices : ITurnServices
    {
        private IRepository<TurnData> TurnRepository { get; }

        private ILogger<GameManagement> Logger { get; }

        public TurnServices(IRepository<TurnData> repository, ILogger<GameManagement> logger)
        {
            Logger = logger;
            TurnRepository = repository;
        }

        public async Task<TurnData> StepTurn()
        {
            var turnData = await TurnRepository.GetUniqueAsync();

            GamePhase currentPhase = turnData.CurrentPhase;
            GamePhase nextPhase;

            // Déterminer la phase suivante
            switch (currentPhase)
            {
                case GamePhase.Planning:
                    nextPhase = GamePhase.Movement;
                    // ProcessMovementPhase();
                    break;
                case GamePhase.Movement:
                    nextPhase = GamePhase.Combat;
                    // ProcessCombatPhase();
                    break;
                case GamePhase.Combat:
                    nextPhase = GamePhase.Resolution;
                    // ProcessEndTurnPhase();
                    break;
                case GamePhase.Resolution:
                    nextPhase = GamePhase.Planning;
                    // StartNewTurn();
                    break;
                default:
                    nextPhase = GamePhase.Planning;
                    break;
            }

            // Mettre à jour la phase actuelle
            turnData.CurrentPhase = nextPhase;

            await TurnRepository.UpdateAsync(turnData);

            Logger.LogInformation($"Game phase advanced from {currentPhase} to {nextPhase}");

            // Déclencher l'événement de changement de phase
            // OnPhaseChanged?.Invoke(nextPhase);*/

            return turnData;
        }

        public async Task<TurnData> GetTurn()
        {
            return await TurnRepository.GetUniqueAsync();
        }
    }
}
