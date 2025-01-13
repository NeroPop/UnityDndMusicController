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

    private ActivityController ActivityController;

    //debuging
    private int ActivityLoads;

    public void LoadActivity()
    {
        ActivityController = Activity.GetComponent<ActivityController>();

        //Find the disabled activity controller via the parent of the current activity
        Transform parent = transform.parent;
        DisabledPlayerControls = parent.Find("Inactive Player").gameObject;

        //Sets up Activity references
        ActivityController.DisplayName = CurrentSong;
        ActivityController.DisplayTime = CurrentTime;
        ActivityController.DisplayRemaining = RemainingTime;
        ActivityController.AudioSlider = AudioSlider;
        ActivityController.PauseButton = Pause.gameObject;
        ActivityController.ResumeButton = Resume.gameObject;

        //Sets up Audio Slider References
        SliderEventTrigger = AudioSlider.GetComponent<EventTrigger>();

        AudioSlider.onValueChanged.AddListener(ActivityController.OnSliderValueChanged);

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { ActivityController.OnPointerDown(); });
        SliderEventTrigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID= EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((eventData) => { ActivityController.OnPointerUp(); });
        SliderEventTrigger.triggers.Add(pointerUpEntry);

        //Sets up references for buttons
        Skip.onClick.AddListener(ActivityController.Skip);
        Back.onClick.AddListener(ActivityController.Back);
        Pause.onClick.AddListener(ActivityController.Pause);
        Resume.onClick.AddListener(ActivityController.Resume);
        ShuffleOff.onClick.AddListener(ActivityController.ToggleShuffle);
        ShuffleOn.onClick.AddListener(ActivityController.ToggleShuffle);

        //Hides the Activity Controls
        ActSelected = false;
        DisabledPlayerControls.SetActive(true);
        ActivityController.ActSelected = ActSelected;
        gameObject.SetActive(false);

        //Starts the song
        ActivityController.PlaySong();

        ActivityLoads++;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
