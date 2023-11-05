﻿using SFML.Graphics;
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
        public int Height { get; set; }
        public int Width { get; set; }
        public FloatRect Collider { get; set; }
        private PlayerStateMachine _states;
        public PlayerState CurrentState { get; set; }
        public Type StateType { get; private set; }
        public bool IsStanding { get; set; }
        public int X 
        {  
            get
            {
                return _x;
            }
            set
            {
                if (_x == value)
                {
                    IsStanding = true;
                }
                else
                {
                    IsStanding = false;
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
        public Player(int x, int y, int width, int height) 
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
            _states = new PlayerStateMachine();
            _states.EnterIn<IdleRight>();
            CurrentState = _states._currentState;
            StateType = _states.State;
            IsStanding = true;
            _states.NewState += NewStateHandler;
        }
        public void ChangeState<TypeOfState>() where TypeOfState : PlayerState
        {
            if (typeof(TypeOfState) != StateType)
            {
                _states.EnterIn<TypeOfState>();
                CurrentState = _states._currentState;
                StateType = _states.State;
            }            
        }
        public void NewStateHandler(object sender, EventArgs e)
        {
            var state = (PlayerStateMachine)sender;
            StateType = state.State;
        }
        public void Move(int x, int y)
        {
            
//            CurrentState.Move();
            X += x;
            Y += y;
        }
        public void BackToIdle()
        {
            CurrentState.BackToIdle();
        }
        public void BackToMoving()
        {
            if (StateType != typeof(MovingLeft) || StateType != typeof(MovingRight)) 
            {
                CurrentState.BackToMoving();
            }
            
        }
        public void Update(bool b)
        {
            if (IsStanding != b)
            {
                BackToMoving();
                IsStanding = b;
            }
            else
            {
                BackToIdle();
            }
        }
    }
}
