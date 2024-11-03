using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActivityManagerOld : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private string variableName;

    [SerializeField]
    private StudioEventEmitter studioEventEmitter;

    [Header("Activity Perameter Triggers")]

    [SerializeField]
    private GameObject ActivityTrigger1;

    [SerializeField]
    private GameObject ActivityTrigger2;

    [SerializeField]
    private GameObject ActivityTrigger3;

    [Header("Buttons")]

    [SerializeField]
    private Sprite ButtonDefaultSprite;

    [SerializeField]
    private Sprite ButtonSelectedSprite;

    [SerializeField]
    private GameObject ActivityButton1;

    [SerializeField]
    private GameObject ActivityButton2;

    [SerializeField]
    private GameObject ActivityButton3;


    private bool playing = false;

    private bool Act1 = false;
    private bool Act2 = false;
    private bool Act3 = false;

    public void Activity1()
    {
        if (!Act1)
        {
            ActivityTrigger1.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
            ActivityButton1.GetComponent<Image>().sprite = ButtonSelectedSprite;
            ActivityButton2.GetComponent<Image>().sprite = ButtonDefaultSprite;
            ActivityButton3.GetComponent<Image>().sprite = ButtonDefaultSprite;
            Act1 = true;
            Act2 = false;
            Act3 = false;

            if (!playing)
            {
                studioEventEmitter.Play();
                playing = true;
            }
        }
        else
        {
            Act1 = false;
            ActivityButton1.GetComponent<Image>().sprite = ButtonDefaultSprite;
            if (playing)
            {
                studioEventEmitter.Stop();
                playing = false;
            }
        }
    }

    public void Activity2()
    {
        if (!Act2)
        {
            ActivityTrigger2.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
            ActivityButton1.GetComponent<Image>().sprite = ButtonDefaultSprite;
            ActivityButton2.GetComponent<Image>().sprite = ButtonSelectedSprite;
            ActivityButton3.GetComponent<Image>().sprite = ButtonDefaultSprite;
            Act1 = false;
            Act2 = true;
            Act3 = false;

            if (!playing)
            {
                studioEventEmitter.Play();
                playing = true;
            }
        }
        else
        {
            Act2 = false;
            ActivityButton2.GetComponent<Image>().sprite = ButtonDefaultSprite;

            if (playing)
            {
                studioEventEmitter.Stop();
                playing = false;
            }
        }
    }

    public void Activity3()
    {
        if (!Act3)
        {
            ActivityTrigger3.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
            ActivityButton1.GetComponent<Image>().sprite = ButtonDefaultSprite;
            ActivityButton2.GetComponent<Image>().sprite = ButtonDefaultSprite;
            ActivityButton3.GetComponent<Image>().sprite = ButtonSelectedSprite;
            Act1 = false;
            Act2 = false;
            Act3 = true;

            if (!playing)
            {
                studioEventEmitter.Play();
                playing = true;
            }
        }
        else
        {
            Act3 = false;
            ActivityButton3.GetComponent<Image>().sprite = ButtonDefaultSprite;

            if (playing)
            {
                studioEventEmitter.Stop();
                playing = false;
            }
        }
    }

    public void Stop()
    {
        if (playing)
        {
            studioEventEmitter.Stop();
            playing = false;
        }
        Debug.Log("Music Playing = " + playing);
    }
}
