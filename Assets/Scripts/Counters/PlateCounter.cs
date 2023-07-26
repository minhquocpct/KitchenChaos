using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCounter : BaseCounter {
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private float spawnPlateTimerMax = 4f;
    [SerializeField] private int spawnAmountMax = 4;
    private float spawnPlateTimer;
    private int plateSpawnAmount;
    
    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            // Player not has kitchen object
            if (plateSpawnAmount > 0) {
                plateSpawnAmount--;
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
                if (OnPlateRemoved != null) {
                    OnPlateRemoved(this, EventArgs.Empty);
                }
            }
        } else {
            if (player.GetKitchenObject().GetKitchenObjectSO() == kitchenObjectSO){
                SpawnPlate();
                player.GetKitchenObject().DestroySelf();
            }
        }
    }

    public override void InteractAlt(Player player) {
        // throw new System.NotImplementedException();
    }
    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;
            SpawnPlate();
        }
    }

    private void SpawnPlate() {
        if (plateSpawnAmount < spawnAmountMax) {
            plateSpawnAmount++;
            if (OnPlateSpawned != null) {
                OnPlateSpawned(this, EventArgs.Empty);
            }
        }
    }
}
