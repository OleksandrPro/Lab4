using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public abstract class PlayerState
    {
        public abstract int MovementCoeffcientX { get; }
        
        public abstract void Move();
        public abstract void BackToIdle();
    }
}
