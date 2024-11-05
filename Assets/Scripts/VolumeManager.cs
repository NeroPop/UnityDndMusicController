using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("UI Elements")]

    [SerializeField]
    private Slider ActivityVolumeSlider;

    [SerializeField]
    private Slider OneshotVolumeSlider;

    [Header("Volume Mixers")]

    [SerializeField]
    private AudioMixerGroup MasterMixer;
    [SerializeField]
    private string MasterVolName;

    [SerializeField]
    private AudioMixerGroup ActivitiesMixer;
    [SerializeField]
    private string ActivitiesVolName;

    [SerializeField]
    private AudioMixerGroup OneshotsMixer;
    [SerializeField]
    private string OneshotsVolName;

    [Header("Debug")]

    private float MasterVolume;
    private float ActivityVolume;
    private float OneshotVolume;

    private float ActivityCurVolume;
    private float OneshotCurVolume;

    public void ActivityVolumeChange()
    {
        ActivityVolume = ActivityVolumeSlider.value;
        ActivitiesMixer.audioMixer.SetFloat(ActivitiesVolName, Mathf.Log10(ActivityVolume) * 20);
    }

    public void OneshotVolumeChange()
    {
        OneshotVolume = OneshotVolumeSlider.value;
        OneshotsMixer.audioMixer.SetFloat(OneshotsVolName, Mathf.Log10(OneshotVolume) * 20);
    }

    public void MasterVolumeChange()
    {
        //Insert Master Volume Slider code here if we need it.
    }
}