using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShared.Game;

namespace GameShared.Services
{
    public interface ITurnServices
    {
        Task<TurnData> StepTurn();
    }
}
