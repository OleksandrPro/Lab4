using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace Lab4
{
    public class Player : ISubject
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
                HealthChanged?.Invoke(this, new EventArgs());
                if (_health == 0)
                    Died?.Invoke(this, new EventArgs());
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
                    StateChanged?.Invoke(this, new EventArgs());
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
                    Notify();
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
                    Notify();
                }
            }
        }
        private List<IObserver> _observers = new List<IObserver>();

        public delegate void StateChange(object sender, EventArgs e);
        public event StateChange StateChanged;
        public delegate void HealthChange(object sender, EventArgs e);
        public event HealthChange HealthChanged;
        public event HealthChange Died;
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
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }
        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }
        public void Notify()
        {

            foreach (var observer in _observers)
            {
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
