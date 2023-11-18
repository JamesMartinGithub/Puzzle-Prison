using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTextSystem : MonoBehaviour
{
    public GameObject smallTextPrefab;
    
    public void PlaySmallText(string[] text) {
        if (GameObject.FindGameObjectWithTag("SmallText")) {
            Destroy(GameObject.FindGameObjectWithTag("SmallText"));
        }
        GameObject p = Instantiate(smallTextPrefab);
        p.GetComponent<SmallText>().RenderText(text);
    }
}