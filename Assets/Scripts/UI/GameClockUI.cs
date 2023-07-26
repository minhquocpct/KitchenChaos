using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameClockUI : MonoBehaviour
{
    [SerializeField] private Image timerClockImage;
    private void Start() {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;
        Hide();
    }
    private void Update() {
        Debug.Log(KitchenGameManager.Instance.GetGamePlayingTimerNomarlized());
        timerClockImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNomarlized();
    }
    private void KitchenGameManager_OnStateChange(object sender, EventArgs e) {
        if (KitchenGameManager.Instance.IsGamePlaying()) {
            timerClockImage.fillAmount = 0f;
            Show();
        }
    }
    private void Show() {
        timerClockImage.gameObject.SetActive(true);
    }
    private void Hide() {
        timerClockImage.gameObject.SetActive(false);
    }
}
