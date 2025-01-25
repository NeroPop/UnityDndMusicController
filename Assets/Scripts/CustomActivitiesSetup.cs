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

namespace MusicMixer.Activities
{
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
        private UnityActivityManager ActivityManager;

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
        public bool clean;
        private bool UIActivityCheck;
        private GameObject newActivity;

        private void Start()
        {
            ActivityManager = gameObject.GetComponent<UnityActivityManager>();
            LoadExistingWavFiles();
        }

        public void PlayActivity(int ActivityNumber)
        {
            //Triggers the activity through ActivityManager
            ActivityManager.TriggerActivity(ActivityNumber);
        }

        public void SetupNewActivity() //Opens up the New Activity sound setup menu
        {
            CustomisationMenuUI.SetActive(true);
            NewActivityUI.SetActive(true);
            GetComponent<ActivityFileSelector>().ControlCustomiseUI(true);
        }

        public void CancelNew() //Cancels the New Activity sound setup menu
        {
            CustomisationMenuUI.SetActive(false);
            NewActivityUI.SetActive(false);

        }

        public void NewActivityName() //Sets the Activity Name to whatever you called it
        {
            ActivityName = ActivityNameInput.text;
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

            //Create the button index
            int buttonIndex = NewActivityInt + PreloadedActivities; //Use the last index in the list

            //Create a new Activity Button
            NewActivityButton(buttonIndex);

            //Create the new Activity game object
            NewActivityGameObject();

            //Create a new media player for the activity
            NewActivityPlayer();

            //Start new activity clip loader
            gameObject.GetComponent<ActivityFileSelector>().NewActivityClipLoader();

            //Hide the customisation menus
            CustomisationMenuUI.SetActive(false);
            NewActivityUI.SetActive(false);
        }

        //Loads existing .wav files from the specified FilePath and creates buttons for them. triggered by scene controller
        public void LoadExistingWavFiles()
        {
            //Checks if buttons have already been loaded
            if (!clean)
            {
                RemoveOldStuff();
            }

            else
            {
#if UNITY_EDITOR
                //Sets the Folder path for in editor
                FolderPath = "Assets\\CustomAudio\\" + SceneName + "\\Activities";
                AssetDatabase.Refresh();
                string[] folders = AssetDatabase.FindAssets("t:Folder", new[] { FolderPath });

                foreach (string folderGUID in folders)
                {
                    // Convert folder GUID to a path
                    string folderPath = AssetDatabase.GUIDToAssetPath(folderGUID);

                    // Extract folder name
                    FolderName = System.IO.Path.GetFileName(folderPath);
                    ActivityName = FolderName;

                    PreloadedActivities++; //Increment the counter for loaded activities

                    //Assign the button index correctly
                    int buttonIndex = PreloadedActivities; //Use the last index in the list

                    //Create a new Activity Button
                    NewActivityButton(buttonIndex);

                    //Create a new Activity game object
                    NewActivityGameObject();

                    //Create a new media player for the activity
                    NewActivityPlayer();

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

            if (!Directory.Exists(FolderPath)) //Checks that the file path exists
            {
                Debug.LogWarning($"The specified directory does not exist: {FolderPath}");
                return;
            }

            if (Directory.Exists(FolderPath))
            {
                string[] folders = Directory.GetDirectories(FolderPath);

                foreach (string folderPath in folders)
                {
                    FolderName = Path.GetFileName(folderPath);
                    ActivityName = FolderName;

                    // Ensure folder exists and is valid
                    if (!string.IsNullOrEmpty(FolderName))
                    {
                        //Sets everything up
                        PreloadedActivities++; //Increment the counter for loaded activities

                        //Assign the button index correctly
                        int buttonIndex = PreloadedActivities; //Use the last index in the list

                        //Create a new Activity Button
                        NewActivityButton(buttonIndex);

                        //Create a new Activity game object
                        NewActivityGameObject();

                        //Create a new media player for the activity
                        NewActivityPlayer();

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
                ActivityAudioSources[i].gameObject.GetComponent<ActivityController>().DestroyMe();
            }
            ActivityAudioSources.Clear();

            //Destroy all old Media players
            for (int i = 0; i < ActivityMediaPlayers.Count; i++)
            {
                ActivityMediaPlayers[i].GetComponent<UIActivitySetup>().DestroyMe();
            }
            ActivityMediaPlayers.Clear();

            //Clear all media players from player controller
            PlayerParent.GetComponent<UIPlayerController>().ActivityPlayers.Clear();

            //Clear all the loaded clips
            ActivityClips.Clear();

            //Clear all the integers
            NewActivityInt = 0;
            PreloadedActivities = 0;

            //Clears the lists in ActivityManager
            ActivityManager.ActivitiesList.Clear();
            ActivityManager.PlayersList.Clear();
            ActivityManager.ActivityButtons.Clear();

            //Sets clean to true and loads the correct buttons back in
            clean = true;
            LoadExistingWavFiles();
        }

        private void NewActivityButton(int buttonIndex)
        {
            //Create the new button for the Activity
            GameObject newActivityButton = Instantiate(ActivityButtonPrefab, ActivityButtonGroup.transform);
            newActivityButton.GetComponentInChildren<TMP_Text>().text = ActivityName;
            newActivityButton.name = "Button " + ActivityName;

            //Add the button to the list
            Button buttonComponent = newActivityButton.GetComponent<Button>();
            ActivityButtons.Add(buttonComponent);

            //Add the button to the Activity Manager list
            ActivityManager.ActivityButtons.Add(newActivityButton);

            //Assign the button index
            newActivityButton.GetComponent<ActivityButtonController>().ButtonIndex = buttonIndex;

            //Ensure that on click it triggers the correct Activity
            buttonComponent.onClick.AddListener(() => PlayActivity(buttonIndex));
        }

        private void NewActivityGameObject()
        {
            //Create the new Activity game object
            newActivity = Instantiate(ActivityPrefab, ActivitiesParent.transform);
            newActivity.name = ActivityName;

            //Assign the new Activity to the Activity Manager and FileSelector list
            ActivityManager.ActivitiesList.Add(newActivity);
            gameObject.GetComponent<ActivityFileSelector>().NewActivity = newActivity;

            //Assign the audio clip to the audio source and set the volume to 0
            ActivityAudioSources.Add(newActivity.GetComponent<AudioSource>());
            newActivity.GetComponent<AudioSource>().volume = 0;
        }

        private void NewActivityPlayer()
        {
            //Create a new Media Player for the activity
            GameObject newActivityPlayer = Instantiate(ActivityPlayerPrefab, PlayerParent.transform);
            newActivityPlayer.name = ActivityName + " Media Player";
            newActivityPlayer.GetComponent<UIActivitySetup>().Activity = newActivity;

            //Triggers the player to set itself up
            newActivityPlayer.GetComponent<UIActivitySetup>().LoadActivity();

            //Add the media player to the list
            ActivityMediaPlayers.Add(newActivityPlayer);
            ActivityManager.PlayersList.Add(newActivityPlayer);

            //Adds media player to Player Controller list
            PlayerParent.GetComponent<UIPlayerController>().ActivityPlayers.Add(newActivityPlayer);

            //disable the media player
            newActivityPlayer.SetActive(false);
        }
    }
}