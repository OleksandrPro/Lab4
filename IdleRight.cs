using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal class IdleRight : PlayerState
    {
        private int _movementCoeffcientX = 0;
        public override int MovementCoeffcientX { get { return _movementCoeffcientX; } }
        private readonly PlayerStateMachine _playerStateMachine;
        public IdleRight(PlayerStateMachine psm) 
        {
            _playerStateMachine = psm;
        }
        public override void Enter()
        {
            Console.WriteLine("player is in IdleRight state");
        }
        public override void Exit()
        {
            Console.WriteLine("player exits IdleRight state");
        }
        public override void Move()
        {
            _playerStateMachine.player.X += Model.HORIZONTAL_UNIT_SIZE * _movementCoeffcientX;
        }
        public override void BackToMoving()
        {
            _playerStateMachine.EnterIn<MovingRight>();
        }
        public override void BackToIdle()
        {

        }
        public override void GoToOppositeMovingDirection()
        {
            _playerStateMachine.EnterIn<MovingLeft>();
        }
    }
}
