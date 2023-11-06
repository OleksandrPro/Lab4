using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public abstract class Level
    {
        public Player player;
        public List<Platform> platforms;        
        public List<SFML.Graphics.FloatRect> barrier;
        public void AddPlatform(int x, int y, int height, int width)
        {
            platforms.Add(new Platform(x, y, height, width));
        }
    }
}
