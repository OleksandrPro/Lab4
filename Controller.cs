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

        private Dictionary<Keyboard.Key, DirectionCoeff> MovementMap;
        private const int UNIT_SIZE = 50;
        private const int VERTICAL_UNIT_SIZE = 250;
        private const int HORIZONTAL_UNIT_SIZE = 10;
        private int _objectsFallingSpeed = 2;
        private const int OBJECT_DAMAGE = 1;
        private Stopwatch keyPressStopwatch = new Stopwatch();
        private Keyboard.Key _previousXAxisKey = Keyboard.Key.Unknown;

        bool isAKeyPressed = false;
        bool isDKeyPressed = false;
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

        public Controller(View viev, Model model)
        {
            _view = viev;
            _model = model;
            viev.GameWindow.KeyPressed += OnKeyPressed;
            viev.GameWindow.KeyReleased += OnKeyReleased;


            _random = new Random();
            _fallingObjectsTimer = new Timer(1000);
            _fallingObjectsTimer.Elapsed += SpawnFallingObject;
            MovementMap = new Dictionary<Keyboard.Key, DirectionCoeff>();
            FillMovementMap();

            _player = model.currentLevel.player;
            _currentLevel = model.currentLevel;

            _player.NewPosition += UpdatePlayerPos;
            _player.NewPosition += UpdatePlayerColliderPosition;

            _fallingObjectsTimer.Enabled = true;
            keyPressStopwatch.Start();
        }
        void onKeyPressed(object sender, EventArgs e)
        {
            KeyEventArgs kargs = (KeyEventArgs)e;
            bool successfullInput = false;
            foreach (var item in MovementMap)
            {
                if (item.Key == kargs.Code)
                {
                    successfullInput = true;

                    int updateX = item.Value._x * UNIT_SIZE;
                    int updateY = item.Value._y * UNIT_SIZE;
                    bool possibleCollision = PreUpdate(_player, updateX, updateY);
                    if (possibleCollision)
                    {
                        return;
                    }
                    //_player.BackToMoving();
                    MovePlayer(updateX, updateY);
 //                   Update();
                }
            }
            //if(!successfullInput)
            //{
            //    return;
            //}
            
            //TimeSpan timeSinceLastKeyPress = keyPressStopwatch.Elapsed;
            //if (TimeCondition(timeSinceLastKeyPress) && IsMatchingKeyValues(previousKey, kargs))
            //{
            //    if (previousKey == kargs.Code)
            //    {
            //        _player.BackToMoving();
            //    }
            //    else
            //    {
            //        _player.BackToIdle();
            //    }
            //}
            //if (XAxisKeyCondition(kargs))
            //{
            //    previousKey = kargs.Code;
            //}

            //keyPressStopwatch.Restart();
        }
        public void OnKeyPressed(object sender, EventArgs e)
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
        public void OnKeyReleased(object sender, EventArgs e)
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
        private bool TimeCondition(TimeSpan timeSinceLastKeyPress)
        {
            return timeSinceLastKeyPress.Milliseconds < 1000;
        }
        private bool IsInputFromXAxisKeys(KeyEventArgs kargs)
        {
            return kargs.Code == Keyboard.Key.D || kargs.Code == Keyboard.Key.A;
        }
        private bool IsMatchingKeyValues(Keyboard.Key previousKey, KeyEventArgs kargs)
        {
            return previousKey != Keyboard.Key.Unknown && kargs.Code == Keyboard.Key.D || kargs.Code == Keyboard.Key.A;
        }
        void PrintKeyPressData()
        {
            if (keyPressStopwatch.IsRunning)
            {
                // Получить прошедшее время с предыдущего вызова
                TimeSpan timeSinceLastKeyPress = keyPressStopwatch.Elapsed;


                // Ваш код для обработки времени между нажатиями клавиши
                Console.WriteLine("Прошло времени с предыдущего нажатия: " + timeSinceLastKeyPress);
                Console.WriteLine("milliseconds: " + timeSinceLastKeyPress.Milliseconds);

                // Сбросить секундомер
                keyPressStopwatch.Restart();
            }
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
        void UpdateGameObjectColliderPosition(object sender, EventArgs e)
        {
            GameObject obj = (GameObject)sender;
            Engine.InitCollider(obj.Collider, obj.Collider.Left + _objectsFallingSpeed, obj.Collider.Top + _objectsFallingSpeed, obj.Collider.Height, obj.Collider.Width);
        }
        public FloatRect GetColiderOfModel()
        {
            return _view.CurrentPlayerModel.GetGlobalBounds();
        }
        private void FillMovementMap()
        {
            //            MovementMap.Add(Keyboard.Key.W, new DirectionCoeff(0, -1));
            //            MovementMap.Add(Keyboard.Key.S, new DirectionCoeff(0, 1));
            MovementMap.Add(Keyboard.Key.W, new DirectionCoeff(0, -5));
            MovementMap.Add(Keyboard.Key.S, new DirectionCoeff(0, 5));
            //            MovementMap.Add(Keyboard.Key.W, new DirectionCoeff(0, -24));
            //            MovementMap.Add(Keyboard.Key.S, new DirectionCoeff(0, 24));
//            MovementMap.Add(Keyboard.Key.A, new DirectionCoeff(-1, 0));
//            MovementMap.Add(Keyboard.Key.D, new DirectionCoeff(1, 0));

        }
        public void MovePlayer(int x, int y)
        {
            int previousX = _player.X;
            bool possibleCollision = PreUpdate(_player, x, y);
            if (possibleCollision)
            {
                return;
            }
            _player.Move(x, y);

            //if (_player.StateType != typeof(IdleLeft) && _player.X < previousX) 
            //{
            //    _player.ChangeState<IdleLeft>();
            //}
            //else if(_player.StateType != typeof(IdleRight) && _player.X > previousX) 
            //{
            //    _player.ChangeState<IdleRight>();
            //}

            Console.WriteLine($"Move player: x = {_player.X}, y = {_player.Y}");
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
                bool willCollide = Engine.isIntersect(_player.Collider, item.Collider);
                if (willCollide)
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
        }
         
        public void Update()
        {
            //if (_playerPrevX != _player.X && _player.StateType != typeof(MovingLeft) && _player.StateType != typeof(MovingRight))
            //{
            //    _player.BackToMoving();
            //}
            //else if (_playerPrevX == _player.X && _player.StateType != typeof(IdleLeft) && _player.StateType != typeof(IdleRight))
            //{
            //    _player.BackToIdle();
            //}
            //_playerPrevX = _player.X;
            CheckAllGameObjectsCollision();
            
            FallingObject toDestroy = null;
            foreach (var item in _model.SpawnedObjects)
            {
                
                item.IncreaseVerticalSpeed(_objectsFallingSpeed);
                if(item.Y >=_view.GameWindow.Size.Y)
                {
                    toDestroy = item;
                    
                }                
            }
            if(toDestroy != null)
            {
                _model.DespawnFallingObject(toDestroy);
            }            
            _view.UpdateFallingObjectPosition(_objectsFallingSpeed);
        }
    }
}