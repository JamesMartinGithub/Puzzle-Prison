using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public float timer;

    void OnEnable()
    {
        Invoke("Change", timer);
    }

    private void Change() {
        SceneManager.LoadScene("Main");
    }
}