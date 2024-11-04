using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class OneShotManager : MonoBehaviour
{
    [Header("FMOD References")]

    [SerializeField]
    private StudioEventEmitter studioEventEmitter;

    [SerializeField]
    private GameObject PerameterTrigger;

    [SerializeField]
    public string OneShotVarName;
}
