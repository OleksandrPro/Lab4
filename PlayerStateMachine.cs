using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class PlayerStateMachine
    {
        private Dictionary<Type, PlayerState> _states;
        public PlayerState _currentState;
        public Type State { get; private set; }

        public delegate void StateChanged(object sender, EventArgs e);
        public event StateChanged NewState;

        public PlayerStateMachine()
        {
            _states = new Dictionary<Type, PlayerState>()
            {
                [typeof(IdleRight)] = new IdleRight(this),
                [typeof(IdleLeft)] = new IdleLeft(this),
                [typeof(MovingRight)] = new MovingRight(this),
                [typeof(MovingLeft)] = new MovingLeft(this)
            };
        }
        public void EnterIn<TState>() where TState : PlayerState
        {
            if(_states.TryGetValue(typeof(TState), out PlayerState state))
            {
//                _currentState?.Exit();
                _currentState = state;
                State = state.GetType();
//                _currentState?.Enter();
                NewState?.Invoke(this, new EventArgs());
            }
        } 
    }
}
