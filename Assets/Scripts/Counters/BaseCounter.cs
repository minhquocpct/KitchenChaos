using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent {
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    public static event EventHandler OnObjectDropHere;
    public abstract void Interact(Player player);
    public abstract void InteractAlt(Player player);
    public Transform getKitchenObjectFollowTransform() {
        return counterTopPoint;
    }
    public void SetKitchenObject (KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) {
            if (OnObjectDropHere != null) {
                OnObjectDropHere(this, EventArgs.Empty);
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
