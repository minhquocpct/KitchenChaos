using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            // Player not have kitchen object 
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            if (OnPlayerGrabbedObject != null) {
                OnPlayerGrabbedObject(this, EventArgs.Empty);
            }
        } 
    }

    public override void InteractAlt(Player player) {
        // throw new NotImplementedException();
    }
}
