using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject {
    [SerializeField] private List<KitchenObjectSO> validKitcheObjectSOList;
    public event EventHandler<OnIngredientAddEventArgs> OnIngredientAdd;
    public class OnIngredientAddEventArgs: EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }

    private List<KitchenObjectSO> kitchenObjectSOList; 
    public void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {
        if (!validKitcheObjectSOList.Contains(kitchenObjectSO)) {
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            return false;
        } else {
            kitchenObjectSOList.Add(kitchenObjectSO);
            if (OnIngredientAdd != null) {
                OnIngredientAdd(this, new OnIngredientAddEventArgs{
                    kitchenObjectSO = kitchenObjectSO
                });
            }
            return true;
        }

    }
    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return kitchenObjectSOList;
    }
}
