namespace Lab4
{
    public class MovingLeft : IPlayerState
    {
        private int _movementCoeffcientX = -1;
        public int MovementCoeffcientX { get { return _movementCoeffcientX; } }
        private readonly PlayerStateMachine _playerStateMachine;
        public MovingLeft(PlayerStateMachine psm)
        {
            _playerStateMachine = psm;
        }
        public void Move()
        {
            _playerStateMachine.player.X += Model.HORIZONTAL_UNIT_SIZE * _movementCoeffcientX;
        }
        public void BackToIdle()
        {
            _playerStateMachine.EnterIn<IdleLeft>();
        }
    }
}
