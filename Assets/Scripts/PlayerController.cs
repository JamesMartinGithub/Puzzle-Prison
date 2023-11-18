using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject forwardSprite;
    public GameObject backwardSprite;
    public GameObject forwardIdleSprite;
    public GameObject backwardIdleSprite;
    private string direction;
    public Transform interactRayOrigin;
    private bool movementAllowed = true;
    RaycastHit hit;
    public GameObject spacebarPrompt;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && movementAllowed) {
            Interact();
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A)) {
            switch (direction) {
                case "up":
                    unwalk();
                    forwardIdleSprite.SetActive(false);
                    backwardIdleSprite.transform.localScale = new Vector3(1, 1, 1);
                    backwardIdleSprite.SetActive(true);
                    break;
                case "right":
                    unwalk();
                    forwardIdleSprite.SetActive(false);
                    backwardIdleSprite.transform.localScale = new Vector3(-1, 1, 1);
                    backwardIdleSprite.SetActive(true);
                    break;
                case "down":
                    unwalk();
                    backwardIdleSprite.SetActive(false);
                    forwardIdleSprite.transform.localScale = new Vector3(1, 1, 1);
                    forwardIdleSprite.SetActive(true);
                    break;
                case "left":
                    unwalk();
                    backwardIdleSprite.SetActive(false);
                    forwardIdleSprite.transform.localScale = new Vector3(-1, 1, 1);
                    forwardIdleSprite.SetActive(true);
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (movementAllowed && !(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) && !(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))) {
            bool diagonal = false;
            if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) | (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))) {
                diagonal = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                backwardSprite.transform.localScale = new Vector3(1, 1, 1);
                forwardSprite.SetActive(false);
                unidle();
                backwardSprite.SetActive(true);
                direction = "up";
                if (diagonal) {
                    transform.Translate(transform.forward * 0.06f);
                }
                else {
                    transform.Translate(transform.forward * 0.1f);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                forwardSprite.transform.localScale = new Vector3(1, 1, 1);
                backwardSprite.SetActive(false);
                unidle();
                forwardSprite.SetActive(true);
                direction = "down";
                if (diagonal)
                {
                    transform.Translate(-transform.forward * 0.06f);
                }
                else
                {
                    transform.Translate(-transform.forward * 0.1f);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (!Input.GetKey(KeyCode.S)) {
                    backwardSprite.transform.localScale = new Vector3(-1, 1, 1);
                    forwardSprite.SetActive(false);
                    unidle();
                    backwardSprite.SetActive(true);
                    direction = "right";
                }
                if (diagonal)
                {
                    transform.Translate(transform.right * 0.06f);
                }
                else
                {
                    transform.Translate(transform.right * 0.1f);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (!Input.GetKey(KeyCode.W)) {
                    forwardSprite.transform.localScale = new Vector3(-1, 1, 1);
                    backwardSprite.SetActive(false);
                    unidle();
                    forwardSprite.SetActive(true);
                    direction = "left";
                }
                if (diagonal)
                {
                    transform.Translate(-transform.right * 0.06f);
                }
                else
                {
                    transform.Translate(-transform.right * 0.1f);
                }
            }
        }
        if (SendInteractRay()) {
            spacebarPrompt.SetActive(true);
        } else {
            spacebarPrompt.SetActive(false);
        }
    }

    private void unwalk() {
        forwardSprite.SetActive(false);
        backwardSprite.SetActive(false);
    }

    private void unidle() {
        backwardIdleSprite.SetActive(false);
        forwardIdleSprite.SetActive(false);
    }

    private void Interact() {
        if (SendInteractRay()) {
            movementAllowed = false;
            hit.collider.gameObject.GetComponent<Interactable>().Interact();
        }
    }

    private bool SendInteractRay() {
        Vector3 dir = new Vector3();
        switch (direction)
        {
            case "up":
                dir = transform.forward;
                break;
            case "down":
                dir = -transform.forward;
                break;
            case "right":
                dir = transform.right;
                break;
            case "left":
                dir = -transform.right;
                break;
        }
        LayerMask mask = LayerMask.GetMask("Interaction");
        if (Physics.Raycast(interactRayOrigin.position, dir, out hit, 3f, mask))
        {
            return true;
        }
        else {
            return false;
        }
        
    }

    public void EnableMovement() {
        movementAllowed = true;
    }
}