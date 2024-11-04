using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class VolumeManager : MonoBehaviour
{
    [Header("FMOD References")]

    [SerializeField]
    private GameObject PerameterTrigger;

    [SerializeField]
    private string ActivityVolName;

    [SerializeField]
    private string OneshotVolName;

    [Header("UI Elements")]

    [SerializeField]
    private Slider ActivityVolumeSlider;

    [SerializeField]
    private Slider OneshotVolumeSlider;

    [Header("Debug")]

    private float ActivityVolume;
    private float OneshotVolume;

    public float ActivityCurVolume;
    public float OneshotCurVolume;

    public void ActivityVolumeChange()
    {
        ActivityVolume = ActivityVolumeSlider.value;

        RuntimeManager.StudioSystem.setParameterByName(ActivityVolName, ActivityVolume);
        PerameterTrigger.GetComponent<StudioGlobalParameterTrigger>().TriggerParameters();
        RuntimeManager.StudioSystem.getParameterByName(ActivityVolName, out ActivityCurVolume);
    }

    public void OneshotVolumeChange()
    {
        OneshotVolume = OneshotVolumeSlider.value;

        RuntimeManager.StudioSystem.setParameterByName(OneshotVolName, OneshotVolume);
        PerameterTrigger.GetComponent<StudioGlobalParameterTrigger>().TriggerParameters();
        RuntimeManager.StudioSystem.getParameterByName(OneshotVolName, out OneshotCurVolume);
    }
}
