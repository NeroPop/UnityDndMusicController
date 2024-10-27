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
    private float minValue;

    [SerializeField]
    private float maxValue;

    [SerializeField]
    private float curValue;

    [SerializeField]
    private StudioEventEmitter studioEventEmitter;

    public void Activity1()
    {
        studioEventEmitter.EventInstance.setParameterByName(variableName, 0);
        curValue = 0;
    }

    public void Activity2()
    {
        studioEventEmitter.EventInstance.setParameterByName(variableName, 45);
        curValue = 45;
    }

    public void Activity3()
    {
        studioEventEmitter.EventInstance.setParameterByName(variableName, 75);
        curValue = 75;
    }
}
