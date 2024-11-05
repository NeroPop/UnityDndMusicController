using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActivityManager : MonoBehaviour
{
    [Header("FMOD References")]

    [SerializeField]
    private StudioEventEmitter studioEventEmitter;

    [SerializeField]
    private GameObject TrackTrigger;

    [SerializeField]
    [Tooltip("Name of the Track 1 volume variable to change. Case sensitive.")]
    private string Track1VolName;

    [SerializeField]
    [Tooltip("Name of the Track 2 volume variable to change. Case sensitive.")]
    private string Track2VolName;

    [SerializeField]
    [Tooltip("Name of the Track 3 volume variable to change. Case sensitive.")]
    private string Track3VolName;

    [Header("Buttons")]

    [SerializeField]
    private Sprite ButtonDefaultSprite;

    [SerializeField]
    private Sprite ButtonSelectedSprite;

    [SerializeField]
    private GameObject ActivityButton1;

    [SerializeField]
    private GameObject ActivityButton2;

    [SerializeField]
    private GameObject ActivityButton3;

    [Header("Fade")]
    public float FadeDuration = 5;
    private float CurrentTime;
    private float FadeTime;
    private float FadeoutTime = 0;
    private float CurFadeTime;

    private float Track1CurVolume;
    private float Track2CurVolume;
    private float Track3CurVolume;

    //hidden variables
    private bool playing = false;

    private int Act = 0;
    private int PrevAct = 0;


    private void Update()
    {
        //sets current time and is used to measure how long the fades have been going on for.
        CurrentTime = CurrentTime + Time.deltaTime;

        if (Act == 1)
        {
            //checks if the music should be fading in or not.
            if (CurrentTime < FadeDuration)
            {
                //FadeTime sets the volume between 0-1, going up as time gets closer to fade duration.
                FadeTime = CurrentTime / FadeDuration;

                //Sets the activity volume to FadeTime then triggers it and sets Track1CurVolume to whatever the current volume is.
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeTime);
                TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);
                //Debug.Log("Fading for " + CurrentTime);
            }

            if (FadeoutTime > 0)
            {
                //Sets FadeoutTime to the opposite of that of Fadetime, also taking the current fade time into account.
                FadeoutTime = (CurFadeTime - (CurrentTime / FadeDuration));

                //checks which previous activity to fade out and then fades it out.
                if (PrevAct == 2)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeoutTime);
                    TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                    //Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
                }

                if (PrevAct == 3)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeoutTime);
                    TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track3VolName, out Track3CurVolume);

                    //Debug.Log("Current Track 3 Volume = " + Track3CurVolume);
                }
            }

            //Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
        }

        //if you clicked activity 2 or 3 the same thing happens but relative to that activity.
        else if (Act == 2)
        {
            if (CurrentTime < FadeDuration)
            {
                FadeTime = CurrentTime / FadeDuration;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeTime);
                TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);
            }

            if (FadeoutTime > 0)
            {
                FadeoutTime = (CurFadeTime - (CurrentTime / FadeDuration));

                if (PrevAct == 1)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeoutTime);
                    TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                    //Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
                }

                if (PrevAct == 3)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeoutTime);
                    TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track3VolName, out Track3CurVolume);

                    //Debug.Log("Current Track 3 Volume = " + Track3CurVolume);
                }
            }

            //Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
        }

        else if (Act == 3)
        {
            if (CurrentTime < FadeDuration)
            {
                FadeTime = CurrentTime / FadeDuration;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeTime);
                TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track3VolName, out Track3CurVolume);

                //Debug.Log("Fading for " + CurrentTime);
            }

            if (FadeoutTime > 0)
            {
                FadeoutTime = (CurFadeTime - (CurrentTime / FadeDuration));

                if (PrevAct == 1)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeoutTime);
                    TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                    //Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
                }

                if (PrevAct == 2)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeoutTime);
                    TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                    //Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
                }
            }

            //Debug.Log("Current Track 3 Volume = " + Track3CurVolume);
        }

        //if you just click off an activity, it only fades out and doesn't fade in.
        else if (Act == 0)
        {
            if (FadeoutTime > 0)
            {
                FadeoutTime = (CurFadeTime - (CurrentTime / FadeDuration));

                if (PrevAct == 1)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeoutTime);
                    TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                    //Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
                }

                if (PrevAct == 2)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeoutTime);
                    TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                    //Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
                }

                if (PrevAct == 3)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeoutTime);
                    TrackTrigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track3VolName, out Track3CurVolume);

                    //Debug.Log("Current Track 3 Volume = " + Track3CurVolume);
                }
            }

            else if (FadeoutTime <= 0)
            {
                studioEventEmitter.Stop();
                playing = false;
            }
        }

    }

    public void Activity1()
    {
        //checks if you weren't already on activity 1.
        if (Act != 1)
        {
            //Changes the button sprites to match the new selection.
            ActivityButton1.GetComponent<Image>().sprite = ButtonSelectedSprite;
            ActivityButton2.GetComponent<Image>().sprite = ButtonDefaultSprite;
            ActivityButton3.GetComponent<Image>().sprite = ButtonDefaultSprite;

            //saves the previous Activity and then sets the current activity as 1
            PrevAct = Act;
            Act = 1;

            //resets the fading times
            CurrentTime = 0;
            FadeoutTime = FadeDuration;

            //Checks how long the fadeout needs to be based on how much the previous track has faded in
            if (PrevAct == 1)
            {
                CurFadeTime = Track1CurVolume;
            }
            else if(PrevAct == 2)
            {
                CurFadeTime = Track2CurVolume;
            }
            else if(PrevAct == 3)
            {
                CurFadeTime = Track3CurVolume;
            }

            //starts playing the music if itisn't already playing.
            if (!playing)
            {
                studioEventEmitter.Play();
                playing = true;
            }
        }
        //if you were already on activity 1 then it fades out the music and resets the button image
        else
        {
            PrevAct = Act;
            Act = 0;
            ActivityButton1.GetComponent<Image>().sprite = ButtonDefaultSprite;
            if (playing)
            {
                FadeoutTime = FadeDuration;
                CurrentTime = 0;
            }
        }
    }

    public void Activity2()
    {
        if (Act != 2)
        {
            //ActivityTrigger2.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
            ActivityButton1.GetComponent<Image>().sprite = ButtonDefaultSprite;
            ActivityButton2.GetComponent<Image>().sprite = ButtonSelectedSprite;
            ActivityButton3.GetComponent<Image>().sprite = ButtonDefaultSprite;
            PrevAct = Act;
            Act = 2;
            CurrentTime = 0;
            FadeoutTime = FadeDuration;

            //Checks how long the fadeout needs to be based on how much the previous track has faded in
            if (PrevAct == 1)
            {
                CurFadeTime = Track1CurVolume;
            }
            else if (PrevAct == 2)
            {
                CurFadeTime = Track2CurVolume;
            }
            else if (PrevAct == 3)
            {
                CurFadeTime = Track3CurVolume;
            }

            if (!playing)
            {
                studioEventEmitter.Play();
                playing = true;
            }
        }
        else
        {
            PrevAct = Act;
            Act = 0;
            ActivityButton2.GetComponent<Image>().sprite = ButtonDefaultSprite;

            if (playing)
            {
                FadeoutTime = FadeDuration;
                CurrentTime = 0;
            }
        }
    }

    public void Activity3()
    {
        if (Act != 3)
        {
            //ActivityTrigger3.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
            ActivityButton1.GetComponent<Image>().sprite = ButtonDefaultSprite;
            ActivityButton2.GetComponent<Image>().sprite = ButtonDefaultSprite;
            ActivityButton3.GetComponent<Image>().sprite = ButtonSelectedSprite;
            PrevAct = Act;
            Act = 3;
            CurrentTime = 0;
            FadeoutTime = FadeDuration;

            //Checks how long the fadeout needs to be based on how much the previous track has faded in
            if (PrevAct == 1)
            {
                CurFadeTime = Track1CurVolume;
            }
            else if (PrevAct == 2)
            {
                CurFadeTime = Track2CurVolume;
            }
            else if (PrevAct == 3)
            {
                CurFadeTime = Track3CurVolume;
            }

            if (!playing)
            {
                studioEventEmitter.Play();
                playing = true;
            }
        }
        else
        {
            PrevAct = Act;
            Act = 0;
            ActivityButton3.GetComponent<Image>().sprite = ButtonDefaultSprite;

            if (playing)
            {
                FadeoutTime = FadeDuration;
                CurrentTime = 0;
            }
        }
    }
}
