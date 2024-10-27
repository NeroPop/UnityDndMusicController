using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;

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

    [Header("Activity Enabled")]

    [SerializeField]
    private UnityEvent On1;

    [SerializeField]
    private UnityEvent On2;

    [SerializeField]
    private UnityEvent On3;

    [Header("Activity Disabled")]

    [SerializeField]
    private UnityEvent Off1;

    [SerializeField]
    private UnityEvent Off2;

    [SerializeField]
    private UnityEvent Off3;

    private bool playing = false;

    private bool Act1 = false;
    private bool Act2 = false;
    private bool Act3 = false;

    public void Activity1()
    {
        if (!Act1)
        {
            ActivityTrigger1.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
            Act1 = true;
            On1.Invoke();
            playing = true;
            Playing();
        }
        else
        {
            Act1 = false;
            Off1.Invoke();
            playing = false;
            Playing();
        }
    }

    public void Activity2()
    {
        if (!Act2)
        {
            ActivityTrigger2.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
            Act2 = true;
            On2.Invoke();
            playing = true;
            Playing();
        }
        else
        {
            Act2 = false;
            Off2.Invoke();
            playing= false;
            Playing();
        }
    }

    public void Activity3()
    {
        if (!Act3)
        {
            ActivityTrigger3.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
            Act3 = true;
            On3.Invoke();
            playing = true;
            Playing();
        }
        else
        {
            Act3 = false;
            Off3.Invoke();
            playing= false;
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
