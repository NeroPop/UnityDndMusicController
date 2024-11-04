using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UnityActivityManager : MonoBehaviour
{
    [Header("Audio Sources")]

    [SerializeField]
    private AudioSource ExploreAudio;

    [SerializeField]
    private AudioSource CombatAudio;

    [SerializeField]
    private AudioSource VictoryAudio;

    [Header("Audio Mixers")]

    public AudioMixerGroup[] ActivityMixers;

    public string[] MixerVolNames;

    [Header("Buttons")]

    [SerializeField]
    private Sprite ButtonDefaultSprite;

    [SerializeField]
    private Sprite ButtonSelectedSprite;

    [SerializeField]
    private GameObject[] ActivityButtons;

    //[Header("Fade")]
    //public float FadeDuration = 5;

    [Header("Debugging")]

    private bool playing = false;

    public int Act = 0;
    public int PrevAct;

    public float CurrentTime;
    public float FadeDuration = 5;
    private float VolumeLevel;
    private float PreVolumeLevel;
    public float FadeoutVolume = 0;
    private float CurFadeTime;


    public void TriggerActivity(int ActivityNumber)
    {
        PrevAct = Act;
        PreVolumeLevel = VolumeLevel;

        //Runs if the button hasn't already been pressed
        if (Act != ActivityNumber)
        {
            //sets the current activity
            Act = ActivityNumber;
            playing = true;

            //Resets fading
            CurrentTime = 0;
            FadeoutVolume = FadeDuration;
            CurFadeTime = PreVolumeLevel;

            //changes the button sprites
            ActivityButtons[Act-1].GetComponent<Image>().sprite = ButtonSelectedSprite;

            //ensures the erray isn't out of bounds
            if (PrevAct > 0)
            {
                ActivityButtons[PrevAct - 1].GetComponent<Image>().sprite = ButtonDefaultSprite;
            }
            Debug.Log("Playing activity " + ActivityNumber);
        }

        //Runs if the button has already been pressed
        else
        {
            ActivityButtons[Act-1].GetComponent<Image>().sprite = ButtonDefaultSprite;
            playing = false;
            Act = 0;

            CurrentTime = 0;
            FadeoutVolume = FadeDuration;
            CurFadeTime = PreVolumeLevel;
        }
    }

    private void Update()
    {
        //sets current time and is used to measure how long the fades have been going on for.
        CurrentTime = CurrentTime + Time.deltaTime;

        //checks if activity is playing or not
        if (Act > 0)
        {
            //checks if the music should be fading in or not.
            if (CurrentTime < FadeDuration)
            {
                //VolumeLevel sets the volume between 0-1, going up as time gets closer to fade duration.
                VolumeLevel = CurrentTime / FadeDuration;

                //Sets the activity volume to VolumeLevel
                ActivityMixers[Act - 1].audioMixer.SetFloat(MixerVolNames[Act-1], Mathf.Log10(VolumeLevel) * 20);
            }

            if (FadeoutVolume > 0)
            {
                //Sets FadeoutVolume to the opposite of that of VolumeLevel, also taking the current fade time into account.
                FadeoutVolume = (CurFadeTime - (CurrentTime / FadeDuration));

                //sets the previous activity volume to FadeoutVolume which decreases over time.
                ActivityMixers[PrevAct - 1].audioMixer.SetFloat(MixerVolNames[PrevAct - 1], Mathf.Log10(FadeoutVolume) * 20);
            }
        }

        else if (Act == 0)
        {
            if (FadeoutVolume > 0)
            {
                //Sets FadeoutVolume to the opposite of that of VolumeLevel, also taking the current fade time into account.
                FadeoutVolume = (CurFadeTime - (CurrentTime / FadeDuration));

                //sets the previous activity volume to FadeoutVolume which decreases over time.
                ActivityMixers[PrevAct - 1].audioMixer.SetFloat(MixerVolNames[PrevAct - 1], Mathf.Log10(FadeoutVolume) * 20);
            }
        }
    }
}
