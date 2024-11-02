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
    private GameObject Track1Trigger;

    [SerializeField]
    private GameObject Track2Trigger;

    [SerializeField]
    private GameObject Track3Trigger;

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

    private float Track1CurVolume;
    private float Track2CurVolume;
    private float Track3CurVolume;

    //hidden variables
    private bool playing = false;

    private int Act = 0;
    private int PrevAct = 0;


    private void Update()
    {
        CurrentTime = CurrentTime + Time.deltaTime;

        if (Act == 1)
        {
            if (CurrentTime < FadeDuration)
            {
                FadeTime = CurrentTime / FadeDuration;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeTime);
                Track1Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                //Debug.Log("Fading for " + CurrentTime);
            }

            if (FadeoutTime > 0)
            {
                FadeoutTime = (1 - (CurrentTime / FadeDuration));

                if (PrevAct == 2)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeoutTime);
                    Track2Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                    Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
                }

                if (PrevAct == 3)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeoutTime);
                    Track3Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track3VolName, out Track3CurVolume);

                    Debug.Log("Current Track 3 Volume = " + Track3CurVolume);
                }
            }

            Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
        }

        else if (Act == 2)
        {
            if (CurrentTime < FadeDuration)
            {
                FadeTime = CurrentTime / FadeDuration;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeTime);
                Track2Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                //Debug.Log("Fading for " + CurrentTime);
            }

            if (FadeoutTime > 0)
            {
                FadeoutTime = (1 - (CurrentTime / FadeDuration));

                if (PrevAct == 1)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeoutTime);
                    Track1Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                    Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
                }

                if (PrevAct == 3)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeoutTime);
                    Track3Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track3VolName, out Track3CurVolume);

                    Debug.Log("Current Track 3 Volume = " + Track3CurVolume);
                }
            }

            Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
        }

        else if (Act == 3)
        {
            if (CurrentTime < FadeDuration)
            {
                FadeTime = CurrentTime / FadeDuration;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeTime);
                Track3Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track3VolName, out Track3CurVolume);

                //Debug.Log("Fading for " + CurrentTime);
            }

            if (FadeoutTime > 0)
            {
                FadeoutTime = (1 - (CurrentTime / FadeDuration));

                if (PrevAct == 1)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeoutTime);
                    Track1Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                    Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
                }

                if (PrevAct == 2)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeoutTime);
                    Track2Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                    Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
                }
            }

            Debug.Log("Current Track 3 Volume = " + Track3CurVolume);
        }

        else if (Act == 0)
        {
            if (FadeoutTime > 0)
            {
                FadeoutTime = (1 - (CurrentTime / FadeDuration));

                if (PrevAct == 1)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeoutTime);
                    Track1Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                    Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
                }

                if (PrevAct == 2)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeoutTime);
                    Track2Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                    Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
                }

                if (PrevAct == 3)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeoutTime);
                    Track3Trigger.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track3VolName, out Track3CurVolume);

                    Debug.Log("Current Track 3 Volume = " + Track3CurVolume);
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
        if (Act != 1)
        {
            //ActivityTrigger1.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
            ActivityButton1.GetComponent<Image>().sprite = ButtonSelectedSprite;
            ActivityButton2.GetComponent<Image>().sprite = ButtonDefaultSprite;
            ActivityButton3.GetComponent<Image>().sprite = ButtonDefaultSprite;
            PrevAct = Act;
            Act = 1;
            CurrentTime = 0;
            FadeoutTime = FadeDuration;

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

   /* public void Stop()
    {
        PrevAct = Act;
        Act = 0;
        if (playing)
        {
            studioEventEmitter.Stop();
            playing = false;
        }
        Debug.Log("Music Playing = " + playing);
    }*/
}
