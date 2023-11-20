using SFML.Graphics;
using System.Collections.Generic;

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
        public void AddBarrier(int x, int y, int height, int width)
        {
            barrier.Add(new FloatRect(x, y, height, width));
        }
    }
}
