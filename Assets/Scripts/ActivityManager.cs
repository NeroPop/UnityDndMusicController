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
    [Tooltip("FMOD Parameter GameObject with the desired Volume variable to change.")]
    private GameObject ActivityTrigger1;

    [SerializeField]
    [Tooltip("Name of the Track 1 volume variable to change. Case sensitive.")]
    private string Track1VolName;

    [SerializeField]
    [Tooltip("FMOD Parameter GameObject with the desired Volume variable to change.")]
    private GameObject ActivityTrigger2;

    [SerializeField]
    [Tooltip("Name of the Track 2 volume variable to change. Case sensitive.")]
    private string Track2VolName;

    [SerializeField]
    [Tooltip("FMOD Parameter GameObject with the desired Volume variable to change.")]
    private GameObject ActivityTrigger3;

    [SerializeField]
    [Tooltip("Name of the Track 3 volume variable to change. Case sensitive.")]
    private string Track3VolName;

    [Header("Volumes")]

    [SerializeField]
    private GameObject Track1Vol;

    [SerializeField]
    private GameObject Track2Vol;

    [SerializeField]
    private GameObject Track3Vol;

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

    //hidden variables

    private bool playing = false;

    private bool Act1 = false;
    private bool Act2 = false;
    private bool Act3 = false;

    //fade control

    private float CurrentTime;
    public float FadeTime;
    public bool Fade = true;


    private void Update()
    {
        CurrentTime = CurrentTime + Time.deltaTime;
    }

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

            CurrentTime = 0;
            ActivityTrigger1.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
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
