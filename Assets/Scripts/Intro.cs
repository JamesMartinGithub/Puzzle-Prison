using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public PlayerController controller;
    public BigTextSystem bigText;
    public string[] text;
    public AudioSource music;

    void Start()
    {
        controller.enabled = false;
        GameObject.Find("Wall Camera").GetComponent<Camera>().enabled = false;
        bigText.timeToWrite = 4;
        bigText.PlayBigText(text);
        Invoke("EndIntro", 34.5f);
    }

    private void EndIntro() {
        controller.enabled = true;
        bigText.timeToWrite = 2.5f;
        music.Play();
    }
}