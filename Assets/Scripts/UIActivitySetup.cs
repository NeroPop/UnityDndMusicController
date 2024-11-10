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
    public TMP_Text CurrentSong;
    public TMP_Text CurrentTime;
    public TMP_Text RemainingTime;
    public Slider AudioSlider;
    public Button Skip;
    public Button Back;
    public Button Pause;
    public Button Resume;
    public Button ShuffleOff;
    public Button ShuffleOn;

    public EventTrigger SliderEventTrigger;

    void Start()
    {
        //Sets up Activity references
        Activity.GetComponent<MusicController>().DisplayName = CurrentSong;
        Activity.GetComponent<MusicController>().DisplayTime = CurrentTime;
        Activity.GetComponent<MusicController>().DisplayRemaining = RemainingTime;
        Activity.GetComponent<MusicController>().AudioSlider = AudioSlider;
        Activity.GetComponent<MusicController>().PauseButton = Pause.gameObject;
        Activity.GetComponent<MusicController>().ResumeButton = Resume.gameObject;

        //Sets up Audio Slider References
        SliderEventTrigger = AudioSlider.GetComponent<EventTrigger>();

        AudioSlider.onValueChanged.AddListener(Activity.GetComponent<MusicController>().OnSliderValueChanged);

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { Activity.GetComponent<MusicController>().OnPointerDown(); });
        SliderEventTrigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID= EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((eventData) => { Activity.GetComponent<MusicController>().OnPointerUp(); });
        SliderEventTrigger.triggers.Add(pointerUpEntry);

        //Sets up references for buttons
        Skip.onClick.AddListener(Activity.GetComponent<MusicController>().Skip);
        Back.onClick.AddListener(Activity.GetComponent<MusicController>().Back);
        Pause.onClick.AddListener(Activity.GetComponent<MusicController>().Pause);
        Resume.onClick.AddListener(Activity.GetComponent<MusicController>().Resume);
        ShuffleOff.onClick.AddListener(Activity.GetComponent<MusicController>().ToggleShuffle);
        ShuffleOn.onClick.AddListener(Activity.GetComponent<MusicController>().ToggleShuffle);
    }
}
