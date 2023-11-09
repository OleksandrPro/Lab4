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
    public class Controller : IObserver, IControllerLaunch, IControllerView
    {
        private View _view;
        private Model _model;

        private Random _random;
        private Timer _fallingObjectsTimer;
        private Timer _fallingScoreObjectsTimer;
        private Player _player;
        private Level _currentLevel;

        private bool isAKeyPressed = false;
        private bool isDKeyPressed = false;
        private bool _isNotGameOver = true;
        public bool IsNotGameOver
        {
            get { return _isNotGameOver; }
            set { _isNotGameOver = value; }
        }
        public Controller(View view, Model model)
        {
            _view = view;
            _model = model;
            view.GameWindow.KeyPressed += OnKeyPressedHorizontal;
            view.GameWindow.KeyReleased += OnKeyReleasedHorizontal;
            view.GameWindow.KeyPressed += OnKeyPressedVertical;

            _model.ScoreChanged += UpdateUIScore;

            _random = new Random();
            _fallingObjectsTimer = new Timer(Model.FALLING_OBJECT_SPAWN_TIME);
            _fallingObjectsTimer.Elapsed += SpawnFallingObject;
            _fallingScoreObjectsTimer = new Timer(Model.FALLING_SCORE_OBJECT_SPAWN_TIME);
            _fallingScoreObjectsTimer.Elapsed += SpawnFallingScoreObject;

            _player = model.CurrentLevel.player;
            _currentLevel = model.CurrentLevel;

            _player.Attach(_view);
            _player.Attach((IObserver)this);

            _player.StateChanged += UpdateAnimation;
            _player.HealthChanged += UpdateUIHealth;
            _player.Died += EndGame;

            _fallingObjectsTimer.Enabled = true;
            _fallingScoreObjectsTimer.Enabled = true;
//            keyPressStopwatch.Start();
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
        public void Update(ISubject subject)
        {
            Player p = subject as Player;
            p.Collider = GetColiderOfModel();
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
            int randomObjPos = _random.Next(0, 1250);
            FallingObject fObj = _model.SpawnFallingObject(randomObjPos, 0);
            _view.AddFallingObject(fObj);
        }
        private void SpawnFallingScoreObject(Object source, ElapsedEventArgs e)
        {
            int randomObjPos = _random.Next(0, 1250);
            FallingObject fObj = _model.SpawnFallingScoreObject(randomObjPos, 0);
            _view.AddFallingScoreObject(fObj);
        }
        void UpdateAnimation(object sender, EventArgs e)
        {
            _view.UpdateAnimation(_player);
        }
        public FloatRect GetColiderOfModel()
        {
            return _view.CurrentPlayerModel.GetGlobalBounds();
        }
        private void MovePlayerHorizontal()
        {
            bool possibleCollision = PreUpdateX(_player);
            if (possibleCollision)
            {
                return;
            }
            _player.MoveHorizontal();
        }
        private void MovePlayerVertical(int y)
        {
            bool possibleCollision = PreUpdateY(_player, y);
            if (possibleCollision)
            {
                return;
            }
            _player.MoveVertical(y);
        }
        private bool PreUpdateX(Player p)
        {
            FloatRect newCollider = p.Collider;
            newCollider.Left += p.CurrentState.MovementCoeffcientX * Model.HORIZONTAL_UNIT_SIZE;
            return CheckPlayerPossibleCollision(newCollider);
        }
        private bool PreUpdateY(Player p, int posChange)
        {
            FloatRect newCollider = p.Collider;
            newCollider.Top += posChange;
            return CheckPlayerPossibleCollision(newCollider);
        }
        private bool CheckPlayerPossibleCollision(FloatRect playerCollider)
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
        private FallingObject CheckGameObjectsCollision(List<FallingObject> list)
        {
            FallingObject toDestroy = null;
            foreach (var item in list)
            {
                bool isCollide = Engine.isIntersect(_player.Collider, item.Collider);
                if (isCollide)
                {
                    toDestroy = item;
                    return toDestroy;                                       
                }
            }
            return toDestroy;
        }
        private void CheckDamageObjectsCollision()
        {
            FallingObject fObj = CheckGameObjectsCollision(_model.SpawnedObjects);
            if(fObj!=null)
            {
                int x = fObj.X;
                int y = fObj.Y;
                _model.DespawnFallingObject(fObj);
                _view.RemoveFallingObjectSprite(x, y);
                _player.ApplyDamage();
            }
        }
        private void CheckScoreObjectsCollision()
        {
            FallingObject fObj = CheckGameObjectsCollision(_model.SpawnedScoreObjects);
            if (fObj != null)
            {
                int x = fObj.X;
                int y = fObj.Y;
                _model.DespawnFallingScoreObject(fObj);
                _view.RemoveFallingScoreObjectSprite(x, y);
                _model.AddScore();
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
        private FallingObject UpdateFallingObjectsPossition(List<FallingObject> list)
        {
            FallingObject toDestroy = null;
            foreach (var item in list)
            {
                item.IncreaseVerticalSpeed(Model.OBJECT_FALLING_SPEED);
                item.UpdateColliderPosition();
                if (item.Y >= _view.GameWindow.Size.Y)
                {
                    toDestroy = item;
                }
            }
            return toDestroy;
        }
        public void Update()
        {
            CheckDamageObjectsCollision();
            CheckScoreObjectsCollision();
            FallingObject toDestroy = UpdateFallingObjectsPossition(_model.SpawnedObjects);
            if (toDestroy != null)
            {
                _model.DespawnFallingObject(toDestroy);                
            }

            toDestroy = UpdateFallingObjectsPossition(_model.SpawnedScoreObjects);
            if (toDestroy != null)
            {
                _model.DespawnFallingScoreObject(toDestroy);
            }
            _view.UpdateFallingObjectPosition(Model.OBJECT_FALLING_SPEED);
        }
        public void UpdateUIHealth(object sender, EventArgs e)
        {
            Player player = (Player)sender;
            _view.UpdateUIHealth(player.Health);
        }
        public void UpdateUIScore(object sender, EventArgs e)
        {
            _view.UpdateUIScore(_model.Score);
        }
        public void EndGame(object sender, EventArgs e)
        {
            _isNotGameOver = false;            
        }
        public void ShowFinalResult()
        {
            _view.SetEndGameScreen();
        }
    }
}