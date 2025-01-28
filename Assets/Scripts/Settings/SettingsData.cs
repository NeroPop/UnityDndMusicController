using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicMixer;

[System.Serializable]
public class SettingsData
{
    public float ActivityFadeDuration;
    public float AmbienceFadeDuration;

    public string SceneName;

    public SettingsData (SettingsManager manager)
    {
        ActivityFadeDuration = manager.ActivityFadeDuration;
        AmbienceFadeDuration = manager.AmbienceFadeDuration;

        SceneName = manager.SceneName;
    }
}
