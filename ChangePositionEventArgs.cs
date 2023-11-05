using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class ChangePositionEventArgs : EventArgs
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public ChangePositionEventArgs(int x, int y) 
        {
            X = x;
            Y = y;
        }
    }
}
