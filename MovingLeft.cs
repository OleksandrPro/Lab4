using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class MovingLeft : PlayerState
    {
        private readonly PlayerStateMachine _playerStateMachine;
        public MovingLeft(PlayerStateMachine psm)
        {
            _playerStateMachine = psm;
            Enter();
        }
        public override void Enter()
        {
            Console.WriteLine("player is in MovingLeft state");
        }

        public override void Exit()
        {
            Console.WriteLine("player exits MovingLeft state");
            _playerStateMachine.EnterIn<IdleLeft>();
        }
        public override void Move()
        {

        }
        public override void BackToMoving()
        {

        }
        public override void BackToIdle()
        {
            _playerStateMachine.EnterIn<IdleLeft>();
        }
    }
}
