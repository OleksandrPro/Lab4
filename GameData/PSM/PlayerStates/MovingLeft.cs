namespace Lab4
{
    public class MovingLeft : IPlayerState, IMovable
    {
        private int _movementCoeffcientX = -1;
        public int MovementCoeffcientX { get { return _movementCoeffcientX; } }
        private readonly PlayerStateMachine _playerStateMachine;
        public MovingLeft(PlayerStateMachine psm)
        {
            _playerStateMachine = psm;
        }
        public void MoveHorizontal()
        {
            Move(Model.HORIZONTAL_UNIT_SIZE * _movementCoeffcientX, 0);
        }
        public void BackToIdle()
        {
            _playerStateMachine.EnterIn<IdleLeft>();
        }
        public void Move(int x, int y)
        {
            //            (_playerStateMachine.player as IMovable)?.Move(x, y);
            IMovable player = _playerStateMachine.player as IMovable;
            player.Move(x, y);
        }
    }
}
