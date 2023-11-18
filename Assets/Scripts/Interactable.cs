using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isPuzzle;
    public GameObject puzzleUI;
    public bool smallText;
    public string[] smallTextString;
    public bool changeText;
    public string[] otherText;
    public bool linkedChange;
    public Interactable link;
    public float otherTime;
    public GameObject UICanvas;
    public bool hasAnimationClip;
    public Animation anim;
    public string animClipName;
    public bool bigText;
    public string[] bigTextString;
    public float timeTillEnd;
    public bool singleUse = false;
    public bool autoInteract = false;
    public bool playSound = false;
    public AudioSource sound;

    private void OnEnable()
    {
        if (autoInteract) {
            Interact();
        }
    }

    public void Interact() {
        if (smallText) {
            UICanvas.GetComponent<SmallTextSystem>().PlaySmallText(smallTextString);
        }
        if (hasAnimationClip) {
            anim.Play(animClipName);
        }
        if (bigText) {
            GameObject.Find("Wall Camera").GetComponent<Camera>().enabled = false;
            UICanvas.GetComponent<BigTextSystem>().PlayBigText(bigTextString);
        }
        if (playSound) {
            sound.Play();
        }
        if (!isPuzzle) {
            Invoke("EnableMovement", timeTillEnd);
            if (changeText)
            {
                if (linkedChange)
                {
                    link.smallTextString = otherText;
                    link.timeTillEnd = otherTime;
                }
                smallTextString = otherText;
                timeTillEnd = otherTime;
            }
        }
        else {
            GameObject.Find("Wall Camera").GetComponent<Camera>().enabled = false;
            puzzleUI.SetActive(true);
        }
    }

    private void EnableMovement() {
        GameObject.Find("Player").GetComponent<PlayerController>().EnableMovement();
        if (singleUse) {
            Destroy(gameObject);
        }
    }
}