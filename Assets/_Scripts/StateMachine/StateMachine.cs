using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR;

namespace StateMachine
{
    public class StateMachine
    {
        private StateNode _current;

        private Dictionary<Type, StateNode> _stateNodes = new();
        private HashSet<ITransition> _anyTransitions = new();

        public void Update()
        {
            ITransition transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);
            
            _current.State?.Update();
        }
        
        public void FixedUpdate()
        {
            _current.State?.FixedUpdate();
        }
        
        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            foreach (var transition in _current.Transitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            return null;
        }
        
        private void ChangeState(IState state)
        {
            if (state == _current.State)
                return;

            IState previousState = _current.State;
            IState nextState = _stateNodes[state.GetType()].State;
            
            previousState?.Exit();
            nextState?.Enter();

            _current = _stateNodes[state.GetType()];
        }
        

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        private StateNode GetOrAddNode(IState state)
        {
            StateNode node = _stateNodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                _stateNodes.Add(state.GetType(), node);
            }

            return node;
        }

        public void SetState(IState state)
        {
            _current = _stateNodes[state.GetType()];
            _current.State?.Enter();
        }
    }

    internal class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
        
    }
}