﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class View
    {
        public RenderWindow GameWindow { get; private set; }
        private Controller _controller;

        public Sprite CurrentPlayerModel;
        private List<RectangleShape> Platforms;

        private List<Sprite> FallingObjects;

        private LinkedList<Sprite> _currentAnimation;
        private LinkedList<Sprite> _idleRightAnimation;
        private LinkedList<Sprite> _idleLeftAnimation;
        private LinkedList<Sprite> _movingRightAnimation;
        private LinkedList<Sprite> _movingLeftAnimation;
        private Dictionary<Type, LinkedList<Sprite>> _stateAnimationPairs;

        private string _folderPathSingleSprite = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\newPlaceholder.png";
        private string _folderPathFireBallSprite = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\fireball2.png";

        //private string _folderPathIdleRight = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\IdleRight";
        //private string _folderPathIdleLeft = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\IdleLeft";
        //private string _folderPathMovingRight = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\MovingRight";
        //private string _folderPathMovingLeft = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\MovingLeft";

        private string _folderPathIdleRight = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\Luffy\\IdleRight";
        private string _folderPathIdleLeft = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\Luffy\\IdleLeft";
        private string _folderPathMovingRight = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\Luffy\\MovingRight";
        private string _folderPathMovingLeft = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\Luffy\\MovingLeft";

        private const int ANIMATION_TIMER = 30;
        private int _animationTick = ANIMATION_TIMER;

        public View(RenderWindow window)
        {
            GameWindow = window;

            Platforms = new List<RectangleShape>();
            FallingObjects = new List<Sprite>();

            _idleRightAnimation = new LinkedList<Sprite>();
            _idleLeftAnimation = new LinkedList<Sprite>();
            _movingRightAnimation = new LinkedList<Sprite>();
            _movingLeftAnimation = new LinkedList<Sprite>();

            FillAnimationList(_idleRightAnimation, _folderPathIdleRight);
            FillAnimationList(_idleLeftAnimation, _folderPathIdleLeft);
            FillAnimationList(_movingRightAnimation, _folderPathMovingRight);
            FillAnimationList(_movingLeftAnimation, _folderPathMovingLeft);

            _stateAnimationPairs = new Dictionary<Type, LinkedList<Sprite>>()
            {
                [typeof(IdleRight)] = _idleRightAnimation,
                [typeof(IdleLeft)] = _idleLeftAnimation,
                [typeof(MovingRight)] = _movingRightAnimation,
                [typeof(MovingLeft)] = _movingLeftAnimation
            };
        }
        public void AddController(Controller controller)
        {
            _controller = controller;
        }
        public void DrawScene()
        {
            GameWindow.Clear(new Color(Color.Black));

            GameWindow.Draw(CurrentPlayerModel);
            foreach (var p in Platforms)
            {
                GameWindow.Draw(p);
            }
            foreach (var g in FallingObjects)
            {
                GameWindow.Draw(g);
            }
            PlayPlayerAnimation();
            GameWindow.Display();
        }
        public void LoadLevel(Level level) 
        {
            AddPlatforms(level);
            AddPlayerModel(level.player);
            SetBarrier(level);
        }
        private void AddPlatforms(Level level)
        {
            foreach (var p in level.platforms)
            {
                RectangleShape newPlatform = new RectangleShape(new Vector2f(p.Height, p.Width));
                newPlatform.Position = new Vector2f(p.X, p.Y);
                newPlatform.FillColor = Color.Cyan;
                Platforms.Add(newPlatform);
                _controller.AddPlatformCollider(p, newPlatform.GetGlobalBounds());
            }
        }
        private void AddPlayerModel(Player player)
        {
            _stateAnimationPairs.TryGetValue(player.CurrentState.GetType(), out _currentAnimation);
            CurrentPlayerModel = _currentAnimation.GetCurrent();
            CurrentPlayerModel.Position = new Vector2f(player.X, player.Y);
            _controller.AddPlayerCollider();
        }
        public void PlayPlayerAnimation()
        {
            --_animationTick;
            Vector2f currentPos = CurrentPlayerModel.Position;
            if (_animationTick==0)
            {
                CurrentPlayerModel = _currentAnimation.GetNext();
                _animationTick = ANIMATION_TIMER;
            }
            
            CurrentPlayerModel.Position = currentPos;
        }
        public void UpdateAnimation(Player p)
        {
            Vector2f currentPos = CurrentPlayerModel.Position;
            _currentAnimation.Reset();
            _stateAnimationPairs.TryGetValue(p.CurrentState.GetType(), out _currentAnimation);
            CurrentPlayerModel = _currentAnimation.GetCurrent();
            CurrentPlayerModel.Position = currentPos;
        }
        private void FillAnimationList(LinkedList<Sprite> list, string path)
        {
            if (Directory.Exists(path))
            {
                string[] imageFiles = Directory.GetFiles(path, "*.png");
                Array.Sort(imageFiles);
                foreach (string imagePath in imageFiles)
                {
                    Texture texture = new Texture(imagePath);
                    Sprite newSprite = new Sprite(texture);
                    list.Add(newSprite);
                }
            }
            else
            {
                Console.WriteLine("Folder doesn't exist");
            }
        }
        private void SetBarrier(Level level)
        {
            int barrierSize = 300;
            int windowX = (int)GameWindow.Size.X;
            int windowY = (int)GameWindow.Size.Y;
            _controller.AddBarrier(0, -barrierSize, windowX, barrierSize);
            _controller.AddBarrier(0, windowY, windowX, barrierSize);
            _controller.AddBarrier(-barrierSize, 0, barrierSize, windowY);
            _controller.AddBarrier(windowX, 0, barrierSize, windowY);
            //xywh
        }
        public void AddFallingObject(FallingObject fObj)
        {
            Texture model = new Texture(_folderPathFireBallSprite);
            Sprite newFObjSprite = new Sprite(model);
            newFObjSprite.Position = new Vector2f(fObj.X, fObj.Y);
            FallingObjects.Add(newFObjSprite);
            _controller.AddFallingObjectCollider(fObj, newFObjSprite.GetGlobalBounds());
        }
        public void RemoveFallingObjectSprite(int x, int y)
        {
            Sprite toRemove = FallingObjects.FirstOrDefault(sprite => sprite.Position.X == x && sprite.Position.Y == y);
            FallingObjects.Remove(toRemove);
        }
        public void UpdateModelPosition(int x, int y)
        {
            CurrentPlayerModel.Position = new Vector2f(x, y);
        }
        public void UpdateFallingObjectPosition(int y)
        {
            Sprite s = null;
            foreach (var item in FallingObjects)
            {
                item.Position = new Vector2f(item.Position.X, item.Position.Y + y);
                if (item.Position.Y >= GameWindow.Size.Y)
                {
                    s = item;
                }
            }
            if (s != null)
            {
                FallingObjects.Remove(s);
            }
        }
    }
}
