using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UnityActivityManager : MonoBehaviour
{
    [Header("Audio Mixers")]

    public AudioMixerGroup[] ActivityMixers;

    public string[] MixerVolNames;

    [Header("Buttons")]

    [SerializeField]
    private GameObject[] ActivityButtons;

    [SerializeField]
    private Sprite ButtonDefaultSprite;

    [SerializeField]
    private Sprite ButtonSelectedSprite;

    [Header("Fade")]
    public float FadeDuration = 5;

    [Header("Debugging")]

    public int Act = 0;     //Act is the current activity number. 0 means no activity is playing.
    private int PrevAct;    //PrevAct is the previous activity number that was playing.
    private int FadingAct;  //FadingAct is the old previous activity if it hasn't finished fading yet.

    private float CurrentTime;  //This is the current amount of time the music has been fading for.

    private float VolumeLevel;      //VolumeLevel is the level at which the current activity's volume is set. (usually increases from 0-1 when you select an activity.)
    private float PreVolumeLevel;   //PreVolumeLevel is the Volume Level of the previous activity
    private float OldPreVolumeLevel;//OldPreVolumeLevel is the volume level of the previous previous activity (used if swtched rapidly)

    public float FadeoutVolume = 0; //FadeoutVolume is the opposite of VolumeLevel and is used to fade out activities.
    private float RemainingVol;     //This is the remaining volume level if it hadn't finished fading when switching activities again.

    private float CurFadeTime;      //the amount the previous activity had faded, aka its volume level.
    private float OldCurFadeTime;   //The previous CurFadeTime remaining if it switches again whilst still fading.


    public void TriggerActivity(int ActivityNumber)
    {
        if (RemainingVol > 0 && FadingAct > 0)
        {
            //sets the old old previous activity volume to 0.
            ActivityMixers[FadingAct - 1].audioMixer.SetFloat(MixerVolNames[FadingAct - 1], Mathf.Log10(0) * 20);
        }

        if (FadeoutVolume > 0 && PrevAct > 0)
        {
            //sets the old previous activity volume to decrease.
            FadingAct = PrevAct;
            RemainingVol = FadeoutVolume;
            OldCurFadeTime = FadeoutVolume;
            OldPreVolumeLevel = PreVolumeLevel;
        }

        PrevAct = Act;
        PreVolumeLevel = VolumeLevel;

        //Runs if the button hasn't already been pressed
        if (ActivityNumber != Act)
        {
            //sets the current activity
            Act = ActivityNumber;

            //Resets fading
            CurrentTime = 0;
            FadeoutVolume = PreVolumeLevel;
            CurFadeTime = PreVolumeLevel;

            //changes the button sprites
            ActivityButtons[Act-1].GetComponent<Image>().sprite = ButtonSelectedSprite;

            //ensures the erray isn't out of bounds
            if (PrevAct > 0)
            {
                ActivityButtons[PrevAct - 1].GetComponent<Image>().sprite = ButtonDefaultSprite;
            }
            //Debug.Log("Playing activity " + ActivityNumber);
        }

        //Runs if the button has already been pressed
        else
        {
            ActivityButtons[Act-1].GetComponent<Image>().sprite = ButtonDefaultSprite;
            Act = 0;

            CurrentTime = 0;
            FadeoutVolume = PreVolumeLevel;
            CurFadeTime = PreVolumeLevel;
        }
    }

    private void Update()
    {
        //Debug.Log("Activity 1's Current Volume is " + VolumeLevel);

        //sets current time and is used to measure how long the fades have been going on for.
        CurrentTime = CurrentTime + Time.deltaTime;

        //checks if activity is playing or not
        if (Act > 0)
        {
            //Music Fades in
            if (CurrentTime < FadeDuration)
            {
                //VolumeLevel sets the volume between 0-1, going up as time gets closer to fade duration.
                VolumeLevel = CurrentTime / FadeDuration;

                //Sets the activity volume to VolumeLevel
                ActivityMixers[Act - 1].audioMixer.SetFloat(MixerVolNames[Act - 1], Mathf.Log10(VolumeLevel) * 20);
                //Debug.Log("Fading in Activity " + Act + " Current Volume is " + VolumeLevel);
            }

            //Music Fades out.
            if (FadeoutVolume > 0 && PrevAct > 0)
            {
                if (PrevAct != Act) //Checks that the current ativity isn't the same as before.
                {
                    //Sets FadeoutVolume to the opposite of that of VolumeLevel, also taking the current fade time into account.
                    FadeoutVolume = CurFadeTime - (CurrentTime / FadeDuration);

                    //sets the previous activity volume to FadeoutVolume which decreases over time.
                    ActivityMixers[PrevAct - 1].audioMixer.SetFloat(MixerVolNames[PrevAct - 1], Mathf.Log10(FadeoutVolume) * 20);
                    //Debug.Log("Fading out Activity " + PrevAct + " Current Volume is " + FadeoutVolume);
                }
            }

            //Music continues to fade out if it didnt finish
            if (RemainingVol > 0 && FadingAct  > 0)
            {
                if (FadingAct != Act) //Checks that the current activity isn't the same as the fading activity.
                {
                    RemainingVol = OldCurFadeTime - (CurrentTime / FadeDuration);

                    //fades out the old previous activity
                    ActivityMixers[FadingAct - 1].audioMixer.SetFloat(MixerVolNames[FadingAct - 1], Mathf.Log10(RemainingVol) * 20);
                }
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

            //Music continues to fade out if it didnt finish
            if (RemainingVol > 0 && FadingAct > 0)
            {
                if (FadingAct != Act)
                {
                    RemainingVol = OldCurFadeTime - (CurrentTime / FadeDuration);

                    //fades out the old previous activity
                    ActivityMixers[FadingAct - 1].audioMixer.SetFloat(MixerVolNames[FadingAct - 1], Mathf.Log10(RemainingVol) * 20);
                }
            }
        }
    }
}