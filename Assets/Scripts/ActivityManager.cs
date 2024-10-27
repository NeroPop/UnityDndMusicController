using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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

    private bool playing = false;

    public void Activity1()
    {
        ActivityTrigger1.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();

        if (!playing)
        {
            studioEventEmitter.Play();
            playing = true;
            Debug.Log("Music Playing = " + playing);
        }
    }

    public void Activity2()
    {
        ActivityTrigger2.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();

        if (!playing)
        {
            studioEventEmitter.Play();
            playing = true;
            Debug.Log("Music Playing = " + playing);
        }
    }

    public void Activity3()
    {
        ActivityTrigger3.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();

        if (!playing)
        {
            studioEventEmitter.Play();
            playing = true;
            Debug.Log("Music Playing = " + playing);
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
