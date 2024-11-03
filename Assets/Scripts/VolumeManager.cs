using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class VolumeManager : MonoBehaviour
{
    [SerializeField]
    private Slider ActivityVolumeSlider;

    [SerializeField]
    private GameObject PerameterTrigger;

    [SerializeField]
    private string ActivityVolName;

    private float ActivityVolume;
    public float ActivityCurVolume;

    public void ActivityVolumeChange()
    {
        ActivityVolume = ActivityVolumeSlider.value;

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(ActivityVolName, ActivityVolume);
        PerameterTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
        FMODUnity.RuntimeManager.StudioSystem.getParameterByName(ActivityVolName, out ActivityCurVolume);
    }
}
