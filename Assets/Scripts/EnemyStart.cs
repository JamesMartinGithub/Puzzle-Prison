using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStart : MonoBehaviour
{
    public EnemyController enemy;

    void OnEnable()
    {
        enemy.StartMoving();
    }
}