using UnityEngine;

namespace StateMachine
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController player;
        protected readonly Animator animator;

        protected static readonly int locomotion = Animator.StringToHash("Locomotion");
        
        protected const float crossfadeDuration = 0.0f; // 2D don't need fade duration between States

        protected BaseState(PlayerController player, Animator animator)
        {
            this.player = player;
            this.animator = animator;
        }
        
        
        public virtual void Enter()
        {
            
        }
        
        public virtual void Update()
        {
            player.MakePlayerLookAtMousePosition();
            
        }

        public virtual void FixedUpdate()
        {
            
        }
        
        public virtual void Exit()
        {
            
        }
    }
}