using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    public static SoundManager Instance  {
        get {
            return instance;
        }
        set {
            instance = value;
        }
    }
    private static SoundManager instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManage_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManage_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPlayerPickingSomething += Player_OnPlayerPickingSomething;
        BaseCounter.OnObjectDropHere += BaseCounter_OnObjectDropHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnObjectDropHere(object sender, EventArgs e) {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPlayerPickingSomething(object sender, EventArgs e) {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManage_OnRecipeFailed(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManage_OnRecipeSuccess(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        AudioClip audioClip = audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)];
        PlaySound(audioClip, position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    public void PlayFootstep(Vector3 position, float volume = 1f) {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
}
