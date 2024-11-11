using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UIActivitySetup : MonoBehaviour
{
    [Header("Activity")]
    public GameObject[] Activity;

    [Header("UI References")]
    public GameObject ActivityPlayerControls;
    public GameObject InactivePlayer;

    public TMP_Text CurrentSong;
    public TMP_Text CurrentTime;
    public TMP_Text RemainingTime;
    public Slider AudioSlider;
    public EventTrigger SliderEventTrigger;
    public Button Skip;
    public Button Back;
    public Button Pause;
    public Button Resume;
    public Button ShuffleOff;
    public Button ShuffleOn;

    private bool ActSelected = false;
    public int Act;

    private bool Started;

    void Start()
    {
        //Sets up the list of activities
        Activity = GetComponent<UnityActivityManager>().Activities;

        //Sets up Audio Slider References
        SliderEventTrigger = AudioSlider.GetComponent<EventTrigger>();

        AudioSlider.onValueChanged.AddListener(Activity[Act].GetComponent<ActivityController>().OnSliderValueChanged);

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { Activity[Act].GetComponent<ActivityController>().OnPointerDown(); });
        SliderEventTrigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID= EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((eventData) => { Activity[Act].GetComponent<ActivityController>().OnPointerUp(); });
        SliderEventTrigger.triggers.Add(pointerUpEntry);

        //Sets up references for buttons
        Skip.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Skip);
        Back.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Back);
        Pause.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Pause);
        Resume.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Resume);
        ShuffleOff.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().ToggleShuffle);
        ShuffleOn.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().ToggleShuffle);

        //Hides the Activity Controls
        ActSelected = false;
        ActivityPlayerControls.SetActive(false);

        //Starts the song
        Activity[Act].GetComponent<ActivityController>().PlaySong();

        //sets up every activity
        Act = 0;
        Started = true;
    }
    public void ToggleActivity(int Act)
    {
        if (!ActSelected)
        {
            //Sets the player to be active
            ActivityPlayerControls.SetActive(true);
            InactivePlayer.SetActive(false);
            ActSelected = true;

            //Sets up Audio Slider References
            SliderEventTrigger = AudioSlider.GetComponent<EventTrigger>();

            AudioSlider.onValueChanged.AddListener(Activity[Act].GetComponent<ActivityController>().OnSliderValueChanged);

            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((eventData) => { Activity[Act].GetComponent<ActivityController>().OnPointerDown(); });
            SliderEventTrigger.triggers.Add(pointerDownEntry);

            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((eventData) => { Activity[Act].GetComponent<ActivityController>().OnPointerUp(); });
            SliderEventTrigger.triggers.Add(pointerUpEntry);

            //Sets up references for buttons
            Skip.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Skip);
            Back.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Back);
            Pause.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Pause);
            Resume.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Resume);
            ShuffleOff.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().ToggleShuffle);
            ShuffleOn.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().ToggleShuffle);

            Activity[Act].GetComponent<ActivityController>().PlaySong();
        }
        else if (ActSelected)
        {
            ActivityPlayerControls.SetActive(false);
            InactivePlayer.SetActive(true);
            ActSelected = false;
        }
    }

    public void ActivityOn(int Act)
    {
        //Sets the player to be active
        ActivityPlayerControls.SetActive(true);
        InactivePlayer.SetActive(false);
        ActSelected = true;

        //Sets up Audio Slider References
        SliderEventTrigger = AudioSlider.GetComponent<EventTrigger>();

        AudioSlider.onValueChanged.AddListener(Activity[Act].GetComponent<ActivityController>().OnSliderValueChanged);

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { Activity[Act].GetComponent<ActivityController>().OnPointerDown(); });
        SliderEventTrigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((eventData) => { Activity[Act].GetComponent<ActivityController>().OnPointerUp(); });
        SliderEventTrigger.triggers.Add(pointerUpEntry);

        //Sets up references for buttons
        Skip.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Skip);
        Back.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Back);
        Pause.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Pause);
        Resume.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().Resume);
        ShuffleOff.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().ToggleShuffle);
        ShuffleOn.onClick.AddListener(Activity[Act].GetComponent<ActivityController>().ToggleShuffle);

        Activity[Act].GetComponent<ActivityController>().PlaySong();
        Debug.Log("Activity On UI change with activity " + Act);
    }

    public void ActivityOff()
    {
        //Sets the player to be inactive
        ActivityPlayerControls.SetActive(false);
        InactivePlayer.SetActive(true);
        ActSelected = false;
        Debug.Log("Activity Off UI change with activity " + Act);
    }
}
