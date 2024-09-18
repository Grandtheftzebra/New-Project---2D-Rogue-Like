using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace StateMachine.States
{
    
public class DashState : BaseState
{
        public DashState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void Enter()
        {
            Debug.Log($"Enter Dash state");
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            player.HandleMovement();
            
        }

        public override void Exit()
        {
            Debug.Log($"Exited Dash state");
        }
    }
}
