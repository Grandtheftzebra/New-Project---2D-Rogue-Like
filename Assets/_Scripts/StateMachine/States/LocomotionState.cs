using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace StateMachine.States
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator animator) :
            base(player, animator) 
        {

        }

        public override void Enter()
        {
            Debug.Log("Entered Loco State");
            player.CanInteract = true;

            //animator.CrossFade(locomotion, crossfadeDuration);
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
            Debug.Log("Exited Loco state");
            player.CanInteract = false;
        }
    }
}