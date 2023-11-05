using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal class IdleRight : PlayerState
    {
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
            _playerStateMachine.EnterIn<MovingRight>();
        }
        public override void BackToMoving()
        {
            _playerStateMachine.EnterIn<MovingRight>();
        }
        public override void BackToIdle()
        {

        }
    }
}
