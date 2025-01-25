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

namespace MusicMixer.Actions
{
    public class OneShotManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject OneshotPrefab;
        [SerializeField] private GameObject OneshotButtonPrefab;

        [Header("References")]
        [SerializeField] private GameObject OneshotButtonGroup;
        [SerializeField] private GameObject Oneshots;
        [SerializeField] private GameObject CustomisationMenuUI;
        [SerializeField] private GameObject NewOneshotUI;
        [SerializeField] private TMP_InputField OneshotNameInput;

        [Header("Lists")]
        public List<Button> OneshotButtons = new List<Button>();
        public List<AudioSource> OneshotAudioSources = new List<AudioSource>();
        public List<AudioClip> Oneshotclips = new List<AudioClip>();

        [Header("Oneshot Properties")]
        [HideInInspector] public string SceneName;
        [HideInInspector] public string OneshotName;
        [HideInInspector] public int NewOneshotInt;
        private int PreloadedOneshots;
        private string FilePath;
        private string audioFolderPath;
        [HideInInspector] public bool clean = true;

        private void Start()
        {
            audioFolderPath = gameObject.GetComponent<OneshotFileSelector>().targetFolderPath;
            //FilePath = "Assets\\CustomAudio\\" + SceneName + "\\One-Shots";
            LoadExistingWavFiles();
        }

        //Plays the Oneshot Sound when you click on the button.
        public void playOneShot(int OneShotNumber)
        {
            OneshotAudioSources[OneShotNumber].Play();
        }

        //Opens up the New Oneshot setup menu
        public void SetupNewOneshot()
        {
            CustomisationMenuUI.SetActive(true);
            NewOneshotUI.SetActive(true);
        }

        //Cancels the New Oneshot setup menu
        public void CancelNew()
        {
            CustomisationMenuUI.SetActive(false);
            NewOneshotUI.SetActive(false);
        }

        //Sets the Oneshot Name to whatever you called it
        public void NewOneShotName()
        {
            OneshotName = OneshotNameInput.text;
        }

        public void NewOneShot()
        {
            // Increment the counter for new oneshots
            NewOneshotInt++;

            // Ensure that audio files are loaded
            LoadAudioFiles();

            // Assign the new button index correctly
            int buttonIndex = NewOneshotInt + PreloadedOneshots - 1; // Use the last index in the list

            //Create a new button for the Oneshot
            NewButtonSetup(buttonIndex);

            //Create the new oneshot game object
            NewActionObject(Oneshotclips[NewOneshotInt - 1]);

            // Hide the customisation menus
            CustomisationMenuUI.SetActive(false);
            NewOneshotUI.SetActive(false);
        }

        // Gets the audio clips that were selected from the file selector and saves them to the list
        private void LoadAudioFiles()
        {
            Oneshotclips = gameObject.GetComponent<OneshotFileSelector>()?.OneshotaudioClips;
        }

        /// <summary>
        /// Loads existing .wav files from the specified FilePath and creates buttons for them.
        /// </summary>
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
                //Sets the File path for in editor
                FilePath = $"Assets/CustomAudio/{SceneName}/One-Shots";

                if (!Directory.Exists(FilePath)) //Checks that the file path exists
                {
                    Debug.LogWarning($"The specified directory does not exist: {FilePath}");
                    return;
                }
                //Finds the wav files in the folder
                string[] wavFiles = Directory.GetFiles(FilePath, "*.wav");

                foreach (string filePath in wavFiles) //Does the following for each wav file
                {
                    //Checks if its in the correct place and then uses it as a clip
                    string relativePath = filePath.Replace(Application.dataPath, "Assets");
                    AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);

                    if (clip != null)
                    {
                        Oneshotclips.Add(clip); //Add the clip to the list of clips

                        PreloadedOneshots++; //Increment the counter for loaded Oneshot

                        //Sets the OneshotName to the clip name
                        OneshotName = clip.name;

                        //Create the new Action game object
                        NewActionObject(clip);

                        //Assign the new button index correctly
                        int buttonIndex = OneshotAudioSources.Count - 1;

                        //Create a new Action Button
                        NewButtonSetup(buttonIndex);
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to load AudioClip at path: {relativePath}");
                    }
                }
#else
             //Sets the File path for in Build
             FilePath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "One-Shots");

            if (!Directory.Exists(FilePath)) //Checks that the file path exists
            {
                Debug.LogWarning($"The specified directory does not exist: {FilePath}");
                return;
            }

            //Finds the wav files in the directory
            string[] Wavfiles = Directory.GetFiles(FilePath, "*.wav");
            Debug.Log($"Found {Wavfiles.Length} .wav files in {FilePath}");

            foreach (string filePath in Wavfiles) //loads the audioclip and instantiates a button for each wav file
            {
                StartCoroutine(LoadAudioClip(filePath));
            }
#endif
                clean = false;
            }
        }

        private IEnumerator LoadAudioClip(string filePath)
        {
            string url = $"file://{filePath}";
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);

                    // Extract file name from the path
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    clip.name = fileName; // Manually set the clip name

                    Oneshotclips.Add(clip);

                    PreloadedOneshots++;

                    //Sets the OneshotName to the clip name
                    OneshotName = clip.name;

                    //Create the new Action game object
                    NewActionObject(clip);

                    //Assign the new button index correctly
                    int buttonIndex = OneshotAudioSources.Count - 1;

                    //Create a new Action Button
                    NewButtonSetup(buttonIndex);
                }
                else
                {
                    Debug.LogError($"Failed to load AudioClip: {filePath}. Error: {www.error}");
                }
            }
        }

        private void RemoveOldStuff()
        {
            //Destroy all the old buttons
            for (int i = 0; i < OneshotButtons.Count; i++)
            {
                OneshotButtons[i].GetComponent<OneshotButtonController>().DestroyMe();
            }
            OneshotButtons.Clear();

            //Destroy all old Audiosources
            for (int i = 0; i < OneshotAudioSources.Count; i++)
            {
                Destroy(OneshotAudioSources[i].gameObject);
            }
            OneshotAudioSources.Clear();

            //Clear all the loaded clips
            Oneshotclips.Clear();

            //Clear all the integers
            NewOneshotInt = 0;
            PreloadedOneshots = 0;

            //Sets clean to true and loads the correct buttons back in
            clean = true;
            LoadExistingWavFiles();
        }

        private void NewButtonSetup(int buttonIndex)
        {
            // Create the new button for the Action
            GameObject newOneshotButton = Instantiate(OneshotButtonPrefab, OneshotButtonGroup.transform);
            newOneshotButton.GetComponentInChildren<TMP_Text>().text = OneshotName;
            newOneshotButton.name = "Button " + OneshotName;

            // Add the button to the list
            Button buttonComponent = newOneshotButton.GetComponent<Button>();
            OneshotButtons.Add(buttonComponent);

            // Assign the button index correctly
            newOneshotButton.GetComponent<OneshotButtonController>().ButtonIndex = buttonIndex;

            // Ensure that on click it plays the correct sound
            buttonComponent.onClick.AddListener(() => playOneShot(buttonIndex));
        }

        private void NewActionObject(AudioClip clip)
        {
            //Create a new Action Gameobject
            GameObject newOneshot = Instantiate(OneshotPrefab, Oneshots.transform);
            newOneshot.name = OneshotName;

            //Assign the audio clip to the audio source
            AudioSource audioSource = newOneshot.GetComponent<AudioSource>();
            audioSource.clip = clip;

            //Add audio source to the list
            OneshotAudioSources.Add(audioSource);
        }
    }
}