using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {
    [SerializeField] private float moveSpeed = 7f; 
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float playerRadious = .7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;
    public event EventHandler OnPlayerPickingSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs: EventArgs {
        public BaseCounter selectedCounter;
    }
    public static Player Instance  {
        get {
            return instance;
        }
        set {
            instance = value;
        }
    }

    public bool IsWalking () {
        return isWalking;
    }
    private bool isWalking;
    private Vector3 LastInteractDir;
    private BaseCounter selectedCounter;
    private static Player instance;
    private void Awake() {
        if (Instance !=null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAtion;
        gameInput.OnInteractAltAction += GameInput_OnInteractAltAtion;
    }

    private void GameInput_OnInteractAltAtion(object sender, EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.InteractAlt(this);
        }
    }

    private void GameInput_OnInteractAtion(object sender, EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteraction();
    }
    private void HandleInteraction() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            LastInteractDir = moveDir;
        }

        bool canInteract = Physics.Raycast(transform.position, LastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask);
        if (canInteract) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                // Has BaseCounter
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
        // Debug.Log(selectedCounter);
    }
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadious, moveDir, moveDistance);
        if (!canMove) {
            // Cannot move toward moveDir

            // Attempt only X movement
            Vector3 moveDirX = new Vector3 (moveDir.x, 0, 0).normalized;
            canMove = moveDir.x!= 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadious, moveDirX, moveDistance);
            
            if (canMove) {
                // Can move only the X
                moveDir = moveDirX;
            } else {
                //Cannot move only the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3 (0, 0, moveDir.z).normalized;
                canMove = moveDir.z!= 0 &&  !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadious, moveDirZ, moveDistance);

                if (canMove) {
                    // Can move only the Z
                    moveDir = moveDirZ;
                } else {
                    // Cannot move any direction
                    // Debug.Log("Player cannot move any direction");
                }
            }
        }
        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        
        transform.forward = Vector3.Slerp( transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        if (OnSelectedCounterChanged != null) {
            OnSelectedCounterChanged(this, new OnSelectedCounterChangedEventArgs {
                selectedCounter = selectedCounter
            });
        }
    }
    public Transform getKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject (KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) {
            if (OnPlayerPickingSomething != null) {
                OnPlayerPickingSomething(this, EventArgs.Empty);
            }
        }
    }
    public KitchenObject GetKitchenObject() {
        return this.kitchenObject;
    }
    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }
    public bool HasKitchenObject() {
        return this.kitchenObject != null;
    }
}
