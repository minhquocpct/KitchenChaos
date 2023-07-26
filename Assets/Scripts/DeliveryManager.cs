using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DeliveryManager : MonoBehaviour {
    [SerializeField] private ListRecipeSO listRecipeSO;
    [SerializeField] private float spawnRecipceTimerMax = 5f;
    [SerializeField] private int spawnAmountRecipeMax = 6;
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeRemoved;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance {
            get {
                return instance;
            }

            private set {
                instance = value;
            }
    }

    private static DeliveryManager instance;
    private List<RecipeSO> waitingRecipceSOList;
    private float spawnRecipceTimer;
    private int successDeliveryAmount;
    private void Awake() {
        waitingRecipceSOList = new List<RecipeSO>();
        if (Instance !=null) {
            Debug.LogError("There is more than one Delivery instance");
        }
        Instance = this;
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipceSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipceSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                bool plateContentsMatchesRecipe = true;
                // Has the same number of ingredients
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    // Cycling through all ingredient in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        // Cycling through all ingredient in the plate
                        if (recipeKitchenObjectSO == plateKitchenObjectSO) {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        // This recipe ingredient was not found on the plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                
                if (plateContentsMatchesRecipe) {
                    // Player deliver the correct recipe
                    successDeliveryAmount++;
                    waitingRecipceSOList.RemoveAt(i);
                    if (OnRecipeRemoved != null) {
                        OnRecipeRemoved(this, EventArgs.Empty);
                    }
                    if (OnRecipeSuccess != null) {
                        OnRecipeSuccess(this, EventArgs.Empty);
                    }
                    return;
                }
            }
        }
        // Mo match found 
        // Player do not deliver the correct recipe
        if (OnRecipeFailed != null) {
            OnRecipeFailed(this, EventArgs.Empty);
        }
    }
    public List<RecipeSO> GetWaittingRecipeSOList() {
        return waitingRecipceSOList;
    }
    public int GetSuccessDeliveryAmount() {
        return successDeliveryAmount;
    }
    private void Update() {
        spawnRecipceTimer -= Time.deltaTime;
        if (spawnRecipceTimer <= 0f) {
            spawnRecipceTimer = spawnRecipceTimerMax;
            RecipeSO recipeSO = listRecipeSO.RecipeSOList[UnityEngine.Random.Range(0, listRecipeSO.RecipeSOList.Count)];
            if (waitingRecipceSOList.Count < spawnAmountRecipeMax) {
                waitingRecipceSOList.Add(recipeSO);
                if (OnRecipeSpawned != null) {
                    OnRecipeSpawned(this, EventArgs.Empty);
                }
            }
        }
    }
}
