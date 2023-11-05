using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab4.UI;

namespace Lab4
{
    public class Controller
    {
        SFML.Window.Window MainWindow;
        public UI UIElements { get; set; }
        private Player player { get; set; }
        private Dictionary<Keyboard.Key, DirectionCoeff> MovementMap;
        private const int UNIT_SIZE = 50;
        private Level _currentLevel;
        private int _counter = 0;
        private Random _random;
//        private ObjectPool<FallingObject> _fallingObjects;
//        private List<Object> _checkCollision;
        private struct DirectionCoeff
        {
            public int _x, _y;
            public DirectionCoeff(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }
        public Controller(SFML.Window.Window window)
        {
            _random = new Random();
            MainWindow = window;
            UIElements = new UI(window);
            UIElements.AddController(this);
            _currentLevel = new Level1();
            player = _currentLevel.player;
            UIElements.LoadLevel(_currentLevel);
            player.Collider = SetColider();
            MovementMap = new Dictionary<Keyboard.Key, DirectionCoeff>();
            FillMovementMap();
            MainWindow.KeyPressed += OnKeyPressed;
            player.NewPosition += UpdatePlayerPos;
            player.NewPosition += UpdateColliderPosition;
//            _fallingObjects = new ObjectPool<FallingObject>(6);
        }
        void OnKeyPressed(object sender, EventArgs e)
        {
            KeyEventArgs kargs = (KeyEventArgs)e;
            foreach (var item in MovementMap)
            {
                if (item.Key == kargs.Code)
                {
                    int updateX = item.Value._x * UNIT_SIZE;
                    int updateY = item.Value._y * UNIT_SIZE;
                    bool possibleCollision = PreUpdate(player, updateX, updateY);
                    if (possibleCollision)
                    {
                        return;
                    }
                    MovePlayer(updateX, updateY);
                }
            }
        }
        void UpdatePlayerPos(object sender, EventArgs e)
        {
            ChangePositionEventArgs changepos = (ChangePositionEventArgs)e;
            UIElements.UpdateModelPosition(changepos.X, changepos.Y);
        }
        void UpdateColliderPosition(object sender, EventArgs e)
        {
            Player player = (Player)sender;
            player.Collider = SetColider();
        }
        public void MovePlayer(int x, int y)
        {
            player.X += x;
            player.Y += y;
            Console.WriteLine($"Move player: x = {player.X}, y = {player.Y}");
        }
        public Vector2f GetPlayerPosition()
        {
            return new Vector2f(player.X, player.Y);
        }
        private void FillMovementMap()
        {
//            MovementMap.Add(Keyboard.Key.W, new DirectionCoeff(0, -1));
//            MovementMap.Add(Keyboard.Key.S, new DirectionCoeff(0, 1));
            MovementMap.Add(Keyboard.Key.W, new DirectionCoeff(0, -5));
            MovementMap.Add(Keyboard.Key.S, new DirectionCoeff(0, 5));
//            MovementMap.Add(Keyboard.Key.W, new DirectionCoeff(0, -24));
//            MovementMap.Add(Keyboard.Key.S, new DirectionCoeff(0, 24));
            MovementMap.Add(Keyboard.Key.A, new DirectionCoeff(-1, 0));
            MovementMap.Add(Keyboard.Key.D, new DirectionCoeff(1, 0));
        }
        public FloatRect SetColider()
        {
            return UIElements.CurrentPlayerModel.GetGlobalBounds();
        }
        public void AddPlatformCollider(Platform p, FloatRect collider)
        {
            p.Collider = collider;
        }
        public void Update()
        {
            foreach (var item in _currentLevel.platforms)
            {
                bool isColliding = Engine.isIntersect(player.Collider, item.Collider);
                if (isColliding) 
                {
                    ++_counter;
                    Console.WriteLine($"Objects are colliding : {_counter}");
                }
            }
        }
        public bool PreUpdate(Player p, int updateX, int updateY)
        {
            FloatRect newCollider = p.Collider;
            newCollider.Left += updateX;
            newCollider.Top += updateY;

            foreach (var item in _currentLevel.platforms)
            {
                bool willCollide = Engine.isIntersect(newCollider, item.Collider);
                if (willCollide)
                {
                    return true;
                }
            }
            foreach (var item in _currentLevel.barrier)
            {
                bool willCollide = Engine.isIntersect(item, newCollider);
                if (willCollide)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
