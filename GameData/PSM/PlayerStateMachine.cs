using System;
using System.Collections.Generic;

namespace Lab4
{
    public class PlayerStateMachine
    {
        private Dictionary<Type, IPlayerState> _states;
        public IPlayerState _currentState;
        public Player player;
        public Type State { get; private set; }

        public PlayerStateMachine(Player player)
        {
            _states = new Dictionary<Type, IPlayerState>()
            {
                [typeof(IdleRight)] = new IdleRight(this),
                [typeof(IdleLeft)] = new IdleLeft(this),
                [typeof(MovingRight)] = new MovingRight(this),
                [typeof(MovingLeft)] = new MovingLeft(this)
            };
            this.player = player;
        }
        public void EnterIn<TState>() where TState : IPlayerState
        {
            if(_states.TryGetValue(typeof(TState), out IPlayerState state))
            {
                _currentState = state;
                State = state.GetType();
                player.CurrentState = state;
            }
        } 
    }
}
