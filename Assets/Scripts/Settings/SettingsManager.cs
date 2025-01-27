using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MusicMixer.Activities;
using MusicMixer.Ambience;
using System.Runtime.Remoting.Messaging;

namespace MusicMixer
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("Fading")]
        [SerializeField] private Slider ActivityFadeSlider;

        public float ActivityFadeDuration;

        [Header("Other")]
        public string SceneName;

        private void Start()
        {
            LoadSettings();
        }

        public void SaveSettings()
        {
            SaveSystem.SaveSettings(this);
            UpdateFadeValues();
        }

        public void LoadSettings()
        {
            SettingsData data = SaveSystem.LoadSettings(SceneName);

            ActivityFadeDuration = data.ActivityFadeDuration;

            UpdateFadeValues();
        }

        public void ActivityFadeUpdate() //Called when the slider value changes
        {
            //Updates the fade duration based on the current slider value.
            ActivityFadeDuration = ActivityFadeSlider.value / 10; //Worth noting I've devided the value by 10 as the slider's max value is set to 100 and we want integers of 0.1
            GetComponent<UnityActivityManager>().FadeDuration = ActivityFadeDuration;
        }

        private void UpdateFadeValues()
        {
            //Sets The activity fade duration to the current fade duration
            GetComponent<UnityActivityManager>().FadeDuration = ActivityFadeDuration;

            //Sets the Activity Fade slider to the Fade duration value
            ActivityFadeSlider.value = ActivityFadeDuration * 10;
        }
    }
}
