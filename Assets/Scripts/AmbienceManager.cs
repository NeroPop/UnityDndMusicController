using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] AmbientAudioSources;

    public float FadeDuration = 5;

    [SerializeField]
    private int CurAmbient;

    [SerializeField]
    private float CurrentTime;

    [SerializeField]
    private float CurVolume;

    [SerializeField]
    private float FadeInVolume;

    [SerializeField]
    private float FadeOutVolume;

    [SerializeField]
    private bool FadeIn = false;

    public void PlayAmbientAudio(int AmbientAudio)
    {
        CurAmbient = AmbientAudio;

        //Fades Out
        if (AmbientAudioSources[AmbientAudio].volume > 0) //Checks if the volume is above 0
        {
            CurVolume = AmbientAudioSources[AmbientAudio].volume;
            FadeOutVolume = CurVolume;
            FadeIn = false;
            CurrentTime = 0;
        }

        //Fades In
        else
        {
            CurVolume = AmbientAudioSources[AmbientAudio].volume;
            FadeIn = true;
            CurrentTime = 0;
        }
    }

    private void Update()
    {
        //sets current time and is used to measure how long the fades have been going on for.
        CurrentTime = CurrentTime + Time.deltaTime;

        //Fading in
        if (CurrentTime < FadeDuration && FadeIn)
        {
            //Increases the Fade in Volume from 0 to 1 as the time approaches the fade duration.
            FadeInVolume = CurrentTime / FadeDuration;

            //sets the CurAmbient audiosource volume to FadeInVolume
            AmbientAudioSources[CurAmbient].volume = FadeInVolume;
        }

        //Fading out
        if (FadeOutVolume > 0 && !FadeIn)
        {
            FadeOutVolume = CurVolume - (CurrentTime / FadeDuration);

            AmbientAudioSources[CurAmbient].volume = FadeOutVolume;
        }
    }
}
