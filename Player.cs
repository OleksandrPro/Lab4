using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Player
    {
        private int _x;
        private int _y;
        public int Height { get; set; }
        public int Width { get; set; }
        public FloatRect Collider { get; set; }
        public int X 
        {  
            get
            {
                return _x;
            }
            set
            {
                if (value != _x)
                {
                    _x = value;
                    NewPosition?.Invoke(this, new ChangePositionEventArgs(_x, _y));
                }
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value != _y)
                {
                    _y = value;
                    NewPosition?.Invoke(this, new ChangePositionEventArgs(_x, _y));
                }
            }
        }
        public delegate void PositionChanged(object sender, EventArgs e);
        public event PositionChanged NewPosition;
        public Player(int x, int y, int width, int height) 
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }
    }
}
