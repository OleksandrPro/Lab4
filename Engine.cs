using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Engine
    {
        public Engine() { }

        public abstract class Collider 
        {
            protected struct Point
            {
                public int x;
                public int y;
                public Point(int a, int b)
                {
                    x = a;
                    y = b;
                }
            }
        }
        public class BoxCollider : Collider
        {
            Collider.Point upperLeft { get; set; }
            Collider.Point bottomRight { get; set; }
            public BoxCollider(int x, int y, int width, int height)
            {
                upperLeft = new Collider.Point(x, y);
                bottomRight = new Collider.Point(x + width, y + height);
            }
            private void updatePointPos(Point p, int x, int y) 
            {
                p.x = x;
                p.y = y;
            }
            public void updateColiderPos(object sender, EventArgs e)
            {
                ChangePositionEventArgs changepos = (ChangePositionEventArgs)e;
                Player player = (Player)sender;
                updatePointPos(upperLeft, changepos.X, changepos.Y);
                updatePointPos(bottomRight, changepos.X + player.Width, changepos.Y + player.Height);
            }
        }
        public class CircleColider : Collider
        {
            Collider.Point center { get; set; }
            int Radius { get; set; }
            public CircleColider(int radius)
            {
                center = new Collider.Point();
                Radius = radius;
            }
        }
        public bool AABB(float x, float y, float w, float h, float X, float Y, float W, float H)
        {
            return x <= X + W && x + h <= X && y <= Y + H && y + h >= Y;
        }
        public static bool isIntersect(FloatRect rect1, FloatRect rect2)
        {
            return rect1.Intersects(rect2);
        }
    }
}
