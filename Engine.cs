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
        public static bool isIntersect(FloatRect rect1, FloatRect rect2)
        {
            return rect1.Intersects(rect2);
        }
        public static void InitCollider(FloatRect colider, float x, float y, float height, float width)
        {
            colider.Left = x; 
            colider.Top = y;
            colider.Width = width;
            colider.Height = height;
        }
        public static FloatRect ChangeColliderPosition(FloatRect colider, float x, float y)
        {
            colider.Left = x;
            colider.Top = y;
            return colider;
        }
        public static FloatRect ChangeColliderPositionX(FloatRect colider, float x)
        {
            colider.Left = x;
            return colider;
        }
        public static FloatRect ChangeColliderPositionY(FloatRect colider, float y)
        {
            colider.Top = y;
            return colider;
        }
    }
}
