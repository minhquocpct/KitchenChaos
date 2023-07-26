using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject sizzlingParticles;
    [SerializeField] private GameObject stoveOnVisual;
    
    private void Start() {
        stoveCounter.OnStateFryingChanged += StoveCounter_OnstateFryingChanged;
    }

    private void StoveCounter_OnstateFryingChanged(object sender, StoveCounter.OnStateFryingChangedEventArgs e) {
        bool showVisual = e.stateFrying == StoveCounter.StateFrying.Frying || e.stateFrying == StoveCounter.StateFrying.Fried;
        sizzlingParticles.SetActive(showVisual);
        stoveOnVisual.SetActive(showVisual);
    }
}
