namespace GameShared.Game.Entities
{
    public class TurnData : BaseEntity
    {
        public int CurrentTurn { get; set; } = 0;

        public GamePhase CurrentPhase { get; set; } = GamePhase.Planning;
    }
}
