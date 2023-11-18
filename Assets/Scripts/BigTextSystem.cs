using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigTextSystem : MonoBehaviour
{
    public Text text;
    private string[] paragraphs;
    private int selected;
    private int selectedC;
    public float timeToWrite;
    public Animation blackoutAnim;
    public MeshRenderer prisonModel;

    private void RenderLine()
    {
        if (selectedC < paragraphs[selected].Length)
        {
            text.text = text.text + paragraphs[selected][selectedC];
            selectedC += 1;
            Invoke("RenderLine", timeToWrite / paragraphs[selected].Length);
        }
        else
        {
            selected += 1;
            RenderLines();
        }
    }

    private void RenderLines()
    {
        if (selected < paragraphs.Length)
        {
            if (paragraphs[selected] == "")
            {
                selected += 1;
                Invoke("ClearNRenders", 2f);
            }
            else
            {
                if (selected == 0)
                {
                    selectedC = 0;
                    Invoke("ClearNRender", 0.2f);
                }
                else
                {
                    selectedC = 0;
                    Invoke("ClearNRender", 2f);
                }
            }
        }
        else
        {
            Invoke("ClearNFade", 3);
        }
    }

    public void PlayBigText(string[] paras)
    {
        selected = 0;
        paragraphs = paras;
        text.text = "";
        blackoutAnim.Play("BlackoutFadeIn");
        prisonModel.enabled = false;
        Invoke("RenderLines", 0.7f);
    }

    private void ClearNRender()
    {
        text.text = "";
        RenderLine();
    }
    private void ClearNRenders()
    {
        text.text = "";
        RenderLines();
    }

    private void ClearNFade()
    {
        blackoutAnim.Play("BlackoutFadeOut");
        Clear();
        //Invoke("Clear", 0.1f);
    }

    private void Clear() {
        text.text = "";
        GameObject.Find("Wall Camera").GetComponent<Camera>().enabled = true;
        prisonModel.enabled = true;
    }
}