using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Lab4.Model;

namespace Lab4
{
    public class View : IPositionChangeObserver, ISpawnNewObjectObserver, IDespawnObjectObserver
    {
        public RenderWindow GameWindow { get; private set; }
        private IControllerView _controller;

        public Sprite CurrentPlayerModel;
        private List<Sprite> Platforms;

        private List<Sprite> FallingDamageObjects;
        private List<Sprite> FallingScoreObjects;

        private CycledLinkedList<Sprite> _currentAnimation;
        private CycledLinkedList<Sprite> _idleRightAnimation;
        private CycledLinkedList<Sprite> _idleLeftAnimation;
        private CycledLinkedList<Sprite> _movingRightAnimation;
        private CycledLinkedList<Sprite> _movingLeftAnimation;
        private Dictionary<Type, CycledLinkedList<Sprite>> _stateAnimationPairs;
        private UI _UI;
        public UI UI { get { return _UI; } }

        private const string _folderPathFireBallSprite = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\fireball2.png";
        private const string _folderPathFloorSprite = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\floor.png";
        private const string _folderPathMeatSprite = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\meat.png";

        private const string _folderPathIdleRight = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\Luffy\\IdleRight";
        private const string _folderPathIdleLeft = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\Luffy\\IdleLeft";
        private const string _folderPathMovingRight = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\Luffy\\MovingRight";
        private const string _folderPathMovingLeft = "D:\\[FILES]\\[УНИВЕР]\\2 курс\\1 семестр\\C#\\ЛР\\ЛР 4\\Lab4\\sprites\\Luffy\\MovingLeft";

        private const int ANIMATION_TIMER = 30;
        private int _animationTick = ANIMATION_TIMER;

        public View(RenderWindow window)
        {
            GameWindow = window;

            Platforms = new List<Sprite>();
            FallingDamageObjects = new List<Sprite>();
            FallingScoreObjects = new List<Sprite>();

            _idleRightAnimation = new CycledLinkedList<Sprite>();
            _idleLeftAnimation = new CycledLinkedList<Sprite>();
            _movingRightAnimation = new CycledLinkedList<Sprite>();
            _movingLeftAnimation = new CycledLinkedList<Sprite>();

            FillAnimationList(_idleRightAnimation, _folderPathIdleRight);
            FillAnimationList(_idleLeftAnimation, _folderPathIdleLeft);
            FillAnimationList(_movingRightAnimation, _folderPathMovingRight);
            FillAnimationList(_movingLeftAnimation, _folderPathMovingLeft);

            _stateAnimationPairs = new Dictionary<Type, CycledLinkedList<Sprite>>()
            {
                [typeof(IdleRight)] = _idleRightAnimation,
                [typeof(IdleLeft)] = _idleLeftAnimation,
                [typeof(MovingRight)] = _movingRightAnimation,
                [typeof(MovingLeft)] = _movingLeftAnimation
            };
            _UI = new UI(window);
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
            foreach (var g in FallingDamageObjects)
            {
                GameWindow.Draw(g);
            }
            foreach (var m in FallingScoreObjects)
            {
                GameWindow.Draw(m);
            }
            PlayPlayerAnimation();
            _UI.Draw();
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
                Texture model = new Texture(_folderPathFloorSprite);
                Sprite newPlatform = new Sprite(model);
                newPlatform.Position = new Vector2f(p.X, p.Y);
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
        private void FillAnimationList(CycledLinkedList<Sprite> list, string path)
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
        public void UpdateSpawn(Model.FallingObjectTypes type, FallingObject fObj)
        {
            if (type == FallingObjectTypes.DamageObject)
                AddDamageObject(fObj);
            if (type == FallingObjectTypes.ScoreObject)
                AddScoreObject(fObj);
        }
        private void AddDamageObject(FallingObject fObj)
        {
            Texture model = new Texture(_folderPathFireBallSprite);
            Sprite newFObjSprite = new Sprite(model);
            newFObjSprite.Position = new Vector2f(fObj.X, fObj.Y);
            FallingDamageObjects.Add(newFObjSprite);
            _controller.AddFallingObjectCollider(fObj, newFObjSprite.GetGlobalBounds());
        }
        private void AddScoreObject(FallingObject fObj)
        {
            Texture model = new Texture(_folderPathMeatSprite);
            Sprite newFObjSprite = new Sprite(model);
            newFObjSprite.Position = new Vector2f(fObj.X, fObj.Y);
            FallingScoreObjects.Add(newFObjSprite);
            _controller.AddFallingObjectCollider(fObj, newFObjSprite.GetGlobalBounds());
        }
        public void UpdateDespawn(FallingObjectTypes type, FallingObject fobj)
        {
            if (type == FallingObjectTypes.DamageObject)
                RemoveDamageObjectSprite(fobj.X, fobj.Y);
            if (type == FallingObjectTypes.ScoreObject)
                RemoveScoreObjectSprite(fobj.X, fobj.Y);
        }
        private void RemoveDamageObjectSprite(int x, int y)
        {
            Sprite toRemove = FallingDamageObjects.FirstOrDefault(sprite => sprite.Position.X == x && sprite.Position.Y == y);
            FallingDamageObjects.Remove(toRemove);
        }
        private void RemoveScoreObjectSprite(int x, int y)
        {
            Sprite toRemove = FallingScoreObjects.FirstOrDefault(sprite => sprite.Position.X == x && sprite.Position.Y == y);
            FallingScoreObjects.Remove(toRemove);
        }
        public void Update(IPositionChanged subject)
        {
            Player player = subject as Player;
            UpdateModelPosition(player.X, player.Y);
        }
        private void UpdateModelPosition(int x, int y)
        {
            CurrentPlayerModel.Position = new Vector2f(x, y);
        }
        public void UpdateFallingObjectPosition(int y)
        {
            UpdatePositionForCollection(y, FallingDamageObjects);
            UpdatePositionForCollection(y, FallingScoreObjects);
        }
        private void UpdatePositionForCollection(int y, List<Sprite> list)
        {
            Sprite s = null;
            foreach (var item in list)
            {
                item.Position = new Vector2f(item.Position.X, item.Position.Y + y);
                if (item.Position.Y >= GameWindow.Size.Y)
                {
                    s = item;
                }
            }
            if (s != null)
            {
                list.Remove(s);
            }
        }
        public void SetEndGameScreen()
        {
            _UI.CreateEndGameScreen();
        }        
    }
}
