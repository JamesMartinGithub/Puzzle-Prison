using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterEnemyColTut : MonoBehaviour
{
    public Animation anim;
    public EnemyController enemy;
    public bool isMain = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Wedge") {
            if (isMain) {
                anim.Play("Caught");
                Invoke("ResetLevel", 1f);
            }
            else
            {
                anim.Play("Caught");
                transform.position = new Vector3(12.79f, 3.41399956f, 14f);
                enemy.ResetPos();
            }
        }
    }

    private void ResetLevel() {
        SceneManager.LoadScene("Main");
    }
}