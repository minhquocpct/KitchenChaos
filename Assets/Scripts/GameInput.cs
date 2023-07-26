using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAltAction;
    private PlayerInputAction playerInputAction;
    private void Awake() {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlt.performed += InteractAlt_performed;
    }

    private void InteractAlt_performed(InputAction.CallbackContext context) {
        if (OnInteractAltAction != null) {
            OnInteractAltAction(this, EventArgs.Empty);
        }
    }

    private void Interact_performed(InputAction.CallbackContext obj) {
        if (OnInteractAction != null) {
            OnInteractAction(this, EventArgs.Empty);
        }
    }
    
    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputAction.Player.Movement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector; 
    }
}
