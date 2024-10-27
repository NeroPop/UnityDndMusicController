using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ActivityManager : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private GameObject ActivityPerameterObject;

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

    private bool playing = false;
    private int CurrentActivity;
    private float curValue;

    public void Activity1()
    {
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
        Debug.Log("Parameters set to " + curValue);
        Debug.Log("Playing Activity track " + CurrentActivity);
        Debug.Log("Music Playing = " + playing);
    }

    public void Activity2()
    {
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
        Debug.Log("Parameters set to " + curValue);
        Debug.Log("Playing Activity track " + CurrentActivity);
        Debug.Log("Music Playing = " + playing);
    }

    public void Activity3()
    {
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
        Debug.Log("Parameters set to " + curValue);
        Debug.Log("Playing Activity track " + CurrentActivity);
        Debug.Log("Music Playing = " + playing);
    }

    public void Stop()
    {
        if (playing)
        {
            studioEventEmitter.Stop();
            studioEventEmitter.gameObject.SetActive(false);
            playing = false;
        }
        Debug.Log("Music Playing = " + playing);
    }
}
