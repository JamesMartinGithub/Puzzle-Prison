using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEscape : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            GameObject.Find("Player").GetComponent<PlayerController>().EnableMovement();
            GameObject.Find("Wall Camera").GetComponent<Camera>().enabled = true;
            gameObject.SetActive(false);
        }
    }
}