using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class FallingObject : GameObject
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public FallingObject(int x, int y)
        {
            X = x;
            Y = y;
        }
        public FallingObject() : this(0, 0) { }
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void IncreaseVerticalSpeed(int speedAmount) 
        {
            Y += speedAmount;
        }
    }
}
