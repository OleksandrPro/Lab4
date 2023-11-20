using SFML.Graphics;
using System.Collections.Generic;
using System;

namespace Lab4
{
    public interface IView
    {
        RenderWindow GameWindow { get; }
        UI UI { get; }
        Sprite CurrentPlayerModel { get; }
        List<Sprite> Platforms { get; }
        List<Sprite> FallingDamageObjects { get; }
        List<Sprite> FallingScoreObjects { get; }

        CycledLinkedList<Sprite> CurrentAnimation { get; }
        CycledLinkedList<Sprite> IdleRightAnimation { get; }
        CycledLinkedList<Sprite> IdleLeftAnimation { get; }
        CycledLinkedList<Sprite> MovingRightAnimation { get; }
        CycledLinkedList<Sprite> MovingLeftAnimation { get; }
        Dictionary<Type, CycledLinkedList<Sprite>> StateAnimationPairs { get; }
        void AddController(Controller controller);
        void DrawScene();
        void LoadLevel(ILevel level);
        void UpdateAnimation(Player p);
        void UpdateFallingObjectPosition(int y);
        void SetEndGameScreen();
    }
}
