using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinationController : MonoBehaviour
{
    public string password;
    public string[] dialLetters = new string[4];
    private char[] dialSelected = new char[4];
    private float[] lerps = {0, 0, 0, 0};
    private int[] currentRots = {0, 0, 0, 0};
    public float dialSpinTime;
    public Animation anim;
    public Transform[] dials;
    private float[] froms = new float[4];
    private float[] tos = new float [4];
    public Text[] texts1 = new Text[5];
    public Text[] texts2 = new Text[5];
    public Text[] texts3 = new Text[5];
    public Text[] texts4 = new Text[5];
    private bool interactable = true;
    public PuzzleComplete pComplete;

    private void Start()
    {
        for (int i = 0; i < 4; i++) {
            dialSelected[i] = dialLetters[i][0];
        }
        for (int e = 0; e < 5; e++)
        {
            texts1[e].text = dialLetters[0][e].ToString();
            texts2[e].text = dialLetters[1][e].ToString();
            texts3[e].text = dialLetters[2][e].ToString();
            texts4[e].text = dialLetters[3][e].ToString();
        }
    }

    private void combinationComplete() {
        pComplete.Complete();
    }

    private void playAnimation() {
        anim.Play("CombinationUnlockAnim");
        Invoke("combinationComplete", 0.8f);
    }

    public void checkCombination() {
        string attempt = dialSelected[0].ToString() + dialSelected[1].ToString() + dialSelected[2].ToString() + dialSelected[3].ToString();
        if (attempt == password) {
            interactable = false;
            Invoke("playAnimation", dialSpinTime);
        }
    }

    private void FixedUpdate()
    {
        if (lerps[0] > 0) {
            lerps[0] -= 0.02f;
            dials[0].localEulerAngles = new Vector3(Mathf.SmoothStep(froms[0], tos[0], 1 - (lerps[0] / dialSpinTime)), 0, 0);
        }
        if (lerps[1] > 0)
        {
            lerps[1] -= 0.02f;
            dials[1].localEulerAngles = new Vector3(Mathf.SmoothStep(froms[1], tos[1], 1 - (lerps[1] / dialSpinTime)), 0, 0);
        }
        if (lerps[2] > 0)
        {
            lerps[2] -= 0.02f;
            dials[2].localEulerAngles = new Vector3(Mathf.SmoothStep(froms[2], tos[2], 1 - (lerps[2] / dialSpinTime)), 0, 0);
        }
        if (lerps[3] > 0)
        {
            lerps[3] -= 0.02f;
            dials[3].localEulerAngles = new Vector3(Mathf.SmoothStep(froms[3], tos[3], 1 - (lerps[3] / dialSpinTime)), 0, 0);
        }
    }

    public void upOne() {
        dialSelected[0] = cycleLetters(0, false);
        spinDialUp(0);
    }
    public void downOne()
    {
        dialSelected[0] = cycleLetters(0, true);
        spinDialDown(0);
    }
    public void upTwo()
    {
        dialSelected[1] = cycleLetters(1, false);
        spinDialUp(1);
    }
    public void downTwo()
    {
        dialSelected[1] = cycleLetters(1, true);
        spinDialDown(1);
    }
    public void upThree()
    {
        dialSelected[2] = cycleLetters(2, false);
        spinDialUp(2);
    }
    public void downThree()
    {
        dialSelected[2] = cycleLetters(2, true);
        spinDialDown(2);
    }
    public void upFour()
    {
        dialSelected[3] = cycleLetters(3, false);
        spinDialUp(3);
    }
    public void downFour()
    {
        dialSelected[3] = cycleLetters(3, true);
        spinDialDown(3);
    }

    private void spinDialUp(int n) {
        if (interactable)
        {
            froms[n] = Mathf.SmoothStep(froms[n], tos[n], 1 - (lerps[n] / dialSpinTime));
            currentRots[n] -= 72;
            tos[n] = currentRots[n];
            lerps[n] = dialSpinTime;
            checkCombination();
        }
    }
    private void spinDialDown(int n)
    {
        if (interactable) {
            froms[n] = Mathf.SmoothStep(froms[n], tos[n], 1 - (lerps[n] / dialSpinTime));
            currentRots[n] += 72;
            tos[n] = currentRots[n];
            lerps[n] = dialSpinTime;
            checkCombination();
        }
    }

    private char cycleLetters(int dial, bool reverse) {
        if (interactable) {
            string ls = dialLetters[dial];
            int index = ls.IndexOf(dialSelected[dial]);
            if (!reverse) {
                if (index == ls.Length - 1) {
                    index = 0;
                }
                else {
                    index += 1;
                }
            }
            else {
                if (index == 0) {
                    index = ls.Length - 1;
                }
                else {
                    index -= 1;
                }
            }
            return ls[index];
        }
        else {
            return 'a';
        }
    }
}