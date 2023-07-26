using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IHasProgress {
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;
    public static event EventHandler OnAnyCut;
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressEventArgs> OnProgressEventChange;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is no kitchen object here
            if (player.HasKitchenObject()) {
                // Player carry something
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;
                CuttingRecipeSO cuttingRecipeSO = getOutputRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            if (OnProgressEventChange != null && cuttingRecipeSO != null) {
                OnProgressEventChange(this, new IHasProgress.OnProgressEventArgs {
                    progressNormalized = (float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax 
            });
        }
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
        if (HasKitchenObject() && HasRepiceOutput(GetKitchenObject().GetKitchenObjectSO())) {
            //There is kitchen object here and has repice output
            cuttingProgress++;
            if (OnCut != null) {
                OnCut(this, EventArgs.Empty);
            }
            if (OnAnyCut != null) {
                OnAnyCut(this, EventArgs.Empty);
            }
            CuttingRecipeSO cuttingRecipeSO = getOutputRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            if (OnProgressEventChange != null) {
                OnProgressEventChange(this, new IHasProgress.OnProgressEventArgs {
                    progressNormalized = (float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax 
                });
            }
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }

        }
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = getOutputRecipeSOWithInput(kitchenObjectSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        } else {
            return null;
        }
    }
    private bool HasRepiceOutput(KitchenObjectSO kitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = getOutputRecipeSOWithInput(kitchenObjectSO);
        return cuttingRecipeSO != null;
    }
    private CuttingRecipeSO getOutputRecipeSOWithInput(KitchenObjectSO kitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == kitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
