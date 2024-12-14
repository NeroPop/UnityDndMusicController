using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

public class OneShotManager : MonoBehaviour
{
    [Header("Prefabs")]

    [SerializeField]
    private GameObject OneshotPrefab;

    [SerializeField]
    private GameObject OneshotButtonPrefab;

    [Header("References")]

    [SerializeField]
    private GameObject OneshotButtonGroup;

    [SerializeField]
    private GameObject Oneshots;

    [SerializeField]
    private GameObject CustomisationMenuUI;

    [SerializeField]
    private GameObject NewOneshotUI;

    [SerializeField]
    private TMP_InputField OneshotNameInput;

    public string SceneName;

    public List<Button> OneshotButtons = new List<Button>();

    [Header("Audio")]

    public List<AudioSource> OneshotAudioSources = new List<AudioSource>();

    public List<AudioClip> Oneshotclips = new List<AudioClip>();

    private string audioFolderPath;

    [Header("New Oneshot Properties")]

    public string OneshotName;

    public int NewOneshotInt;

    private int PreloadedOneshots;

    [SerializeField]
    private string FilePath;

    private void Start()
    {
        audioFolderPath = gameObject.GetComponent<OneshotFileSelector>().targetFolderPath;
        //FilePath = "Assets\\CustomAudio\\" + SceneName + "\\One-Shots";
    }

    public void playOneShot(int OneShotNumber)
    {
        OneshotAudioSources[OneShotNumber].Play();
        Debug.Log("Playing Oneshot number " + OneShotNumber);
    }

    public void SetupNewOneshot()
    {
        CustomisationMenuUI.SetActive(true);
        NewOneshotUI.SetActive(true);
    }

    public void NewOneShotName()
    {
        OneshotName = OneshotNameInput.text;
        Debug.Log("New Oneshot Name is " + OneshotName);
    }

    public void NewOneShot()
    {
        // Increment the counter for new oneshots
        NewOneshotInt++;

        // Ensure that audio files are loaded
        LoadAudioFiles();

        // Create the new button for the oneshot
        GameObject newOneshotButton = Instantiate(OneshotButtonPrefab, OneshotButtonGroup.transform);
        newOneshotButton.GetComponentInChildren<TMP_Text>().text = OneshotName;
        newOneshotButton.name = "Button " + OneshotName;

        // Add the button to the list
        Button buttonComponent = newOneshotButton.GetComponent<Button>();
        OneshotButtons.Add(buttonComponent);

        // Assign the button index correctly
        int buttonIndex = NewOneshotInt + PreloadedOneshots - 1; // Use the last index in the list
        newOneshotButton.GetComponent<OneshotButtonController>().ButtonIndex = buttonIndex;

        // Add the click event listener
        buttonComponent.onClick.AddListener(() => playOneShot(buttonIndex));

        // Create the new oneshot game object
        GameObject newOneshot = Instantiate(OneshotPrefab, Oneshots.transform);
        newOneshot.name = OneshotName;

        // Assign the audio clip to the audio source
        OneshotAudioSources.Add(newOneshot.GetComponent<AudioSource>());
        newOneshot.GetComponent<AudioSource>().clip = Oneshotclips[NewOneshotInt - 1];

        Debug.Log($"OneshotButtons.Count: {OneshotButtons.Count}");
        Debug.Log($"ButtonIndex: {buttonIndex}");
        Debug.Log($"Oneshotclips.Count: {Oneshotclips.Count}");
        Debug.Log($"NewOneshotInt: {NewOneshotInt}");
        Debug.Log($"PreloadedOneshots: {PreloadedOneshots}");

        // Assign the audio clip to the audio source
       /* AudioSource audioSource = newOneshot.GetComponent<AudioSource>();
        if (Oneshotclips.Count >= NewOneshotInt)
        {
            audioSource.clip = Oneshotclips[buttonIndex];
            OneshotAudioSources.Add(audioSource);
        }
        else
        {
            Debug.LogWarning($"No audio clip found for index {buttonIndex}");
        } */

        // Hide the customisation menus
        CustomisationMenuUI.SetActive(false);
        NewOneshotUI.SetActive(false);

        Debug.Log($"Created new Oneshot: {OneshotName} (Index: {buttonIndex})");

    }


    private void LoadAudioFiles()
    {
        Oneshotclips = gameObject.GetComponent<OneshotFileSelector>()?.audioClips;
    }

    public void CancelNew()
    {
        CustomisationMenuUI.SetActive(false);
        NewOneshotUI.SetActive(false);
        Debug.Log("Cancelled new Oneshot");
    }

    /// <summary>
    /// Loads existing .wav files from the specified FilePath and creates buttons for them.
    /// </summary>
    public void LoadExistingWavFiles()
    {
        Debug.Log("Triggered LoadExistingWavFiles");

        FilePath = $"Assets/CustomAudio/{SceneName}/One-Shots";

        if (!Directory.Exists(FilePath))
        {
            Debug.LogWarning($"The specified directory does not exist: {FilePath}");
            Debug.Log($"The specified directory does not exist: {FilePath}");
            return;
        }

        string[] wavFiles = Directory.GetFiles(FilePath, "*.wav");

        foreach (string filePath in wavFiles)
        {
            string relativePath = filePath.Replace(Application.dataPath, "Assets");
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);

            Debug.Log("Loading Oneshot Files");

            if (clip != null)
            {
                Oneshotclips.Add(clip);

                PreloadedOneshots++;

                //NewOneshotInt = NewOneshotInt + 1;

                // Create a button for each loaded clip
                GameObject newOneshotButton = Instantiate(OneshotButtonPrefab, OneshotButtonGroup.transform);
                newOneshotButton.GetComponentInChildren<TMP_Text>().text = clip.name;
                newOneshotButton.name = "Button " + clip.name;

                Button buttonComponent = newOneshotButton.GetComponent<Button>();
                OneshotButtons.Add(buttonComponent);

                GameObject newOneshot = Instantiate(OneshotPrefab, Oneshots.transform);
                newOneshot.name = clip.name;

                AudioSource audioSource = newOneshot.GetComponent<AudioSource>();
                audioSource.clip = clip;
                OneshotAudioSources.Add(audioSource);

                int buttonIndex = OneshotAudioSources.Count - 1;
                buttonComponent.onClick.AddListener(() => playOneShot(buttonIndex));

                Debug.Log($"Loaded Oneshot: {clip.name}");
            }
            else
            {
                Debug.LogWarning($"Failed to load AudioClip at path: {relativePath}");
                Debug.Log($"Failed to load AudioClip at path: {relativePath}");
            }
        }

        Debug.Log("Done loading oneshots at " + FilePath);
    }
}
