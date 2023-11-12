using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public class Player : IHealthEvents, IStateUpdate, IPositionChanged, IDied
    {
        private int _x;
        private int _y;
        private int _health;
        private IPlayerState _currentState;
        public int Health 
        { 
            get
            {
                return _health;
            }
            set
            {
                _health = value;
                HealthEventNotify();
                if (_health == 0)
                    DiedNotify();
            }
        }
        public FloatRect Collider { get; set; }
        private PlayerStateMachine _states;
        public IPlayerState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    StateEventNotify();
                }
            }
        }       
        public Type StateType { get; private set; }
        public int X 
        {  
            get
            {
                return _x;
            }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    PositionChangeNotify();
                }
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value != _y)
                {
                    _y = value;
                    PositionChangeNotify();
                }
            }
        }
        private List<IPositionChangeObserver> _positionChangeObservers = new List<IPositionChangeObserver>();
        private List<IHealthEventObserver> _healthEventObservers = new List<IHealthEventObserver>();
        private List<IStateObserver> _stateEventObservers = new List<IStateObserver>();
        private List<IDeathObserver> _deathEventObservers = new List<IDeathObserver>();

        public Player(int x, int y, int health)
        {
            if (health <= 0)
                throw new ArgumentException("HP can't be 0 or lower");
            X = x;
            Y = y;
            Health = health;
            _states = new PlayerStateMachine(this);
            
            _states.NewState += NewStateHandler;
            _states.EnterIn<IdleRight>();
        }
        public Player(int x, int y) : this(x, y, Model.PLAYER_START_HEALTH) { }
        public void Attach(IPositionChangeObserver observer)
        {
            _positionChangeObservers.Add(observer);
        }
        public void Detach(IPositionChangeObserver observer)
        {
            _positionChangeObservers.Remove(observer);
        }
        public void PositionChangeNotify()
        {
            foreach (var observer in _positionChangeObservers)
            {
                observer.Update(this);
            }
        }
        public void Attach(IHealthEventObserver observer)
        {
            _healthEventObservers.Add(observer);
        }
        public void Detach(IHealthEventObserver observer)
        {
            _healthEventObservers.Remove(observer);
        }
        public void HealthEventNotify()
        {
            foreach (var observer in _healthEventObservers)
            {
                observer.Update(this);
            }
        }
        public void Attach(IStateObserver observer)
        {
            _stateEventObservers.Add(observer);
        }
        public void Detach(IStateObserver observer)
        {
            _stateEventObservers.Remove(observer);
        }
        public void StateEventNotify()
        {
            foreach (var observer in _stateEventObservers)
            {
                observer.Update(this);
            }            
        }
        public void Attach(IDeathObserver observer)
        {
            _deathEventObservers.Add(observer);
        }
        public void Detach(IDeathObserver observer)
        {
            _deathEventObservers.Remove(observer);
        }
        public void DiedNotify()
        {
            for (int i = 0; i < _deathEventObservers.Count; i++)
            {
                var observer = _deathEventObservers[i];
                observer.Update(this);
            }
        }
        public void ChangeState<TypeOfState>() where TypeOfState : IPlayerState
        {
            if (typeof(TypeOfState) != StateType)
            {
                _states.EnterIn<TypeOfState>();
            }            
        }
        public void NewStateHandler(object sender, EventArgs e)
        {
            var state = (PlayerStateMachine)sender;
            CurrentState = state._currentState;
            StateType = state.State;
        }
        public void MoveHorizontal()
        {
            CurrentState.Move();
        }
        public void MoveVertical(int y)
        {
            Y += y;
        }
        public void BackToIdle()
        {
            if (StateType != typeof(IdleLeft) && StateType != typeof(IdleRight))
            {
                CurrentState.BackToIdle();
            }                
        }
        public void ApplyDamage()
        {
            ApplyCertainDamage(Model.OBJECT_DAMAGE);
        }
        private void ApplyCertainDamage(int damage)
        {
            Health -= damage;
        }
    }
}
