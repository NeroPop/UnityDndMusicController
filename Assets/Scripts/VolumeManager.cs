using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("UI Elements")]

    [SerializeField]
    private Slider ActivityVolumeSlider;

    [SerializeField]
    private Slider AmbienceVolumeSlider;

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
    private AudioMixerGroup AmbienceMixer;
    [SerializeField]
    private string AmbienceVolName;

    [SerializeField]
    private AudioMixerGroup OneshotsMixer;
    [SerializeField]
    private string OneshotsVolName;

    [Header("Debug")]

    private float MasterVolume;
    private float ActivityVolume;
    private float AmbienceVolume;
    private float OneshotVolume;

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

    public void AmbienceVolumeChange()
    {
        AmbienceVolume = AmbienceVolumeSlider.value;
        AmbienceMixer.audioMixer.SetFloat(AmbienceVolName, Mathf.Log10(AmbienceVolume) * 20);
    }
}