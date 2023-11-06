using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public abstract class PlayerState
    {
        public abstract void Enter();
        public abstract void Exit();
        public abstract void BackToMoving();
        public abstract void BackToIdle();
        public abstract void GoToOppositeMovingDirection();
    }
}
