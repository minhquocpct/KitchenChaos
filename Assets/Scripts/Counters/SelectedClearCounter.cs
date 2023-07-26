using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedClearCounter : MonoBehaviour {
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if (e.selectedCounter == baseCounter) {
            Show();
        } else {
            Hide();
        }
    }
    private void Show() {
        foreach(GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(true);
        }

    }
    private void Hide() {
        foreach(GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(false);
        }
    }
}
