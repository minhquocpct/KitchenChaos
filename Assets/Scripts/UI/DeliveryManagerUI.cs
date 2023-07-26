using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemple;
 
    private void Awake() {
        recipeTemple.gameObject.SetActive(false);
    }
    private void Start() {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManage_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeRemoved += DeliveryManage_OnRecipeRemoved;
    }

    private void DeliveryManage_OnRecipeSpawned(object sender, EventArgs e) {
        UpdateVisual();
    }
    private void DeliveryManage_OnRecipeRemoved(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach(Transform child in container) {
            if (child == recipeTemple) continue;
            Destroy(child.gameObject);
        }
        
        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaittingRecipeSOList()) {
            Transform recipeTransform = Instantiate(recipeTemple, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().GetRecipeName(recipeSO);
        }
    }
}
