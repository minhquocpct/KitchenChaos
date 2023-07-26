using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter {

    public static DeliveryCounter Instance  {
        get {
            return instance;
        }
        set {
            instance = value;
        }
    }
    private static DeliveryCounter instance;
    private void Awake() {
        Instance = this;
    }
    // Start is called before the first frame update
    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }

    public override void InteractAlt(Player player) {
        // throw new System.NotImplementedException();
    }
}
