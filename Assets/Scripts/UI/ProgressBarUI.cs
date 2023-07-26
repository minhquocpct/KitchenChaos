using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image image;
    private IHasProgress hasProgress;
    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.LogError("GameObject"+hasProgressGameObject+"don't have commponent implements IHasProgress");
        }
        hasProgress.OnProgressEventChange += HasProgress_OnProgressBarChange;
        image.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressBarChange(object sender, IHasProgress.OnProgressEventArgs e) {
        image.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f) { 
            Hide();
        } else {
            Show();
        }
    }

    // private void CuttingCounter_OnProgressBarChange(object sender, CuttingCounter.OnProgressEventArgs e) {
    //     image.fillAmount = e.progressNormalized;
    //     if (e.progressNormalized == 0f || e.progressNormalized == 1f) { 
    //         Hide();
    //     } else {
    //         Show();
    //     }
    // }
    private void Show() {
        gameObject.SetActive(true);
    } 
    private void Hide() {
        gameObject.SetActive(false);
    }
}
