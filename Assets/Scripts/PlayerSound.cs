using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {
    [SerializeField] private float footstepTimerMax = .1f;
    private Player player;
    private float footstepTimer;

    private void Awake() {
        player = GetComponent<Player>();
    }
    private void Update() {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f) {
            footstepTimer = footstepTimerMax;
            if (player.IsWalking()) {
                SoundManager.Instance.PlayFootstep(player.transform.position);
            }

        }
    }
}
