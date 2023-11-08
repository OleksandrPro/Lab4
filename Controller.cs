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

        private int _objectsFallingSpeed = 2;
        private const int OBJECT_DAMAGE = 1;
        private Stopwatch keyPressStopwatch = new Stopwatch();
        private Keyboard.Key _previousXAxisKey = Keyboard.Key.Unknown;

        bool isAKeyPressed = false;
        bool isDKeyPressed = false;
        private readonly object keyLock = new object();

        public Controller(View view, Model model)
        {
            _view = view;
            _model = model;
            view.GameWindow.KeyPressed += OnKeyPressedHorizontal;
            view.GameWindow.KeyReleased += OnKeyReleasedHorizontal;
            view.GameWindow.KeyPressed += OnKeyPressedVertical;

            _random = new Random();
            _fallingObjectsTimer = new Timer(1000);
            _fallingObjectsTimer.Elapsed += SpawnFallingObject;

            _player = model.currentLevel.player;
            _currentLevel = model.currentLevel;

            _player.NewPosition += UpdatePlayerPos;
            _player.NewPosition += UpdatePlayerColliderPosition;
            _player.StateChanged += UpdateAnimation;

            _fallingObjectsTimer.Enabled = true;
            keyPressStopwatch.Start();
        }
        public void OnKeyPressedHorizontal(object sender, EventArgs e)
        {
            lock (keyLock)
            {
                if (((KeyEventArgs)e).Code == Keyboard.Key.A)
                {
                    isAKeyPressed = true;
                }
                if (((KeyEventArgs)e).Code == Keyboard.Key.D)
                {
                    isDKeyPressed = true;
                }
            }
        }
        public void OnKeyReleasedHorizontal(object sender, EventArgs e)
        {
            lock (keyLock)
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
        }
        public void OnKeyPressedVertical(object sender, EventArgs e)
        {
            int movementCoeff = 0;
            if (((KeyEventArgs)e).Code == Keyboard.Key.W)
            {
                movementCoeff = -1;
            }
            if (((KeyEventArgs)e).Code == Keyboard.Key.S)
            {
                movementCoeff = 1;
            }
            MovePlayerVertical(Model.VERTICAL_UNIT_SIZE * movementCoeff);
        }
        public void MovementHandler()
        {
            //engine
            if (!isAKeyPressed && !isDKeyPressed)
            {
                if (_player.CurrentState is MovingLeft)
                {
                    _player.ChangeState<IdleLeft>();
                }
                else if (_player.CurrentState is MovingRight)
                {
                    _player.ChangeState<IdleRight>();
                }
            }
            else if (isAKeyPressed && isDKeyPressed)
            {
                _player.BackToIdle();
            }
            else if (isAKeyPressed)
            {
                _player.ChangeState<MovingLeft>();
            }
            else if (isDKeyPressed)
            {
                _player.ChangeState<MovingRight>();
            }
            MovePlayerHorizontal();
        }
        private void SpawnFallingObject(Object source, ElapsedEventArgs e)
        {
            //engine or model
            int randomObjPos = _random.Next(0, 1200);
            FallingObject fObj = _model.SpawnFallingObject(randomObjPos, 0);
            _view.AddFallingObject(fObj);
        }
        void UpdatePlayerPos(object sender, EventArgs e)
        {
            //engine
            ChangePositionEventArgs changepos = (ChangePositionEventArgs)e;
            _view.UpdateModelPosition(changepos.X, changepos.Y);
        }
        void UpdatePlayerColliderPosition(object sender, EventArgs e)
        {
            //properties
            Player player = (Player)sender;
            player.Collider = GetColiderOfModel();
        }
        void UpdateAnimation(object sender, EventArgs e)
        {
            _view.UpdateAnimation(_player);
        }
        public FloatRect GetColiderOfModel()
        {
            return _view.CurrentPlayerModel.GetGlobalBounds();
        }
        public void MovePlayerHorizontal()
        {
            bool possibleCollision = PreUpdateX(_player);
            if (possibleCollision)
            {
                return;
            }
            _player.MoveHorizontal();
        }
        public void MovePlayerVertical(int y)
        {
            bool possibleCollision = PreUpdateY(_player, y);
            if (possibleCollision)
            {
                return;
            }
            _player.MoveVertical(y);
        }
        public bool PreUpdateX(Player p)
        {
            FloatRect newCollider = p.Collider;
            newCollider.Left += p.CurrentState.MovementCoeffcientX * Model.HORIZONTAL_UNIT_SIZE;
            return CheckPlayerPossibleCollision(newCollider);
        }
        public bool PreUpdateY(Player p, int posChange)
        {
            FloatRect newCollider = p.Collider;
            newCollider.Top += posChange;
            return CheckPlayerPossibleCollision(newCollider);
        }
        public bool CheckPlayerPossibleCollision(FloatRect playerCollider)
        {
            foreach (var item in _currentLevel.platforms)
            {
                bool willCollide = Engine.isIntersect(playerCollider, item.Collider);
                if (willCollide)
                {
                    return true;
                }
            }
            foreach (var item in _currentLevel.barrier)
            {
                bool willCollide = Engine.isIntersect(item, playerCollider);
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
//                Console.WriteLine($"to destroy: x = {toDestroy.X},  y = {toDestroy.Y}");
                int x = toDestroy.X;
                int y = toDestroy.Y;
                _model.DespawnFallingObject(toDestroy);
                _view.RemoveFallingObjectSprite(x, y);
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
//            Console.WriteLine(_model.SpawnedObjects.Count);
        }
    }
}