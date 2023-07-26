using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameStartCountDownUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI countdownText;
    private void Start() {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;
        Hide();
    }

    private void Update() {
        countdownText.text = Math.Ceiling(KitchenGameManager.Instance.GetCountDownToStartTimer()).ToString();
    }
    private void KitchenGameManager_OnStateChange(object sender, EventArgs e) {
        if (KitchenGameManager.Instance.IsGameCountDowntToStart()) {
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
