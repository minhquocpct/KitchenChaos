using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {

    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectsList;
    private void Start() {
        plateKitchenObject.OnIngredientAdd += PlateKitchenObject_OnIngredientAdd;
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectsList) {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }
    private void PlateKitchenObject_OnIngredientAdd(object sender, PlateKitchenObject.OnIngredientAddEventArgs e) {
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectsList) {
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO) {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
