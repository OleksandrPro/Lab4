namespace Lab4
{
    public class MovingLeft : PlayerState
    {
        private int _movementCoeffcientX = -1;
        public override int MovementCoeffcientX { get { return _movementCoeffcientX; } }
        private readonly PlayerStateMachine _playerStateMachine;
        public MovingLeft(PlayerStateMachine psm)
        {
            _playerStateMachine = psm;
        }
        public override void Move()
        {
            _playerStateMachine.player.X += Model.HORIZONTAL_UNIT_SIZE * _movementCoeffcientX;
        }
        public override void BackToIdle()
        {
            _playerStateMachine.EnterIn<IdleLeft>();
        }
    }
}
