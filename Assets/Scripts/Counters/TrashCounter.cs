using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TrashCounter : BaseCounter {

    public static event EventHandler OnAnyObjectTrashed;
    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            // Player carry something
            player.GetKitchenObject().DestroySelf();
            if (OnAnyObjectTrashed != null) {
                OnAnyObjectTrashed(this, EventArgs.Empty);
            }
        }
    }

    public override void InteractAlt(Player player) {
        // throw new System.NotImplementedException();
    }
}
