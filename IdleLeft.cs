namespace Lab4
{
    public class IdleLeft : IPlayerState
    {
        private int _movementCoeffcientX = 0;
        public int MovementCoeffcientX { get { return _movementCoeffcientX; } }
        private readonly PlayerStateMachine _playerStateMachine;
        public IdleLeft(PlayerStateMachine psm)
        {
            _playerStateMachine = psm;
        }
        public void Move()
        {
            _playerStateMachine.player.X += Model.HORIZONTAL_UNIT_SIZE * _movementCoeffcientX;
        }
        public void BackToIdle()
        {

        }
    }
}
