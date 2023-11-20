using SFML.Graphics;
using System.Collections.Generic;

namespace Lab4
{
    public interface ILevel
    {
        Player Player { get; }
        List<Platform> Platforms { get; }
        List<SFML.Graphics.FloatRect> Barrier { get; }
        void AddPlatform(int x, int y, int height, int width);
        void AddBarrier(int x, int y, int height, int width);
    }
}
