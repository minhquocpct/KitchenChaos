using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class GameOverUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI orderDeliveryText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button backToMenuButton;
    private void Awake() {
        playAgainButton.onClick.AddListener(() =>{
            Loader.Load(Loader.Scene.GameScene);
        });
        backToMenuButton.onClick.AddListener(() =>{
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void Start() {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;
        Hide();
    }
    private void KitchenGameManager_OnStateChange(object sender, EventArgs e) {
        if (KitchenGameManager.Instance.IsGameOVer()) {
            orderDeliveryText.text = DeliveryManager.Instance.GetSuccessDeliveryAmount().ToString();
            Show();
        } else {
            Hide();
        }
    }
    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}
