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
        [SerializeField] private Slider AmbienceFadeSlider;
        public float ActivityFadeDuration;
        public float AmbienceFadeDuration;

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
            AmbienceFadeDuration = data.AmbienceFadeDuration;

            UpdateFadeValues();
        }

        public void ActivityFadeUpdate() //Called when the slider value changes
        {
            //Updates the fade duration based on the current slider value.
            ActivityFadeDuration = ActivityFadeSlider.value / 10; //Worth noting I've devided the value by 10 as the slider's max value is set to 100 and we want integers of 0.1
            GetComponent<UnityActivityManager>().FadeDuration = ActivityFadeDuration;
        }

        public void AmbienceFadeUpdate() //Called when the slider value changes
        {
            //Updates the fade duration based on the current slider value.
            AmbienceFadeDuration = AmbienceFadeSlider.value / 10; //Worth noting I've devided the value by 10 as the slider's max value is set to 100 and we want integers of 0.1
            GetComponent<AmbienceManager>().FadeDuration = AmbienceFadeDuration;
            GetComponent<AmbienceManager>().AmbientFadeUpdate();
        }

        private void UpdateFadeValues()
        {
            //Sets The activity fade duration and audio slider value to the current fade duration
            GetComponent<UnityActivityManager>().FadeDuration = ActivityFadeDuration;
            ActivityFadeSlider.value = ActivityFadeDuration * 10;

            //Sets The ambience fade duration and audio slider value to the current fade duration
            GetComponent<AmbienceManager>().FadeDuration = AmbienceFadeDuration;
            AmbienceFadeSlider.value = AmbienceFadeDuration * 10;
            GetComponent<AmbienceManager>().AmbientFadeUpdate();
        }
    }
}
