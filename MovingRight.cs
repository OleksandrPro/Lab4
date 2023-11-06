using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal class MovingRight : PlayerState
    {
        private readonly PlayerStateMachine _playerStateMachine;
        public MovingRight(PlayerStateMachine psm)
        {
            _playerStateMachine = psm;
        }
        public override void Enter()
        {
            Console.WriteLine("player is in MovingRight state");
        }
        public override void Exit()
        {
            Console.WriteLine("player exits MovingRight state");
        }
        public override void BackToMoving()
        {

        }
        public override void BackToIdle()
        {
            _playerStateMachine.EnterIn<IdleRight>();
        }
        public override void GoToOppositeMovingDirection()
        {
            _playerStateMachine.EnterIn<MovingLeft>();
        }
    }
}
