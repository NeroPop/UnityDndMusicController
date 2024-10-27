using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ActivityManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ActivityPerameterObject;

    [SerializeField]
    private string variableName;

    [SerializeField]
    private float curValue;

    [SerializeField]
    private StudioEventEmitter studioEventEmitter;

    [SerializeField]
    private int CurrentActivity;

    [Header("Activity Perameter Triggers")]

    [SerializeField]
    private GameObject ActivityTrigger1;

    [SerializeField]
    private GameObject ActivityTrigger2;

    [SerializeField]
    private GameObject ActivityTrigger3;

    private bool playing = false;

    public void Activity1()
    {
       // studioEventEmitter.EventInstance.setParameterByName(variableName, 10 / 100);

        curValue = 10;
        CurrentActivity = 1;

        ActivityTrigger1.gameObject.SetActive(true);
        ActivityTrigger2.gameObject.SetActive(false);
        ActivityTrigger3.gameObject.SetActive(false);

        if (!playing)
        {
            studioEventEmitter.Play();
            studioEventEmitter.gameObject.SetActive(true);
            playing = true;
        }
    }

    public void Activity2()
    {
        //studioEventEmitter.EventInstance.setParameterByName(variableName, 45 / 100);
        curValue = 45;
        CurrentActivity = 2;

        ActivityTrigger1.gameObject.SetActive(false);
        ActivityTrigger2.gameObject.SetActive(true);
        ActivityTrigger3.gameObject.SetActive(false);

        if (!playing)
        {
            studioEventEmitter.Play();
            studioEventEmitter.gameObject.SetActive(true);
            playing = true;
        }
    }

    public void Activity3()
    {
        //studioEventEmitter.EventInstance.setParameterByName(variableName, 75 / 100);
        curValue = 75;
        CurrentActivity = 3;

        ActivityTrigger1.gameObject.SetActive(false);
        ActivityTrigger2.gameObject.SetActive(false);
        ActivityTrigger3.gameObject.SetActive(true);

        if (!playing)
        {
            studioEventEmitter.Play();
            studioEventEmitter.gameObject.SetActive(true);
            playing = true;
        }
    }

    public void Stop()
    {
        if (playing)
        {
            studioEventEmitter.Stop();
            studioEventEmitter.gameObject.SetActive(false);
            playing = false;
        }
    }
}
