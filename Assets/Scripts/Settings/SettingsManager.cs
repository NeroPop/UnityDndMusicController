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

        private void Start()
        {
            //Sets the Activity Fade slider to whatever value we've assigned FadeDuration in editor
            ActivityFadeSlider.value = GetComponent<UnityActivityManager>().FadeDuration * 10;
        }

        public void ActivityFadeUpdate() //Called when the slider value changes
        {
            //Updates the fade duration based on the current slider value.
            float value = ActivityFadeSlider.value;
            GetComponent<UnityActivityManager>().FadeDuration = (value / 10); //Worth noting I've devided the value by 10 as the slider's max value is set to 100 and we want integers of 0.1
        }
    }
}
