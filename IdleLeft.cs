using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal class IdleLeft : PlayerState
    {
        private readonly PlayerStateMachine _playerStateMachine;
        public IdleLeft(PlayerStateMachine psm)
        {
            _playerStateMachine = psm;
        }
        public override void Enter()
        {
            Console.WriteLine("player is in IdleLeft state");
        }
        public override void Exit()
        {
            Console.WriteLine("player exits IdleLeft state");
        }
        public override void BackToMoving()
        {
            _playerStateMachine.EnterIn<MovingLeft>();
        }
        public override void BackToIdle()
        {

        }
        public override void GoToOppositeMovingDirection()
        {
            _playerStateMachine.EnterIn<MovingRight>();
        }
    }
}
