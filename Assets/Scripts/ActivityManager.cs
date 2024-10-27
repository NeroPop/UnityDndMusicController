using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActivityManager : MonoBehaviour
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

    public bool Act1 = false;
    public bool Act2 = false;
    public bool Act3 = false;

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
            playing = true;
            Playing();
        }
        else
        {
            Act1 = false;
            ActivityButton1.GetComponent<Image>().sprite = ButtonDefaultSprite;
            playing = false;
            Playing();
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
            playing = true;
            Playing();
        }
        else
        {
            Act2 = false;
            ActivityButton2.GetComponent<Image>().sprite = ButtonDefaultSprite;
            playing = false;
            Playing();
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
            playing = true;
            Playing();
        }
        else
        {
            Act3 = false;
            ActivityButton3.GetComponent<Image>().sprite = ButtonDefaultSprite;
            playing = false;
            Playing();
        }
    }

    private void Playing()
    {
        if (playing)
        {
            studioEventEmitter.Play();     }
        else
        {
            studioEventEmitter.Stop();
        }
        Debug.Log("Music Playing = " + playing);
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
