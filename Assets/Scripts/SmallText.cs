using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallText : MonoBehaviour
{
    private Text text;
    private string[] paragraphs;
    private int selected;
    private int selectedC;
    public float timeToWrite;
    void Start()
    {
        //text = GameObject.Find("Small Text").GetComponent<Text>();
        //RenderText(new string[4] { "This is a test yo! oooooooooo", "Second para!?", "", "Final para..." });
    }

    private void RenderLine() {
        if (selectedC < paragraphs[selected].Length) {
            text.text = text.text + paragraphs[selected][selectedC];
            selectedC += 1;
            Invoke("RenderLine", timeToWrite / paragraphs[selected].Length);
        }
        else {
            selected += 1;
            RenderLines();
        }
    }

    private void RenderLines() {
        if (selected < paragraphs.Length) {
            if (paragraphs[selected] == "") {
                selected += 1;
                Invoke("ClearNRenders", 3f);
            }
            else {
                if (selected == 0) {
                    selectedC = 0;
                    Invoke("ClearNRender", 0.2f);
                }
                else {
                    selectedC = 0;
                    Invoke("ClearNRender", 3f);
                }
            }
        }
        else {
            Invoke("ClearNDestroy", 3);
        }
    }

    public void RenderText(string[] paras) {
        selected = 0;
        paragraphs = paras;
        text = GameObject.Find("Small Text").GetComponent<Text>();
        text.text = "";
        RenderLines();
    }

    private void ClearNRender() {
        text.text = "";
        RenderLine();
    }
    private void ClearNRenders()
    {
        text.text = "";
        RenderLines();
    }

    private void ClearNDestroy() {
        text.text = "";
        Destroy(gameObject, 0);
    }
}