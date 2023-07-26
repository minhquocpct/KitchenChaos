using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    public void GetRecipeName(RecipeSO recipeSO) {
        recipeNameText.text = recipeSO.nameRecipe; 
        foreach (Transform child in iconContainer) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList) {
            Transform inconTransform = Instantiate(iconTemplate, iconContainer);
            inconTransform.gameObject.SetActive(true);
            inconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

}
