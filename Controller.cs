using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static Lab4.UI;
using static SFML.Window.Keyboard;

namespace Lab4
{
    public class Controller
    {
        View _view;
        Model _model;

        private Random _random;
        private Timer _fallingObjectsTimer;
        private Player _player;
        private Level _currentLevel;

        private const int UNIT_SIZE = 50;
        private const int VERTICAL_UNIT_SIZE = 250;
        private const int HORIZONTAL_UNIT_SIZE = 10;
        private int _objectsFallingSpeed = 2;
        private const int OBJECT_DAMAGE = 1;
        private Stopwatch keyPressStopwatch = new Stopwatch();
        private Keyboard.Key _previousXAxisKey = Keyboard.Key.Unknown;

        bool isAKeyPressed = false;
        bool isDKeyPressed = false;
        bool isWKeyPressed = false;
        bool isSKeyPressed = false;
        private bool _noXAxisKeyPressed = true;
        private struct DirectionCoeff
        {
            public int _x, _y;
            public DirectionCoeff(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }

        public Controller(View view, Model model)
        {
            _view = view;
            _model = model;
            view.GameWindow.KeyPressed += OnKeyPressedHorizontal;
            view.GameWindow.KeyReleased += OnKeyReleasedHorizontal;
            view.GameWindow.KeyPressed += OnKeyPressedVertical;

            _random = new Random();
            _fallingObjectsTimer = new Timer(5000);
            _fallingObjectsTimer.Elapsed += SpawnFallingObject;

            _player = model.currentLevel.player;
            _currentLevel = model.currentLevel;

            _player.NewPosition += UpdatePlayerPos;
            _player.NewPosition += UpdatePlayerColliderPosition;

            _fallingObjectsTimer.Enabled = true;
            keyPressStopwatch.Start();
        }
        public void OnKeyPressedHorizontal(object sender, EventArgs e)
        {
            if (((KeyEventArgs)e).Code == Keyboard.Key.A)
            {
                isAKeyPressed = true;                
            }
            if (((KeyEventArgs)e).Code == Keyboard.Key.D)
            {
                isDKeyPressed = true;                
            }
            _noXAxisKeyPressed = false;
        }
        public void OnKeyReleasedHorizontal(object sender, EventArgs e)
        {
            if (((KeyEventArgs)e).Code == Keyboard.Key.A)
            {
                isAKeyPressed = false;
            }
            if (((KeyEventArgs)e).Code == Keyboard.Key.D)
            {
                isDKeyPressed = false;
            }
        }
        public void OnKeyPressedVertical(object sender, EventArgs e)
        {
            if (((KeyEventArgs)e).Code == Keyboard.Key.W)
            {
                MovePlayer(0, -VERTICAL_UNIT_SIZE);
            }
            if (((KeyEventArgs)e).Code == Keyboard.Key.S)
            {
                MovePlayer(0, VERTICAL_UNIT_SIZE);
            }
        }
        public void MovementHandler()
        {
            if (!isAKeyPressed && !isDKeyPressed)
            {
                _noXAxisKeyPressed = true;
            }
            if (_noXAxisKeyPressed || (isAKeyPressed && isDKeyPressed))
            {
                _player.BackToIdle();
            }
            else if (isAKeyPressed)
            {
                MovePlayer(-HORIZONTAL_UNIT_SIZE, 0);
                if (_previousXAxisKey == Keyboard.Key.Unknown)
                {
                    _player.BackToMoving();
                }
                else
                {
                    if (_previousXAxisKey == Keyboard.Key.A)
                    {
                        if (_player.StateType != typeof(MovingLeft) && _player.StateType != typeof(MovingRight))
                        {
                            _player.BackToMoving();
                        }
                    }
                    else
                    {
                        if (_player.StateType != typeof(MovingLeft) && _player.StateType != typeof(MovingRight))
                        {
                            _player.GoToOppositeMovingDirection();
                        }
                    }
                }
                
                _previousXAxisKey = Keyboard.Key.A;
            }
            else if (isDKeyPressed)
            {
                MovePlayer(HORIZONTAL_UNIT_SIZE, 0);
                if (_previousXAxisKey == Keyboard.Key.Unknown)
                {
                    _player.BackToMoving();
                }
                else
                {
                    if (_previousXAxisKey == Keyboard.Key.D)
                    {
                        if (_player.StateType != typeof(MovingLeft) && _player.StateType != typeof(MovingRight))
                        {
                            _player.BackToMoving();
                        }
                    }
                    else
                    {
                        if (_player.StateType != typeof(MovingLeft) && _player.StateType != typeof(MovingRight))
                        {
                            _player.GoToOppositeMovingDirection();
                        }
                    }
                }                
                _previousXAxisKey = Keyboard.Key.D;
            }            
        }
        private void SpawnFallingObject(Object source, ElapsedEventArgs e)
        {
            int randomObjPos = _random.Next(0, 1200);
            FallingObject fObj = _model.SpawnFallingObject(randomObjPos, 0);
            _view.AddFallingObject(fObj);
        }
        void UpdatePlayerPos(object sender, EventArgs e)
        {
            ChangePositionEventArgs changepos = (ChangePositionEventArgs)e;
            _view.UpdateModelPosition(changepos.X, changepos.Y);
        }
        void UpdatePlayerColliderPosition(object sender, EventArgs e)
        {
            Player player = (Player)sender;
            player.Collider = GetColiderOfModel();
        }
        public FloatRect GetColiderOfModel()
        {
            return _view.CurrentPlayerModel.GetGlobalBounds();
        }
        public void MovePlayer(int x, int y)
        {
            bool possibleCollision = PreUpdate(_player, x, y);
            if (possibleCollision)
            {
                return;
            }
            _player.Move(x, y);

//            Console.WriteLine($"Move player: x = {_player.X}, y = {_player.Y}");
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
        public void CheckAllGameObjectsCollision()
        {
            FallingObject toDestroy = null;
            foreach (var item in _model.SpawnedObjects)
            {
                bool isCollide = Engine.isIntersect(_player.Collider, item.Collider);
//                Console.WriteLine("status: "+isCollide);
                if (isCollide)
                {
                    _player.Health -= OBJECT_DAMAGE;
                    toDestroy = item;
                    Console.WriteLine("Player health: "+_player.Health);
                    //Console.WriteLine("------------------");
                    //Console.WriteLine();
                    //Console.WriteLine("-10 HP");
                    //Console.WriteLine();
                    //Console.WriteLine("------------------");
                }
            }
            if (toDestroy != null)
            {
                _model.DespawnFallingObject(toDestroy);
            }
        }
        public void RenderLevel()
        {
            _view.LoadLevel(_currentLevel);
        }
        public void AddPlayerCollider()
        {
            _player.Collider = GetColiderOfModel();
        }
        public void AddPlatformCollider(Platform p, FloatRect collider)
        {
            p.Collider = collider;
        }
        public void AddBarrier(int x, int y, int height, int width)
        {
            _currentLevel.barrier.Add(new FloatRect(x, y, height, width));
        }       
        public void AddFallingObjectCollider(FallingObject fObj, FloatRect collider)
        {
            fObj.Collider = collider;
            Engine.InitCollider(fObj.Collider, fObj.X, fObj.Y, fObj.Collider.Height, fObj.Collider.Width);
        }
        public void Update()
        {
            CheckAllGameObjectsCollision();
            FallingObject toDestroy = null;
            foreach (var item in _model.SpawnedObjects)
            {
                item.IncreaseVerticalSpeed(_objectsFallingSpeed);
                item.UpdateColliderPosition();
                if (item.Y >= _view.GameWindow.Size.Y)
                {
                    toDestroy = item;
                }
//                item.Print();
            }
            if (toDestroy != null)
            {
                _model.DespawnFallingObject(toDestroy);
            }
            _view.UpdateFallingObjectPosition(_objectsFallingSpeed);
        }
    }
}