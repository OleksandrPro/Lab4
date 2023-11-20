namespace Lab4
{
    internal class IdleRight : IPlayerState, IMovable
    {
        private int _movementCoeffcientX = 0;
        public int MovementCoeffcientX { get { return _movementCoeffcientX; } }
        private readonly PlayerStateMachine _playerStateMachine;
        public IdleRight(PlayerStateMachine psm) 
        {
            _playerStateMachine = psm;
        }
        public void MoveHorizontal()
        {
            Move(Model.HORIZONTAL_UNIT_SIZE * _movementCoeffcientX, 0);
        }
        public void BackToIdle()
        {

        }
        public void Move(int x, int y)
        {
            //            (_playerStateMachine.player as IMovable)?.Move(x, y);
            IMovable player = _playerStateMachine.player as IMovable;
            player.Move(x, y);
        }
    }
}
