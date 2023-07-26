using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is no kitchen object here
            if (player.HasKitchenObject()) {
                // Player carry something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {
                // Player has nothing
            }
        } else {
            // There is kitchen object here
            if (player.HasKitchenObject()) {
                // Player carry something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Player holding plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                } else {
                    // Player not carry plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        // has plate on clear counter
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            } else {
                //
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlt(Player player) {
        // throw new System.NotImplementedException();
    }
}
