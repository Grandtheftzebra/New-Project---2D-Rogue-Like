using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputController;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> Move;
    public event Action Interact;
    public event Action Dash;
    public event Action RangeAttack;
    public event Action MeleeAttack;
    
    public Vector2 Movement => _inputController.Player.Move.ReadValue<Vector2>();
    
    public Vector3 MousePos => (Vector2)_mainCam.ScreenToWorldPoint(Input.mousePosition); 
    
    private PlayerInputController _inputController;
    private Camera _mainCam;

    private void OnEnable()
    {
        if (_inputController == null)
        {
            _inputController = new PlayerInputController();
            _inputController.Player.SetCallbacks(this);
        }
        
        _inputController.Enable(); //TODO: Maybe refactor this into a method later
        _mainCam = Camera.main; // Unity has to recompile otherwise this can throw an error, in Project Settings > Editor > Disable 
    }

    private void OnDisable()
    {
        _inputController.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move?.Invoke(context.ReadValue<Vector2>()); // Not really in use right now, maybe redundant since Movement is used for Player Movement!
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            RangeAttack?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            Interact?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        Dash?.Invoke();
    }

    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            MeleeAttack?.Invoke();
    }
}