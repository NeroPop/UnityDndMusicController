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
    private float FadeTime;
    private float FadeoutTime = 0;
    private float CurFadeTime;


    public void TriggerActivity(int ActivityNumber)
    {
        PrevAct = Act;

        //Runs if the button hasn't already been pressed
        if (Act != ActivityNumber)
        {
            //sets the current activity
            Act = ActivityNumber;
            playing = true;

            //changes the button sprites
            ActivityButtons[Act-1].GetComponent<Image>().sprite = ButtonSelectedSprite;
            ActivityButtons[PrevAct - 1].GetComponent<Image>().sprite = ButtonDefaultSprite;
        }

        //Runs if the button has already been pressed
        else
        {
            ActivityButtons[Act-1].GetComponent<Image>().sprite = ButtonDefaultSprite;
            playing = false;
        }
    }

    private void Update()
    {
        //sets current time and is used to measure how long the fades have been going on for.
        CurrentTime = CurrentTime + Time.deltaTime;
    }
}
