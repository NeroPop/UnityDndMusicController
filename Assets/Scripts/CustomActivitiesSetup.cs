using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.Networking;


public class CustomActivitiesSetup : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject ActivityPrefab;
    [SerializeField] private GameObject ActivityButtonPrefab;

    [Header("References")]
    [SerializeField] private GameObject ActivityButtonGroup;
    [SerializeField] private GameObject ActivitiesParent;
    [SerializeField] private GameObject CustomisationMenuUI;
    [SerializeField] private GameObject NewActivityUI;
    [SerializeField] private TMP_InputField ActivityNameInput;

    [Header("Lists")]
    public List<Button> ActivityButtons = new List<Button>();
    public List<AudioSource> ActivityAudioSources = new List<AudioSource>();
    public List<AudioClip> ActivityClips = new List<AudioClip>();

    [Header("Ambience Properties")]
    public string SceneName;
    public float FadeDuration = 5;
    public string ActivityName;
    public int NewActivityInt;
    private int PreloadedActivities;
    private string FilePath;
    private string AudioFolderPath;
    public bool clean = true;

    public void PlayActivity(int ActivityNumber)
    {

    }

    public void SetupNewActivity() //Opens up the New Activity sound setup menu
    {
        CustomisationMenuUI.SetActive(true);
        NewActivityUI.SetActive(true);
    }

    public void CancelNew() //Cancels the New Activity sound setup menu
    {
        CustomisationMenuUI.SetActive(false);
        NewActivityUI.SetActive(false);
    }

    public void NewActivityName() //Sets the Activity Name to whatever you called it
    {
        ActivityName = ActivityNameInput.text;
        Debug.Log("New Activity Name is " + ActivityName);
    }

    //Gets the audio clips that were selected from the file selector and saves them to the list
    private void LoadAudioFiles()
    {
        ActivityClips = gameObject.GetComponent<ActivityFileSelector>()?.ActivityaudioClips;
    }

    public void NewActivity()
    {
        //Increment the counter for new Activity
        NewActivityInt++;

        //Ensure that audio files are loaded
        LoadAudioFiles();

        //Create the new button for the Activity
        GameObject newActivityButton = Instantiate(ActivityButtonPrefab, ActivityButtonGroup.transform);
        newActivityButton.GetComponentInChildren<TMP_Text>().text = ActivityName;
        newActivityButton.name = "Button " + ActivityName;

        //Add the button to the list
        Button buttonComponent = newActivityButton.GetComponent<Button>();
        ActivityButtons.Add(buttonComponent);

        //Assign the button index correctly
        int buttonIndex = NewActivityInt + PreloadedActivities - 1; //Use the last index in the list
        newActivityButton.GetComponent<AmbientButtonController>().ButtonIndex = buttonIndex;

        //Ensure that on click it triggers the correct Activity
        buttonComponent.onClick.AddListener(() => PlayActivity(buttonIndex));

        //Create the new Activity game object
        GameObject newActivity = Instantiate(ActivityPrefab, ActivitiesParent.transform);
        newActivity.name = ActivityName;

        //Assign the audio clip to the audio source
        ActivityAudioSources.Add(newActivity.GetComponent<AudioSource>());
        newActivity.GetComponent<AudioSource>().clip = ActivityClips[NewActivityInt - 1];

        //Assign the Fade time to the Activity Controller
        newActivity.GetComponent<AmbientController>().FadeDuration = FadeDuration;

        //Testing Debugs which can be removed when it works
        Debug.Log($"AmbientButtons.Count: {ActivityButtons.Count}");
        Debug.Log($"AmbientIndex: {buttonIndex}");
        Debug.Log($"AmbientClips.Count: {ActivityClips.Count}");
        Debug.Log($"NewAmbientInt: {NewActivityInt}");
        Debug.Log($"PreloadedAmbience: {PreloadedActivities}");

        //Hide the customisation menus
        CustomisationMenuUI.SetActive(false);
        NewActivityUI.SetActive(false);

        Debug.Log($"Created new Ambient: {ActivityName} (Index: {buttonIndex})");
    }
}
