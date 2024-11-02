using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActivityManager : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private string variableName;

    [SerializeField]
    private StudioEventEmitter studioEventEmitter;

    [Header("Activity Perameter Triggers")]

    [SerializeField]
    private GameObject ActivityTrigger1;

    [SerializeField]
    private GameObject ActivityTrigger2;

    [SerializeField]
    private GameObject ActivityTrigger3;

    [Header("Volumes")]

    [SerializeField]
    private GameObject Track1Vol;

    [SerializeField]
    [Tooltip("Name of the Track 1 volume variable to change. Case sensitive.")]
    private string Track1VolName;

[SerializeField]
    private GameObject Track2Vol;

    [SerializeField]
    [Tooltip("Name of the Track 2 volume variable to change. Case sensitive.")]
    private string Track2VolName;

    [SerializeField]
    private GameObject Track3Vol;

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

    //hidden variables
    private bool playing = false;

    public int Act = 0;
    public int PrevAct = 0;

    //fade control
    public float FadeDuration = 5;
    public bool Fade = true;
    private float CurrentTime;
    private float FadeTime;
    private float FadeoutTime;

    private float Track1CurVolume;
    private float Track2CurVolume;
    private float Track3CurVolume;


    private void Update()
    {
        CurrentTime = CurrentTime + Time.deltaTime;

        if (Act == 1)
        {
            if (CurrentTime < FadeDuration)
            {
                FadeTime = CurrentTime / FadeDuration;
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeTime);
                Track1Vol.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                //Debug.Log("Fading for " + CurrentTime);
            }

            if (FadeoutTime > 0)
            {
                FadeoutTime = (FadeDuration - CurrentTime);

                if (PrevAct == 2)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeoutTime);
                    Track2Vol.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                    Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
                }

                if (PrevAct == 3)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeoutTime);
                    Track3Vol.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
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
                Track2Vol.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                //Debug.Log("Fading for " + CurrentTime);
            }

            if (FadeoutTime > 0)
            {
                FadeoutTime = (FadeDuration - CurrentTime);

                if (PrevAct == 1)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeoutTime);
                    Track1Vol.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                    Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
                }

                if (PrevAct == 3)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track3VolName, FadeoutTime);
                    Track3Vol.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
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
                Track3Vol.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track3VolName, out Track3CurVolume);

                //Debug.Log("Fading for " + CurrentTime);
            }

            if (FadeoutTime > 0)
            {
                FadeoutTime = (FadeDuration - CurrentTime);

                if (PrevAct == 1)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track1VolName, FadeoutTime);
                    Track1Vol.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track1VolName, out Track1CurVolume);

                    Debug.Log("Current Track 1 Volume = " + Track1CurVolume);
                }

                if (PrevAct == 2)
                {
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Track2VolName, FadeoutTime);
                    Track2Vol.GetComponent<FMODUnity.StudioGlobalParameterTrigger>().TriggerParameters();
                    FMODUnity.RuntimeManager.StudioSystem.getParameterByName(Track2VolName, out Track2CurVolume);

                    Debug.Log("Current Track 2 Volume = " + Track2CurVolume);
                }
            }

            Debug.Log("Current Track 3 Volume = " + Track3CurVolume);
        }

        else
        {
            FadeTime = 0;
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
                studioEventEmitter.Stop();
                playing = false;
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
                studioEventEmitter.Stop();
                playing = false;
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
                studioEventEmitter.Stop();
                playing = false;
            }
        }
    }

    public void Stop()
    {
        PrevAct = Act;
        Act = 0;
        if (playing)
        {
            studioEventEmitter.Stop();
            playing = false;
        }
        Debug.Log("Music Playing = " + playing);
    }
}
