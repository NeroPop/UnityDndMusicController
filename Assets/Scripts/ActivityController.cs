using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class ReadOnlyAttribute : PropertyAttribute { }
public class ActivityController : MonoBehaviour
{
    // [Header("References")]
    // public GameObject MusicManager;

    //private UnityActivityManager activityManager;

    [Header("Soundtrack Audio")]

    //List of all the audio tracks
    //public AudioClip[] Tracks;
    public List<AudioClip> Tracks = new List<AudioClip>();

    [Header("Display Elements")]

    //UI elements
    public TMP_Text DisplayName;
    public TMP_Text DisplayTime;
    public TMP_Text DisplayRemaining;
    public Slider AudioSlider;
    public GameObject PauseButton;
    public GameObject ResumeButton;
    static public float TimePercent;

    [Header("Music Information")]

    //Displays the music informaiton in the inspector. All either read only or private
    [ReadOnly]
    [SerializeField]
    int TrackNumber = 1;

    [ReadOnly]
    [SerializeField]
    private float CurrentTime = 0;

    [ReadOnly]
    [SerializeField]
    private float TrackLength;

    [ReadOnly]
    [SerializeField]
    private bool Shuffle = false;

    [ReadOnly]
    [SerializeField]
    private bool Paused = false;

    [ReadOnly]
    [SerializeField]
    private bool isDragging = false;

    public bool ActSelected;

    //Only used in the script internally
    private float ClipTime = 0;
    private int PrevTrack;
    private float displayseconds;
    private float displayminutes;
    private float remaining;
    private float remainingseconds;
    private float remainingminutes;

    //Events
    public UnityEvent ResetDial;


