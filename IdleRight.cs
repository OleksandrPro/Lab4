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
        public override void Move()
        {
            _playerStateMachine.player.X += Model.HORIZONTAL_UNIT_SIZE * _movementCoeffcientX;
        }
        public override void BackToIdle()
        {

        }
    }
}
