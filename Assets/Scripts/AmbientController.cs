using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicMixer.Ambience
{
    public class AmbientController : MonoBehaviour
    {
        public float FadeDuration;

        private float CurrentTime;
        private float FadeInVolume;
        private float FadeOutVolume;
        private float CurrentVolume;

        private bool FadeIn;

        public void TriggerAmbient()
        {
            AudioSource AmbientAudioSource = gameObject.GetComponent<AudioSource>();
            CurrentVolume = AmbientAudioSource.volume;

            if (!FadeIn) //triggers if fading in
            {
                FadeIn = true;
                AmbientAudioSource.Play();
            }

            else if (FadeIn) //triggers if fading out
            {
                FadeOutVolume = CurrentVolume;
                FadeIn = false;
            }
            CurrentTime = 0;
        }

        private void Update()
        {
            CurrentTime = CurrentTime + Time.deltaTime;     //sets current time and is used to measure how long the fades have been going on for.

            if (FadeIn)
            {
                //Fading in
                if (CurrentTime < FadeDuration)
                {
                    //Increases the Fade in Volume from 0 to 1 as the time approaches the fade duration.
                    FadeInVolume = CurrentTime / FadeDuration;

                    //sets the CurAmbient audiosource volume to FadeInVolume
                    gameObject.GetComponent<AudioSource>().volume = FadeInVolume;
                }
            }

            else
            {
                if (FadeOutVolume > 0)
                {
                    FadeOutVolume = CurrentVolume - (CurrentTime / FadeDuration);

                    gameObject.GetComponent<AudioSource>().volume = FadeOutVolume;
                }
            }
        }
    }
}
