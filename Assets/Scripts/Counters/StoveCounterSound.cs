using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour {
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start() {
        stoveCounter.OnStateFryingChanged += StoveCounter_OnStateFryingChanged;
    }

    private void StoveCounter_OnStateFryingChanged(object sender, StoveCounter.OnStateFryingChangedEventArgs e) {
        if (e.stateFrying == StoveCounter.StateFrying.Frying || e.stateFrying == StoveCounter.StateFrying.Fried) {
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }
}
