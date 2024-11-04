using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;
using System;

public class OneShotManager : MonoBehaviour
{
    [Header("FMOD References")]

    [SerializeField]
    private StudioEventEmitter studioEventEmitter;

    [SerializeField]
    private GameObject PerameterTrigger;

    [SerializeField]
    public string OneShotPeramName;

    [SerializeField]
    private float CurOneshot;

    public void playOneShot(int OneShotNumber)
    {
        RuntimeManager.StudioSystem.setParameterByName(OneShotPeramName, OneShotNumber);
        PerameterTrigger.GetComponent<StudioGlobalParameterTrigger>().TriggerParameters();
        studioEventEmitter.Play();
        RuntimeManager.StudioSystem.getParameterByName(OneShotPeramName, out CurOneshot);
        Debug.Log("Playing OneShot " + CurOneshot);
    }
}
