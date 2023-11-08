using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Player
    {
        private int _x;
        private int _y;
        private PlayerState _currentState;
        private PlayerState _previousState;
        public int Height { get; set; }
        public int Width { get; set; }
        public int Health { get; set; }
        public FloatRect Collider { get; set; }
        private PlayerStateMachine _states;
        public PlayerState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                if (_currentState != value)
                {
                    _previousState = _currentState;
                    _currentState = value;
                    StateChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public PlayerState PreviousState 
        { 
            get
            {
                return _previousState;
            }
            set
            {
                _previousState = value;
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
                    NewPosition?.Invoke(this, new ChangePositionEventArgs(_x, _y));
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
                    NewPosition?.Invoke(this, new ChangePositionEventArgs(_x, _y));
                }
            }
        }
        public delegate void PositionChanged(object sender, EventArgs e);
        public event PositionChanged NewPosition;
        public delegate void StateChange(object sender, EventArgs e);
        public event StateChange StateChanged;        
        public Player(int x, int y, int width, int height, int health)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
            Health = health;

            _states = new PlayerStateMachine(this);
            
            _states.NewState += NewStateHandler;
            _states.EnterIn<IdleRight>();
        }
        public Player(int x, int y, int width, int height) : this(x, y, width, height, 3) { }
        public void ChangeState<TypeOfState>() where TypeOfState : PlayerState
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
        public void BackToMoving()
        {
            if (StateType != typeof(MovingLeft) ||   StateType != typeof(MovingRight)) 
            {
                CurrentState.BackToMoving();
            }            
        }
        public void GoToOppositeMovingDirection()
        {
            CurrentState.GoToOppositeMovingDirection();
        }
    }
}
