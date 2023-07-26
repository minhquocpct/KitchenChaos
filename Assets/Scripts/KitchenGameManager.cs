using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class KitchenGameManager : MonoBehaviour {
    [SerializeField] private float waitingToStartTimer = 1f;
    [SerializeField] private float countdownToStartTimer = 3f;
    [SerializeField] private float gamePlayingTimerMax = 10f;
    public static KitchenGameManager Instance {
        get {
            return instance;
        }
        set {
            instance = value;
        }
    }
    public event EventHandler OnStateChange;
    private enum State {
        waitingToStart,
        countdownToStart,
        gamePlaying,
        gameOver
    }
    private State state;
    private static KitchenGameManager instance;
    private float gamePlayingTimer;

    private void Awake() {
        state = State.waitingToStart;
        Instance = this;
    }
    public bool IsGamePlaying() {
        return state == State.gamePlaying;
    }
    public bool IsGameCountDowntToStart() {
        return state == State.countdownToStart;
    }
    public bool IsGameOVer() {
        return state == State.gameOver;
    }
    public float GetCountDownToStartTimer() {
        return countdownToStartTimer;
    }
    public float GetGamePlayingTimerNomarlized() {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
    private void Update() {
        switch(state) {
            case State.waitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f) {
                    state = State.countdownToStart;
                    if (OnStateChange != null) {
                        OnStateChange(this, EventArgs.Empty);
                    }
                }
                break;
            case State.countdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f) {
                    state = State.gamePlaying;
                    if (OnStateChange != null) {
                        OnStateChange(this, EventArgs.Empty);
                    }
                    gamePlayingTimer = gamePlayingTimerMax;
                }
                break;
            case State.gamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f) {
                    state = State.gameOver;
                    if (OnStateChange != null) {
                        OnStateChange(this, EventArgs.Empty);
                    }
                }
                break;
            case State.gameOver:
                break;
        }
        Debug.Log(state);
    }
}
