using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShared.Game;
using GameShared.Persistance;

namespace GameShared.Services
{
    public class TurnServices : ITurnServices
    {
        private IRepository<TurnData> TurnRepository { get; }

        public TurnServices(IRepository<TurnData> repository)
        {
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
                    //ProcessMovementPhase();
                    break;
                case GamePhase.Movement:
                    nextPhase = GamePhase.Combat;
                    //ProcessCombatPhase();
                    break;
                case GamePhase.Combat:
                    nextPhase = GamePhase.Resolution;
                    //ProcessEndTurnPhase();
                    break;
                case GamePhase.Resolution:
                    nextPhase = GamePhase.Planning;
                    //StartNewTurn();
                    break;
                default:
                    nextPhase = GamePhase.Planning;
                    break;
            }

            // Mettre à jour la phase actuelle
            turnData.CurrentPhase = nextPhase;

            await TurnRepository.UpdateAsync(turnData);

            Debug.WriteLine($"Game phase advanced from {currentPhase} to {nextPhase}");

            // Déclencher l'événement de changement de phase
            //OnPhaseChanged?.Invoke(nextPhase);*/

            return turnData;
        }
    }
}
