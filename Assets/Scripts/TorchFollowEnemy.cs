using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchFollowEnemy : MonoBehaviour
{
    public Transform enemy;
    private float start;
    private float end;
    private float lerp = 0f;

    void FixedUpdate()
    {
        transform.position = enemy.position;
        if (lerp > 0) {
            lerp -= 0.02f;
            transform.localEulerAngles = new Vector3(0, Mathf.SmoothStep(start, end, 1 - lerp), 0);
        }
    }
    public void SetDirection(string dir) {
        switch (dir) {
            case "up":
                lerp = 1f;
                end = 1f;
                start = CurrentEuler();
                if (start > 269)
                {
                    start = -90;
                }
                break;
            case "down":
                lerp = 1f;
                end = 180f;
                start = CurrentEuler();
                break;
            case "right":
                lerp = 1f;
                end = 90f;
                start = CurrentEuler();
                break;
            case "left":
                lerp = 1f;
                end = 270f;
                start = CurrentEuler();
                if (start < 1){
                    start = 360;
                }
                break;
        }
    }

    private float CurrentEuler() {
        return transform.localEulerAngles.y;
    }
}