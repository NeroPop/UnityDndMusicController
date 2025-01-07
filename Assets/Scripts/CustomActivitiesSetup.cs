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
using System.ComponentModel;


public class CustomActivitiesSetup : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject ActivityPrefab;
    [SerializeField] private GameObject ActivityButtonPrefab;
    [SerializeField] private GameObject ActivityPlayerPrefab;

    [Header("References")]
    [SerializeField] private GameObject ActivityButtonGroup;
    [SerializeField] private GameObject ActivitiesParent;
    [SerializeField] private GameObject PlayerParent;
    [SerializeField] private GameObject CustomisationMenuUI;
    [SerializeField] private GameObject InactiveMediaPlayer;
    [SerializeField] private GameObject NewActivityUI;
    [SerializeField] private TMP_InputField ActivityNameInput;
    [SerializeField] private CustomisationInterface CustomisationInterface;

    [Header("Lists")]
    public List<Button> ActivityButtons = new List<Button>();
    public List<GameObject> ActivityMediaPlayers = new List<GameObject>();
    public List<AudioSource> ActivityAudioSources = new List<AudioSource>();
    public List<AudioClip> ActivityClips = new List<AudioClip>();

    [Header("UI Elements")]
    private TMP_Text DisplayName;
    private TMP_Text DisplayText;


    [Header("Activities Properties")]
    public string SceneName;
    public string ActivityName;
    public int NewActivityInt;
    private int PreloadedActivities;
    private string FilePath;
    private string FolderPath;
    private string FolderName;
    private string AudioFolderPath;
    public bool clean = true;

    private void Start()
    {
        LoadExistingWavFiles();
    }

    public void PlayActivity(int ActivityNumber)
    {
        //Triggers the activity to start playing
       // ActivityAudioSources[ActivityNumber].GetComponent<ActivityController>().PlaySong();

        //Gets a reference to the Activity Manager
        UnityActivityManager ActivityManager = gameObject.GetComponent<UnityActivityManager>();
        ActivityManager.TriggerActivity(ActivityNumber + 1);

        Debug.Log($"Activity {ActivityNumber} Triggered play");
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
        UnityActivityManager ActivityManager = gameObject.GetComponent<UnityActivityManager>();

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
        newActivityButton.GetComponent<ActivityButtonController>().ButtonIndex = buttonIndex;

        //Ensure that on click it triggers the correct Activity
        buttonComponent.onClick.AddListener(() => PlayActivity(buttonIndex));

        //Create the new Activity game object
        GameObject newActivity = Instantiate(ActivityPrefab, ActivitiesParent.transform);
        newActivity.name = ActivityName;

        //Assign the audio clip to the audio source
        ActivityAudioSources.Add(newActivity.GetComponent<AudioSource>());
        newActivity.GetComponent<AudioSource>().clip = ActivityClips[NewActivityInt - 1];


        //Create a new Media Player for the activity
        GameObject newActivityPlayer = Instantiate(ActivityPlayerPrefab, PlayerParent.transform);
        newActivityPlayer.name = ActivityName + " Media Player";
        newActivityPlayer.GetComponent<UIActivitySetup>().Activity = newActivity;

        //Triggers the player to set itself up
        newActivityPlayer.GetComponent<UIActivitySetup>().LoadActivity();

        //Add the media player to the list
        ActivityMediaPlayers.Add(newActivityPlayer);
        ActivityManager.PlayersList.Add(newActivityPlayer);

        //Testing Debugs which can be removed when it works
        Debug.Log($"ActivityButtons.Count: {ActivityButtons.Count}");
        Debug.Log($"ActivityIndex: {buttonIndex}");
        Debug.Log($"ActivityClips.Count: {ActivityClips.Count}");
        Debug.Log($"NewActivityInt: {NewActivityInt}");
        Debug.Log($"PreloadedActivities: {PreloadedActivities}");

        //Hide the customisation menus
        CustomisationMenuUI.SetActive(false);
        NewActivityUI.SetActive(false);

        Debug.Log($"Created new Activity: {ActivityName} (Index: {buttonIndex})");
    }

    //Loads existing .wav files from the specified FilePath and creates buttons for them. triggered by scene controller
    public void LoadExistingWavFiles()
    {
        //Gets reference to the ActivityManager
        UnityActivityManager ActivityManager = gameObject.GetComponent<UnityActivityManager>();

        //Checks if buttons have already been loaded
        if (!clean)
        {
            RemoveOldStuff();
        }

        else
        {
            Debug.Log("Triggered LoadExistingWavFiles");
#if UNITY_EDITOR
            //Sets the Folder path for in editor
            FolderPath = "Assets\\CustomAudio\\" + SceneName + "\\Activities";
            AssetDatabase.Refresh();
            string[] folders = AssetDatabase.FindAssets("t:Folder", new[] {FolderPath});

            foreach (string folderGUID in folders)
            {
                // Convert folder GUID to a path
                string folderPath = AssetDatabase.GUIDToAssetPath(folderGUID);

                // Extract folder name
                FolderName = System.IO.Path.GetFileName(folderPath);

                PreloadedActivities++; //Increment the counter for loaded activities

                //Setup the activity

                //Create the new button for each loaded clip
                GameObject newActivityButton = Instantiate(ActivityButtonPrefab, ActivityButtonGroup.transform);
                newActivityButton.GetComponentInChildren<TMP_Text>().text = FolderName;
                newActivityButton.name = "Button " + FolderName;
                Debug.Log("New Activity Button Name = Button " + FolderName);

                //Add the button to the list
                Button buttonComponent = newActivityButton.GetComponent<Button>();
                ActivityButtons.Add(buttonComponent);

                //Create the new Activity game object
                GameObject newActivity = Instantiate(ActivityPrefab, ActivitiesParent.transform);
                newActivity.name = FolderName;

                //Assign the button index correctly
                int buttonIndex = NewActivityInt + PreloadedActivities - 1; //Use the last index in the list
                newActivityButton.GetComponent<ActivityButtonController>().ButtonIndex = buttonIndex;

                //Ensure that on click it triggers the correct Activity
                buttonComponent.onClick.AddListener(() => PlayActivity(buttonIndex));

                //Create a new Media Player for the activity
                GameObject newActivityPlayer = Instantiate(ActivityPlayerPrefab, PlayerParent.transform);
                newActivityPlayer.name = ActivityName + " Media Player";
                newActivityPlayer.GetComponent<UIActivitySetup>().Activity = newActivity;

                //Triggers the player to set itself up
                newActivityPlayer.GetComponent<UIActivitySetup>().LoadActivity();

                //Add the media player to the list
                ActivityMediaPlayers.Add(newActivityPlayer);
                ActivityManager.PlayersList.Add(newActivityPlayer);

                // Check if the folder is directly under the specified directory
                if (System.IO.Path.GetDirectoryName(folderPath) == FolderPath)
                {
                    newActivity.GetComponent<ActivityClipLoader>().ActivityName = ActivityName;
                    newActivity.GetComponent<ActivityClipLoader>().LoadClips();
                }
                else
                {
                    Debug.LogWarning("Folder not found in specified directory " + System.IO.Path.GetDirectoryName(folderPath));
                }
            }
#else
            //Sets the Folder path for in Build
            FolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "Activities");

            if (!Directory.Exists(FilePath)) //Checks that the file path exists
            {
                Debug.LogWarning($"The specified directory does not exist: {FilePath}");
                return;
            }

            if (Directory.Exists(FolderPath))
            {
                string[] folders = Directory.GetDirectories(FolderPath);

                foreach (string folderPath in folders)
                {
                    FolderName = Path.GetFileName(folderPath);

                    Debug.Log("Found folder " + FolderName + " at " + folderPath);

                    // Ensure folder exists and is valid
                    if (!string.IsNullOrEmpty(FolderName))
                    {
                        //Sets everything up
                        //Create the new button for each loaded clip
                        GameObject newActivityButton = Instantiate(ActivityButtonPrefab, ActivityButtonGroup.transform);
                        newActivityButton.GetComponentInChildren<TMP_Text>().text = FolderName;
                        newActivityButton.name = "Button " + FolderName;

                        //Add the button to the list
                        Button buttonComponent = newActivityButton.GetComponent<Button>();
                        ActivityButtons.Add(buttonComponent);

                        //Create the new Activity game object
                        GameObject newActivity = Instantiate(ActivityPrefab, ActivitiesParent.transform);
                        newActivity.name = FolderName;

                        //Assign the button index correctly
                        int buttonIndex = NewActivityInt + PreloadedActivities - 1; //Use the last index in the list
                        newActivityButton.GetComponent<ActivityButtonController>().ButtonIndex = buttonIndex;

                        //Ensure that on click it triggers the correct Activity
                        buttonComponent.onClick.AddListener(() => PlayActivity(buttonIndex));

                        //Create a new Media Player for the activity
                        GameObject newActivityPlayer = Instantiate(ActivityPlayerPrefab, PlayerParent.transform);
                        newActivityPlayer.name = ActivityName + " Media Player";
                        newActivityPlayer.GetComponent<UIActivitySetup>().Activity = newActivity;

                        //Triggers the player to set itself up
                        newActivityPlayer.GetComponent<UIActivitySetup>().LoadActivity();

                        //Add the media player to the list
                        ActivityMediaPlayers.Add(newActivityPlayer);
                        ActivityManager.PlayersList.Add(newActivityPlayer);

                        //Begins loading all the clips
                        newActivity.GetComponent<ActivityClipLoader>().ActivityName = ActivityName;
                        newActivity.GetComponent<ActivityClipLoader>().LoadClips();
                    }
                    else
                    {
                        Debug.LogWarning("Folder not found in specified directory. " + Directory.GetDirectories(FolderPath));
                    }
                }
            }
#endif
        }
    }

    //Ensures theres no button duplicates
    private void RemoveOldStuff()
    {
        //Destroy all the old buttons
        for (int i = 0; i < ActivityButtons.Count; i++)
        {
            ActivityButtons[i].GetComponent<ActivityButtonController>().DestroyMe();
        }
        ActivityButtons.Clear();

        //Destroy all old Audiosources
        for (int i = 0; i < ActivityAudioSources.Count; i++)
        {
            Destroy(ActivityAudioSources[i].gameObject);
            ActivityAudioSources[i].GetComponent<ActivityClipLoader>().Clean();
        }
        ActivityAudioSources.Clear();

        //Clear all the loaded clips
        ActivityClips.Clear();

        //Clear all the integers
        NewActivityInt = 0;
        PreloadedActivities = 0;

        //Sets clean to true and loads the correct buttons back in
        clean = true;
        LoadExistingWavFiles();
    }
}
