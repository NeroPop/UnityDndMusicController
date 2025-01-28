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
using MusicMixer.Activities;

namespace MusicMixer.Ambience
{
    public class AmbienceManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject AmbientPrefab;
        [SerializeField] private GameObject AmbientButtonPrefab;

        [Header("References")]
        [SerializeField] private GameObject AmbientButtonGroup;
        [SerializeField] private GameObject AmbienceParent;
        [SerializeField] private GameObject CustomisationMenuUI;
        [SerializeField] private GameObject NewAmbientUI;
        [SerializeField] private TMP_InputField AmbientNameInput;

        [Header("Lists")]
        public List<Button> AmbientButtons = new List<Button>();
        public List<AudioSource> AmbientAudioSources = new List<AudioSource>();
        public List<AudioClip> AmbientClips = new List<AudioClip>();

        [Header("Ambience Properties")]
        public string SceneName;
        public float FadeDuration = 5;
        public int Amb;
        public string AmbientName;
        public int NewAmbientInt;
        private int PreloadedAmbience;
        private string FilePath;
        private string AudioFolderPath;
        public bool clean = true;


        private void Start()
        {
            LoadExistingWavFiles();
        }

        //Plays or stops playing the Ambient Sound when you click on the button.
        public void ToggleAmbientAudio(int AmbientButton)
        {
            //Ensures the button always plays a sound even if there's more buttons than sound.
            if (AmbientButton >= AmbientAudioSources.Count)
            {
                Amb = AmbientAudioSources.Count - 1;
            }
            else if (AmbientButton < AmbientAudioSources.Count)
            {
                Amb = AmbientButton;
            }

            //Triggers the ambient sound to begin or stop.
            AmbientAudioSources[Amb].GetComponent<AmbientController>().TriggerAmbient();

            AmbientButtons[AmbientButton].GetComponent<ButtonToggle>().ToggleButton();
        }

        //Opens up the New Ambient sound setup menu
        public void SetupNewAmbience()
        {
            CustomisationMenuUI.SetActive(true);
            NewAmbientUI.SetActive(true);
            GetComponent<AmbienceFileSelector>().ControlCustomiseUI(true);
        }

        //Cancels the New Ambient sound setup menu
        public void CancelNew()
        {
            CustomisationMenuUI.SetActive(false);
            NewAmbientUI.SetActive(false);
        }

        //Sets the Ambient Name to whatever you called it
        public void NewAmbientName()
        {
            AmbientName = AmbientNameInput.text;
        }

        public void NewAmbient()
        {
            //Increment the counter for new Ambience
            NewAmbientInt++;

            //Ensure that audio files are loaded
            LoadAudioFiles();

            //Create the button index
            int buttonIndex = NewAmbientInt + PreloadedAmbience - 1; //Use the last index in the list

            //Create a new Ambient button
            NewAmbientButton(buttonIndex);

            //Create the new Ambient game object
            NewAmbientGameObject(AmbientClips[NewAmbientInt - 1]);

            //Hide the customisation menus
            CustomisationMenuUI.SetActive(false);
            NewAmbientUI.SetActive(false);

            Debug.Log($"Created new Ambient: {AmbientName} (Index: {buttonIndex})");
        }

        //Gets the audio clips that were selected from the file selector and saves them to the list
        private void LoadAudioFiles()
        {
            AmbientClips = gameObject.GetComponent<AmbienceFileSelector>()?.AmbientaudioClips;
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
                Debug.Log("Triggered LoadExistingWavFiles");
#if UNITY_EDITOR
                //Sets the File path for in editor
                FilePath = $"Assets/CustomAudio/{SceneName}/Ambience";

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

                    Debug.Log("Loading Ambient Files");

                    if (clip != null)
                    {
                        AmbientClips.Add(clip); //Add the clip to the list of clips

                        PreloadedAmbience++; //Increment the counter for loaded ambient

                        AmbientName = clip.name; //Gets the AmbientName

                        //Create the new Ambient game object
                        NewAmbientGameObject(clip);

                        //Create the button index
                        int buttonIndex = AmbientAudioSources.Count - 1;

                        //Create a new Ambient Button
                        NewAmbientButton(buttonIndex);

                        Debug.Log($"Loaded Ambient: {AmbientName}");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to load AudioClip at path: {relativePath}");
                    }
                }
                Debug.Log("Done loading Ambience at " + FilePath);
#else
            //Sets the File path for in Build
            FilePath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "Ambience");

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

                    AmbientClips.Add(clip); // Add the clip to the list of clips

                    PreloadedAmbience++; // Increment the counter for loaded ambient

                    AmbientName = clip.name; //Gets the AmbientName

                    //Create the new Ambient game object
                    NewAmbientGameObject(clip);

                    //Create the button index
                    int buttonIndex = AmbientAudioSources.Count - 1;

                    //Create a new Ambient Button
                    NewAmbientButton(buttonIndex);

                    Debug.Log($"Loaded Ambient: {AmbientName}");
                }
                else
                {
                    Debug.LogError($"Failed to load AudioClip: {filePath}. Error: {www.error}");
                }
            }
        }

        //Ensures theres no button duplicates
        private void RemoveOldStuff()
        {
            //Destroy all the old buttons
            for (int i = 0; i < AmbientButtons.Count; i++)
            {
                AmbientButtons[i].GetComponent<AmbientButtonController>().DestroyMe();
            }
            AmbientButtons.Clear();

            //Destroy all old Audiosources
            for (int i = 0; i < AmbientAudioSources.Count; i++)
            {
                Destroy(AmbientAudioSources[i].gameObject);
            }
            AmbientAudioSources.Clear();

            //Clear all the loaded clips
            AmbientClips.Clear();

            //Clear all the integers
            NewAmbientInt = 0;
            PreloadedAmbience = 0;

            //Sets clean to true and loads the correct buttons back in
            clean = true;
            LoadExistingWavFiles();
        }

        private void NewAmbientButton(int buttonIndex)
        {
            //Create the new button for the ambience
            GameObject newAmbientButton = Instantiate(AmbientButtonPrefab, AmbientButtonGroup.transform);
            newAmbientButton.GetComponentInChildren<TMP_Text>().text = AmbientName;
            newAmbientButton.name = "Button " + AmbientName;

            //Add the button to the list
            Button buttonComponent = newAmbientButton.GetComponent<Button>();
            AmbientButtons.Add(buttonComponent);

            //Assign the button index
            newAmbientButton.GetComponent<AmbientButtonController>().ButtonIndex = buttonIndex;

            //Ensure that on click it triggers the correct ambience
            buttonComponent.onClick.AddListener(() => ToggleAmbientAudio(buttonIndex));
        }

        private void NewAmbientGameObject(AudioClip clip)
        {
            //Create the new Ambient game object
            GameObject newAmbient = Instantiate(AmbientPrefab, AmbienceParent.transform);
            newAmbient.name = AmbientName;

            //Assign the audio clip to the audio source
            AudioSource audioSource = newAmbient.GetComponent<AudioSource>();
            audioSource.clip = clip;
            AmbientAudioSources.Add(audioSource);

            //Assign the Fade time to the Ambient Controller
            newAmbient.GetComponent<AmbientController>().FadeDuration = FadeDuration;
        }

        public void AmbientFadeUpdate()
        {
            for (int i = 0; i < AmbientAudioSources.Count; i++)
            {
                AmbientAudioSources[i].gameObject.GetComponent<AmbientController>().FadeDuration = FadeDuration;
            }
        }
    }
}