    //Tells unity what onUnpdate is because it's dumb
    public event Action<float> OnUpdate;
    private void Start()
    {
        //activityManager = MusicManager.GetComponent<UnityActivityManager>();

        //Starts playing the first song.
        //If it's shuffled then the track is random, if not then it sets the track to 1
        if (!Shuffle)
        {
            TrackNumber = 0;
            PlaySong();
        }

        else if (Shuffle)
        {
            TrackNumber = Random.Range(0, Tracks.Count);
            PlaySong();
        }

        // Add listeners to handle the slider events
        AudioSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void Update()
    {
        if (ActSelected)
        {
            //Get's a reference for the AudioSource
            AudioSource audio = GetComponent<AudioSource>();

            //Figures out the track length of the audio clip
            TrackLength = audio.clip.length;

            //checks if the music is paused
            if (!Paused)
            {
                //Adds 1 second to current time every second
                OnUpdate?.Invoke(Time.deltaTime);
                CurrentTime += Time.deltaTime;

                //If the user isnt dragging the audio slider then it sets the audio slider to the current time to keep track of progress
                if (!isDragging)
                {
                    //AudioSlider.value = CurrentTime;
                    TimePercent = (CurrentTime / TrackLength) * 100;
                }
            }
            //Displays the current and remaining times
            DisplayRemaining.text = "-" + FormatTime(TrackLength - CurrentTime);
            DisplayTime.text = FormatTime(CurrentTime);
            AudioSlider.value = CurrentTime;
        }
    }

    IEnumerator Playing()
    {
        //Get's a reference for the AudioSource
        AudioSource audio = GetComponent<AudioSource>();

        //Starts playing the audio clip
        audio.Play();
         
        //Sends a message to the console about what track is playing, how long it is and what time it's playing from.
       // Debug.Log("Playing Track " + TrackNumber + " Song Length " + audio.clip.length.ToString("F2") + " Playing from " + CurrentTime.ToString("F2"));

        //Waits until the end of the song
        yield return new WaitForSeconds(audio.clip.length - CurrentTime);

         //Checks if the music is paused
         if (!Paused)
         {
            //Checks if the music isn't shuffled
            if (!Shuffle)
             {
                //Checks that the clip has definitely finished
                 if (CurrentTime >= audio.clip.length)
                 {
                    //Sets the previous track as the current track before setting the current track to the next one and calling the PlaySong function
                     PrevTrack = TrackNumber;
                     TrackNumber = (TrackNumber + 1) % Tracks.Count;
                     PlaySong();
                     StopCoroutine(Playing());
                 }
             }

             //Checks if the music is on shuffle
             else if (Shuffle)
             {
                //Checks that the clip has definitely finished
                if (CurrentTime >= audio.clip.length)
                 {
                    //Calls tbe shuffled function
                     Shuffled();
                     StopCoroutine(Playing());
                 }
             }
         }
    }

    public void PlaySong()
    {

        
        //Get's a reference for the AudioSource
        AudioSource audio = GetComponent<AudioSource>();


        //Sets the track number to the last track if the current track number is somehow below 0
        if (TrackNumber < 0 || TrackNumber >= Tracks.Count) return;

        //Sets the audio clip as whatever the current track number is
        audio.clip = Tracks[TrackNumber];

        //Sets the times back to 0
        CurrentTime = 0;
        AudioSlider.value = 0;
        displayseconds = 0;
        displayminutes = 0;

        //Resets the dial back to 0
        ResetDial.Invoke();

        //Displays the song name
        DisplayName.text = audio.clip.name;


        // Set the slider's max value to the length of the audio clip
        AudioSlider.maxValue = audio.clip.length;

        // Add listeners to handle the slider events
        AudioSlider.onValueChanged.AddListener(OnSliderValueChanged);

        //Figures out how long is left of the audio clip
        ClipTime = audio.clip.length - CurrentTime;

        //Starts the playing couroutine if it's not paused
        if (!Paused)
        {
            StartCoroutine(Playing());
        }
    }

    public void loadCustomTrack()
    {
        TrackNumber = 0;
        PlaySong();
    }

    public void Skip()
    {
        StopCoroutine(Playing());

        //Plays the next song
        //If shuffled, it calls the shuffle function, if not shuffled it increases the track number and then plays the next song
        if (!Shuffle)
        {
            TrackNumber = (TrackNumber + 1) % Tracks.Count;
            PlaySong();
        }

        else if (Shuffle)
        {
            Shuffled();
        }
    }

    public void Back()
    {
        //Plays the previous song
        //If shuffled, it plays the previous track, if not shuffled it takes the current track and removes 1
        StopCoroutine(Playing());
        if (Shuffle)
        {
            TrackNumber = PrevTrack;
        }
        else
        {
            TrackNumber = (TrackNumber - 1 + Tracks.Count) % Tracks.Count;
        }
        PlaySong();
    }

    public void OnPointerDown()
    {
        //If the user starts holding the mouse pointer down on the slider, it sets whole numbers to true for precision and also dragging to true before calling pause
        isDragging = true;
        AudioSlider.wholeNumbers = true;
        Pause();
    }

    public void OnPointerUp()
    {
        //If the user stops holding the mouse pointer down on the slider, it sets whole numbers to false for smooth progress and dragging to false before calling resume
        isDragging = false;
        AudioSlider.wholeNumbers = false;
        Resume();
    }

    public void Pause()
    {
        //Gets a reference to the audio source
        AudioSource audio = GetComponent<AudioSource>();

        //Pauses the audio and stops the coroutine from playing the next track. Also sends a message to the console displaying what time it paused at
        StopCoroutine(Playing());
        Paused = true;
        audio.Pause();
        //Debug.Log("Paused at " + CurrentTime.ToString("F2"));

        //Switches the pause & resume buttons for UX
        PauseButton.SetActive(false);
        ResumeButton.SetActive(true);
    }

    public void Resume()
    {
        //Gets a reference to the audio source
        AudioSource audio = GetComponent<AudioSource>();

        //Starts the coroutine again and unpauses the audio
        audio.time = CurrentTime;
        StartCoroutine(Playing());
        Paused = false;
        audio.UnPause();

        //Switches the pause & resume buttons for UX
        PauseButton.SetActive(true);
        ResumeButton.SetActive(false);
       // Debug.Log("Resume at " + CurrentTime.ToString("F2"));
    }

    private void Shuffled()
    {
        //Sets the previous track to the current track number
        PrevTrack = TrackNumber;
        do
        {
            //randomises the next track from the tracks list
            TrackNumber = Random.Range(0, Tracks.Count);
        } while (TrackNumber == PrevTrack);

        //plays the random song
        PlaySong();
    }

    public void ToggleShuffle()
    {
        //Enables or disables shuffle depending on what state it was on before
        if (!Shuffle)
        {
            Shuffle = true;
        }

        else if (Shuffle)
        {
            Shuffle = false;
        }
    }

    public void OnSliderValueChanged(float value)
    {
        //Get's a reference for the AudioSource
        AudioSource audio = GetComponent<AudioSource>();

        if (isDragging)
        {
            // Update the audio playback time to match the slider value
            audio.time = value;
            CurrentTime = value;

            //update the display times to match the slider value
            displayseconds = CurrentTime;
            displayminutes = ((int)displayseconds) / 60;
            displayseconds = displayseconds - (displayminutes * 60);
        }
    }

    public void OnDialScrubChanged()
    {
        //Get's a reference for the AudioSource
        AudioSource audio = GetComponent<AudioSource>();

        if (isDragging)
        {
            // Update the audio playback time to match the slider value
            CurrentTime = (TimePercent / 100) * TrackLength;
            audio.time = CurrentTime;


           // audio.time = value;
           // CurrentTime = value;

            //update the display times to match the slider value
            displayseconds = CurrentTime;
            displayminutes = ((int)displayseconds) / 60;
            displayseconds = displayseconds - (displayminutes * 60);
        }
    }

    private string FormatTime(float time)
    {
        //formats time into seconds and minutes to be displayed properly
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
