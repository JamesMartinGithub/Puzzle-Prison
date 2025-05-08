using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockpickController : MonoBehaviour
{
    public GameObject buttons;
    public RectTransform lockpickTransform;
    public RectTransform lockpickPivot;
    public RectTransform pivotRotAdjust;
    public RectTransform pivotPosAdjust;
    public RectTransform[] pinTransforms;
    public Animation TensionWrenchAnim;
    private Vector3 lockpickInitPos;
    private float lockpickStart;
    private float lockpickEnd;
    private float pivotStart = 0;
    private float pivotEnd;
    public float pickMoveTime; //In seconds
    private float lerpTime = 0;
    private float lerpTimeR = 0;
    private float lerpTimeP = 0;
    private float lerpTimePickExit = 0;
    public float pickExitTime;
    private int currentPin = 3;
    private int[] pinHeights = {0, 0, 0, 0, 0, 0};
    private float clickPointY;
    private float currentPointY;
    private float[] pinStarts;
    private float[] pinEnds;
    private float[] pinInitPosXs;
    private bool pinRaised = false;
    private bool interactable = true;
    public int[] targetHeights = {0, 0, 0, 0, 0, 0};
    public bool[] enabledPins = {true, true, true, true, true, true};
    private int enabledNumber = 6;
    public PuzzleComplete pComplete;

    void Start()
    {
        lockpickInitPos = lockpickTransform.localPosition;
        lockpickStart = lockpickInitPos.x;
        lockpickEnd = lockpickInitPos.x;
        pinStarts = new float[6];
        pinEnds = new float[6];
        pinInitPosXs = new float[6];
        for (int i = 0; i < 6; i++) {
            pinInitPosXs[i] = pinTransforms[i].localPosition.x;
            //Disable uneeded pins
            if (enabledPins[i] == false) {
                pinTransforms[i].gameObject.SetActive(false);
                enabledNumber -= 1;
            }
        }
    }

    private void pickComplete() {
        pComplete.Complete();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && interactable) {
            buttons.SetActive(false);
            clickPointY = Input.mousePosition.y;
            currentPointY = clickPointY;
        }
        if (Input.GetKey(KeyCode.Mouse0) && interactable) {
            currentPointY = Mathf.Max(currentPointY, Input.mousePosition.y);
            float difference = currentPointY - clickPointY;
            switch (difference) {
                case float i when i >= 0 && i < 100:
                    float dynamicDifference = Mathf.Clamp(Input.mousePosition.y - clickPointY, 0, 200);
                    pivotRotAdjust.localEulerAngles = new Vector3(0, 0, Mathf.SmoothStep(0f, -0.5f, dynamicDifference / 200f));
                    float pinInitY = pinHeights[currentPin - 1] * 50;
                    if (lerpTimeP <= 0) {
                        pinTransforms[currentPin - 1].localPosition = new Vector3(pinInitPosXs[currentPin - 1], Mathf.SmoothStep(pinInitY, pinInitY + 10, dynamicDifference / 200f), 0);
                    }
                    break;
                case float i when i >= 100:
                    int currentVal = pinHeights[currentPin - 1];
                    if (currentVal < 4) {
                        clickPointY = clickPointY + 100f;
                        currentPointY = clickPointY;
                        pivotEnd = getAngle(currentVal + 1);
                        if (lockpickPivot.localEulerAngles.z > 180)
                        {
                            pivotStart = (lockpickPivot.localEulerAngles.z - 360) - 0.5f;
                        }
                        else
                        {
                            pivotStart = lockpickPivot.localEulerAngles.z - 0.5f;
                        }
                        lerpTimeR = pickMoveTime;
                        pivotRotAdjust.localEulerAngles = new Vector3(0, 0, 0);
                        lockpickPivot.localEulerAngles = new Vector3(0, 0, pivotStart);
                        pinRaise(currentPin - 1);
                        pinRaised = true;
                    }
                    break;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && interactable) {
            pivotRotAdjust.localEulerAngles = new Vector3(0, 0, 0);
            if (pinRaised) {
                pinsFall(currentPin - 1);
                pinRaised = false;
            }
            if (lerpTimeP <= 0) {
                pinTransforms[currentPin - 1].localPosition = new Vector3(pinInitPosXs[currentPin - 1], pinHeights[currentPin - 1] * 50, 0);
            }
            if (interactable) {
                buttons.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (lerpTime > 0) {
            lerpTime -= 0.02f;
            lockpickTransform.localPosition = new Vector3(Mathf.SmoothStep(lockpickStart, lockpickEnd, 1 - (lerpTime / pickMoveTime)), lockpickInitPos.y, 0);
        }
        if (lerpTimeR > 0) {
            lerpTimeR -= 0.02f;
            lockpickPivot.localEulerAngles = new Vector3(0, 0, Mathf.SmoothStep(pivotStart, pivotEnd, 1 - (lerpTimeR / pickMoveTime)));
            float currentAngle = 0;
            if (lockpickPivot.localEulerAngles.z > 180)
            {
                currentAngle = lockpickPivot.localEulerAngles.z - 360;
            }
            else {
                currentAngle = lockpickPivot.localEulerAngles.z;
            }
            if (currentAngle < -4)
            {
                pivotPosAdjust.localPosition = new Vector3(Mathf.SmoothStep(0, 74, ((currentAngle + 4) / -12.2f)), 0, 0);
            }
            else {
                pivotPosAdjust.localPosition = new Vector3(0, 0, 0);
            }
        }
        if (lerpTimeP > 0) {
            lerpTimeP -= 0.02f;
            for (int i = 0; i < 6; i++) {
                pinTransforms[i].localPosition = new Vector3(pinInitPosXs[i], Mathf.SmoothStep(pinStarts[i], pinEnds[i], 1 - (lerpTimeP / pickMoveTime)), 0);
            }
        }
        //Pick exit lerp
        if (lerpTimePickExit > 0) {
            lerpTimePickExit -= 0.02f;
            lockpickTransform.localPosition = new Vector3(Mathf.SmoothStep(lockpickEnd, 1800f, 1 - (lerpTimePickExit / pickExitTime)), lockpickInitPos.y, 0);
            lockpickPivot.localEulerAngles = new Vector3(0, 0, Mathf.SmoothStep(pivotEnd, -0.1f, 1 - (lerpTimePickExit / pickExitTime)));
        }
    }

    public void pinRaise(int raisedPinIndex) {
        //Check if unclick animation needed
        if (pinHeights[raisedPinIndex] == targetHeights[raisedPinIndex]) {
            pinTransforms[raisedPinIndex].gameObject.GetComponent<Animation>().Play("PinUnclickAnim");
        }
        for (int i = 0; i < 6; i++)
        {
            pinStarts[i] = pinHeights[i] * 50;
            if (raisedPinIndex == i) {
                pinStarts[i] = pinTransforms[i].localPosition.y;
                pinHeights[i] = pinHeights[i] + 1;
            }
            pinEnds[i] = pinHeights[i] * 50;
        }
        lerpTimeP = pickMoveTime;
        //Check if click animation needed
        if (pinHeights[raisedPinIndex] == targetHeights[raisedPinIndex]) {
            pinTransforms[raisedPinIndex].gameObject.GetComponent<Animation>().Play("PinClickAnim");
        }
    }

    public void pinsFall(int raisedPinIndex) {
        int correctPosCounter = 0;
        for (int i = 0; i < 6; i++) {
            if (raisedPinIndex == i) {
                pinStarts[i] = pinTransforms[i].localPosition.y;
            } else {
                pinStarts[i] = pinHeights[i] * 50;
            }
            if (raisedPinIndex != i && pinHeights[i] > -4) {
                //Check if unclick animation needed
                if (pinHeights[i] == targetHeights[i])
                {
                    pinTransforms[i].gameObject.GetComponent<Animation>().Play("PinUnclickAnim");
                }
                pinHeights[i] = pinHeights[i] - 1;
                //Check if click animation needed
                if (pinHeights[i] == targetHeights[i]) {
                    pinTransforms[i].gameObject.GetComponent<Animation>().Play("PinClickAnim");
                }
            }
            pinEnds[i] = pinHeights[i] * 50;
            if (enabledPins[i] && pinHeights[i] == targetHeights[i]) {
                correctPosCounter += 1;
            }
        }
        lerpTimeP = pickMoveTime;
        //Check if all at correct height
        if (correctPosCounter == enabledNumber) {
            interactable = false;
            buttons.SetActive(false);
            lerpTimePickExit = pickExitTime;
            TensionWrenchAnim.Play("TensionWrenchExitAnim");
            Invoke("pickComplete", 1.2f);
        }
    }

    public void a1() {
        if (enabledPins[0]) {
            MovePick(1);
            lockpickEnd = 0;
            pivotEnd = getAngle(pinHeights[0]);
        }
    }
    public void a2()
    {
        if (enabledPins[1]) {
            MovePick(2);
            lockpickEnd = 200;
            pivotEnd = getAngle(pinHeights[1]);
        }
    }
    public void a3()
    {
        if (enabledPins[2]) {
            MovePick(3);
            lockpickEnd = 400;
            pivotEnd = getAngle(pinHeights[2]);
        }
    }
    public void a4()
    {
        if (enabledPins[3]) {
            MovePick(4);
            lockpickEnd = 600;
            pivotEnd = getAngle(pinHeights[3]);
        }
    }
    public void a5()
    {
        if (enabledPins[4]) {
            MovePick(5);
            lockpickEnd = 800;
            pivotEnd = getAngle(pinHeights[4]);
        }
    }
    public void a6()
    {
        if (enabledPins[5]) {
            MovePick(6);
            lockpickEnd = 1000;
            pivotEnd = getAngle(pinHeights[5]);
        }
    }

    private void MovePick(int i) {
        lerpTime = pickMoveTime;
        lerpTimeR = pickMoveTime;
        lockpickStart = lockpickTransform.localPosition.x;
        if (lockpickPivot.localEulerAngles.z > 180)
        {
            pivotStart = lockpickPivot.localEulerAngles.z - 360;
        }
        else {
            pivotStart = lockpickPivot.localEulerAngles.z;
        }
        currentPin = i;
    }

    private float getAngle(int height) {
        switch (height) {
            case 4:
                return -16.1f;
            case 3:
                return -14.1f;
            case 2:
                return -12f;
            case 1:
                return -10f;
            case 0:
                return -8f;
            case -1:
                return -5.95f;
            case -2:
                return -3.95f;
            case -3:
                return -1.95f;
            case -4:
                return 0f;
        }
        return 0f;
    }
}
