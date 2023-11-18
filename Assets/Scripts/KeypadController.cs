using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadController : MonoBehaviour
{
    public string code; //Also defines input length
    private string input;
    private bool interactable = true;
    public Text display;
    public Animation anim;
    private enum Message { Unlocked, Invalid }
    public PuzzleComplete pComplete;
    public AudioSource beep;

    private void UnlockComplete() {
        pComplete.Complete();
    }

    private void Unlock() {
        display.text = Message.Unlocked.ToString();
        anim.Play("KeypadUnlockAnim");
        Invoke("UnlockComplete", 1.7f);
    }

    private void Reject() {
        display.text = Message.Invalid.ToString();
        anim.Play("KeypadRejectAnim");
        Invoke("Clear", 1f);
    }

    private void Clear() {
        input = "";
        display.text = input;
        anim.Stop();
        display.color = new Color(0.8773585f, 0.8773585f, 0.8773585f);
        interactable = true;
    }

    public void CLR() {
        if (interactable) {
            Clear();
        }
    }
    public void One() {
        numberPressed(1);
    }
    public void Two()
    {
        numberPressed(2);
    }
    public void Three()
    {
        numberPressed(3);
    }
    public void Four()
    {
        numberPressed(4);
    }
    public void Five()
    {
        numberPressed(5);
    }
    public void Six()
    {
        numberPressed(6);
    }
    public void Seven()
    {
        numberPressed(7);
    }
    public void Eight()
    {
        numberPressed(8);
    }
    public void Nine()
    {
        numberPressed(9);
    }

    private void numberPressed(int num) {
        if (interactable) {
            input += num.ToString();
            display.text = input;
            beep.Play();
            if (input.Length == code.Length)
            {
                //Check if code matches
                if (input == code) {
                    //Code matches
                    interactable = false;
                    Invoke("Unlock", 0.3f);
                }
                else {
                    interactable = false;
                    Invoke("Reject", 0.3f);
                }

            }
        }
    }
}