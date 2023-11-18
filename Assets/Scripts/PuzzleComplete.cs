using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleComplete : MonoBehaviour
{
    public GameObject door;
    public GameObject openDoor;
    public AudioSource sound;
    public void Complete() {
        door.SetActive(false);
        openDoor.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerController>().EnableMovement();
        GameObject.Find("Wall Camera").GetComponent<Camera>().enabled = true;
        sound.Play();
        gameObject.SetActive(false);
    }
}