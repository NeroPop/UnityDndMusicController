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
    public GameObject Activity;

    [Header("UI References")]
    public GameObject ActivityPlayerControls;
    public GameObject DisabledPlayerControls;

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

    void Start()
    {
        //Sets up Activity references
        Activity.GetComponent<ActivityController>().DisplayName = CurrentSong;
        Activity.GetComponent<ActivityController>().DisplayTime = CurrentTime;
        Activity.GetComponent<ActivityController>().DisplayRemaining = RemainingTime;
        Activity.GetComponent<ActivityController>().AudioSlider = AudioSlider;
        Activity.GetComponent<ActivityController>().PauseButton = Pause.gameObject;
        Activity.GetComponent<ActivityController>().ResumeButton = Resume.gameObject;

        //Sets up Audio Slider References
        SliderEventTrigger = AudioSlider.GetComponent<EventTrigger>();

        AudioSlider.onValueChanged.AddListener(Activity.GetComponent<ActivityController>().OnSliderValueChanged);

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { Activity.GetComponent<ActivityController>().OnPointerDown(); });
        SliderEventTrigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID= EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((eventData) => { Activity.GetComponent<ActivityController>().OnPointerUp(); });
        SliderEventTrigger.triggers.Add(pointerUpEntry);

        //Sets up references for buttons
        Skip.onClick.AddListener(Activity.GetComponent<ActivityController>().Skip);
        Back.onClick.AddListener(Activity.GetComponent<ActivityController>().Back);
        Pause.onClick.AddListener(Activity.GetComponent<ActivityController>().Pause);
        Resume.onClick.AddListener(Activity.GetComponent<ActivityController>().Resume);
        ShuffleOff.onClick.AddListener(Activity.GetComponent<ActivityController>().ToggleShuffle);
        ShuffleOn.onClick.AddListener(Activity.GetComponent<ActivityController>().ToggleShuffle);

        //Hides the Activity Controls
        ActSelected = false;
        ActivityPlayerControls.SetActive(false);
        DisabledPlayerControls.SetActive(true);

        //Starts the song
        Activity.GetComponent<ActivityController>().PlaySong();
    }
    public void ToggleActivity()
    {
        if (!ActSelected)
        {
            ActivityPlayerControls.SetActive(true);
            DisabledPlayerControls.SetActive(false);
            ActSelected = true;
        }
        else if (ActSelected)
        {
            ActivityPlayerControls.SetActive(false);
            DisabledPlayerControls.SetActive(true);
            ActSelected = false;
        }
    }

    public void ActivityOn()
    {
        ActivityPlayerControls.SetActive(true);
        DisabledPlayerControls.SetActive(false);
        ActSelected = true;
    }

    public void ActivityOff(bool Inactive)
    {
        ActivityPlayerControls.SetActive(false);
        ActSelected = false;
        if (Inactive)
        {
            DisabledPlayerControls.SetActive(true);
        }
    }
}
