using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;

namespace Lab4
{
    public class Controller : IControllerLaunch, IControllerView, IPositionChangeObserver, IStateObserver, IDeathObserver
    {
        private IView _view;
        private IModel _model;

        private Random _random;
        private Timer _fallingObjectsTimer;
        private Timer _fallingScoreObjectsTimer;
        private Player _player;
        public ILevel CurrentLevel;

        private bool isAKeyPressed = false;
        private bool isDKeyPressed = false;
        private bool _isNotGameOver = true;
        public bool IsNotGameOver
        {
            get { return _isNotGameOver; }
            set { _isNotGameOver = value; }
        }
        public Controller(IView view, IModel model)
        {
            _view = view;
            _model = model;            

            _random = new Random();
            _fallingObjectsTimer = new Timer(Model.FALLING_OBJECT_SPAWN_TIME);
            _fallingScoreObjectsTimer = new Timer(Model.FALLING_SCORE_OBJECT_SPAWN_TIME);

            _player = model.CurrentLevel.Player;
            CurrentLevel = model.CurrentLevel;            

            _fallingObjectsTimer.Enabled = true;
            _fallingScoreObjectsTimer.Enabled = true;

            InitializeEventSubscriptions(_view, _model);
        }
        private void InitializeEventSubscriptions(IView view, IModel model)
        {
            if (view == null || model == null)
            {
                throw new NullReferenceException();
            }
            view.GameWindow.KeyPressed += OnKeyPressedHorizontal;
            view.GameWindow.KeyReleased += OnKeyReleasedHorizontal;
            view.GameWindow.KeyPressed += OnKeyPressedVertical;

            _fallingObjectsTimer.Elapsed += SpawnDamageObject;
            _fallingScoreObjectsTimer.Elapsed += SpawnScoreObject;

            _player.Attach((IPositionChangeObserver)view);
            _player.Attach((IPositionChangeObserver)this);
            _player.Attach((IStateObserver)this);
            _player.Attach((IHealthEventObserver)view.UI);

            _player.Attach((IDeathObserver)view.UI);
            _player.Attach((IDeathObserver)this);

            ((ISpawnNewObject)model).Attach((ISpawnNewObjectObserver)view);
            ((IDespawnObject)model).Attach((IDespawnObjectObserver)view);
            ((IScoreUpdate)model).Attach((IScoreUpdateObserver)view.UI);
        }
        private void UnsubscribeFromEvents()
        {
            _view.GameWindow.KeyPressed -= OnKeyPressedHorizontal;
            _view.GameWindow.KeyReleased -= OnKeyReleasedHorizontal;
            _view.GameWindow.KeyPressed -= OnKeyPressedVertical;

            _fallingObjectsTimer.Elapsed -= SpawnDamageObject;
            _fallingScoreObjectsTimer.Elapsed -= SpawnScoreObject;

            _player.Detach((IPositionChangeObserver)_view);
            _player.Detach((IPositionChangeObserver)this);
            _player.Detach((IStateObserver)this);
            _player.Detach((IHealthEventObserver)_view.UI);
            _player.Detach((IDeathObserver)this);
            _player.Detach((IDeathObserver)_view.UI);

            ((ISpawnNewObject)_model).Detach((ISpawnNewObjectObserver)_view);
            ((IDespawnObject)_model).Detach((IDespawnObjectObserver)_view);
            ((IScoreUpdate)_model).Detach((IScoreUpdateObserver)_view.UI);
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
        public void Update(IPositionChanged subject)
        {
            Player p = subject as Player;
            p.Collider = GetColiderOfModel();
        }
        public void Update(IStateUpdate subject)
        {
            _view.UpdateAnimation(_player);
        }        
        public void MovementHandler()
        {
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
        private void SpawnDamageObject(Object source, ElapsedEventArgs e)
        {
            int randomObjPos = _random.Next(0, 1250);
            _model.SpawnFallingObject(Model.FallingObjectTypes.DamageObject, randomObjPos, 0);
        }
        private void SpawnScoreObject(Object source, ElapsedEventArgs e)
        {
            int randomObjPos = _random.Next(0, 1250);
            _model.SpawnFallingObject(Model.FallingObjectTypes.ScoreObject, randomObjPos, 0);
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
            foreach (var item in CurrentLevel.Platforms)
            {
                bool willCollide = Engine.isIntersect(playerCollider, item.Collider);
                if (willCollide)
                {
                    return true;
                }
            }
            foreach (var item in CurrentLevel.Barrier)
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
            FallingObject fObj = CheckGameObjectsCollision(_model.SpawnedDamageObjects);
            if(fObj!=null)
            {
                _model.DespawnFallingObject(Model.FallingObjectTypes.DamageObject, fObj);
                _player.ApplyDamage();
            }
        }
        private void CheckScoreObjectsCollision()
        {
            FallingObject fObj = CheckGameObjectsCollision(_model.SpawnedScoreObjects);
            if (fObj != null)
            {
                _model.DespawnFallingObject(Model.FallingObjectTypes.ScoreObject, fObj);
                _model.AddScore();
            }
        }
        public void RenderLevel()
        {
            _view.LoadLevel(CurrentLevel);
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
            CurrentLevel.AddBarrier(x, y, height, width);
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
            FallingObject toDestroy = UpdateFallingObjectsPossition(_model.SpawnedDamageObjects);
            if (toDestroy != null)
            {
                _model.DespawnFallingObject(Model.FallingObjectTypes.DamageObject, toDestroy);                
            }
            toDestroy = UpdateFallingObjectsPossition(_model.SpawnedScoreObjects);
            if (toDestroy != null)
            {
                _model.DespawnFallingObject(Model.FallingObjectTypes.ScoreObject, toDestroy);
            }
            _view.UpdateFallingObjectPosition(Model.OBJECT_FALLING_SPEED);
        }
        public void ShowFinalResult()
        {
            _view.SetEndGameScreen();
        }
        public void Update(IDied subject)
        {
            _isNotGameOver = false;
            _player.BackToIdle();
            UnsubscribeFromEvents();
        }
    }
}