using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints;
    public string startDirection; //up, down, left, right
    public float speed;
    private Vector3 direction;
    private bool moving = false;
    private int selected;
    public bool moveOnAwake;
    public TorchFollowEnemy torch;
    public GameObject forwardSprite;
    public GameObject backwardSprite;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        changeDirection(startDirection);
        selected = 0;
        if (moveOnAwake) {
            moving = true;
        }
    }

    public void StartMoving() {
        moving = true;
    }

    private void FixedUpdate()
    {
        if (moving) {
            if (Vector3.Distance(waypoints[selected].position, transform.position) > 0.2f)
            {
                transform.Translate(direction * speed);
            }
            else
            {
                moving = false;
                SelectNext();
            }
        }
    }

    private void SelectNext() {
        if (waypoints[selected].gameObject.GetComponent<WayPoint>().isEnd) {
            moving = false;
            gameObject.SetActive(false);
        }
        else {
            if (waypoints[selected].gameObject.GetComponent<WayPoint>().isLast) {
                selected = 0;
                changeDirection(waypoints[selected].gameObject.GetComponent<WayPoint>().direction);
                moving = true;
            }
            else {
                changeDirection(waypoints[selected].gameObject.GetComponent<WayPoint>().direction);
                selected += 1;
                moving = true;
            }
        }
    }

    private void changeDirection(string dir) {
        switch (dir) {
            case "up":
                direction = transform.forward;
                forwardSprite.SetActive(false);
                backwardSprite.SetActive(true);
                backwardSprite.transform.localScale = new Vector3(1, 1, 1);
                break;
            case "down":
                direction = -transform.forward;
                forwardSprite.SetActive(true);
                backwardSprite.SetActive(false);
                forwardSprite.transform.localScale = new Vector3(1, 1, 1);
                break;
            case "right":
                direction = transform.right;
                forwardSprite.SetActive(false);
                backwardSprite.SetActive(true);
                backwardSprite.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case "left":
                direction = -transform.right;
                forwardSprite.SetActive(true);
                backwardSprite.SetActive(false);
                forwardSprite.transform.localScale = new Vector3(-1, 1, 1);
                break;
            default:
                Debug.LogError("Invalid direction given to enemy: " + gameObject.name);
                break;
        }
        torch.SetDirection(dir);
    }

    public void ResetPos() {
        selected = 0;
        transform.position = startPos;
        moving = true;
    }
}