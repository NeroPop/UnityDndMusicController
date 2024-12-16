using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;

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

    public string SceneName;
    public List<Button> OneshotButtons = new List<Button>();

    [Header("Audio")]
    public List<AudioSource> OneshotAudioSources = new List<AudioSource>();
    public List<AudioClip> Oneshotclips = new List<AudioClip>();

    public string audioFolderPath;

    [Header("New Oneshot Properties")]
    public string OneshotName;
    public int NewOneshotInt;
    private int PreloadedOneshots;
    public bool clean = true;

    private void Start()
    {
        /*// Set up the audio folder path
#if UNITY_EDITOR
        audioFolderPath = $"Assets/StreamingAssets/CustomAudio/{SceneName}/One-Shots";
#else
    audioFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "One-Shots");
#endif
        Debug.Log($"AudioFolderPath set to: {audioFolderPath}");

       */ // Load existing audio files into Oneshotclips
       // LoadAudioFiles();
    }

    public void PlayOneShot(int OneShotNumber)
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
        // Ensure audio files are loaded
        LoadAudioFiles();

        // Ensure that Oneshotclips is populated
        if (Oneshotclips == null || NewOneshotInt - 1 < 0 || NewOneshotInt - 1 >= Oneshotclips.Count)
        {
            Debug.LogError("Invalid index for Oneshotclips. Ensure audio clips are loaded before creating a new oneshot.");
            return;
        }

        // Increment the counter for new oneshots
        NewOneshotInt++;
        Debug.Log("NewOneshotInt is " + NewOneshotInt);

        // Path setup
        string targetPath = Path.Combine(Application.dataPath, audioFolderPath);
        Debug.Log("target path = " + targetPath);
        Debug.Log("audioFolderPath = " + audioFolderPath);

        // Create the new button
        GameObject newOneshotButton = Instantiate(OneshotButtonPrefab, OneshotButtonGroup.transform);
        newOneshotButton.GetComponentInChildren<TMP_Text>().text = OneshotName;
        newOneshotButton.name = "Button " + OneshotName;

        Button buttonComponent = newOneshotButton.GetComponent<Button>();
        OneshotButtons.Add(buttonComponent);

        // Assign the button index
        int buttonIndex = NewOneshotInt + PreloadedOneshots - 1;
        newOneshotButton.GetComponent<OneshotButtonController>().ButtonIndex = buttonIndex;
        buttonComponent.onClick.AddListener(() => PlayOneShot(buttonIndex));

        Debug.Log("buttonIndex is " + buttonIndex);

        // Create the new oneshot game object
        GameObject newOneshot = Instantiate(OneshotPrefab, Oneshots.transform);
        newOneshot.name = OneshotName;

        // Assign the audio clip to the audio source
        OneshotAudioSources.Add(newOneshot.GetComponent<AudioSource>());
        Debug.Log("Oneshotclips Count = " + Oneshotclips.Count);
        newOneshot.GetComponent<AudioSource>().clip = Oneshotclips[NewOneshotInt - 1];

        // Hide the customisation menus
        CustomisationMenuUI.SetActive(false);
        NewOneshotUI.SetActive(false);

        Debug.Log($"Created new Oneshot: {OneshotName} (Index: {buttonIndex})");
    }

    private void LoadAudioFiles()
    {
        Oneshotclips.Clear();

#if UNITY_EDITOR
        Debug.Log($"Loading audio files in Editor from: {audioFolderPath}");
        string[] wavFiles = Directory.GetFiles(audioFolderPath, "*.wav");

        foreach (string filePath in wavFiles)
        {
            string relativePath = filePath.Replace("\\", "/").Replace(Application.dataPath, "Assets");
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);

            if (clip != null)
            {
                Oneshotclips.Add(clip);
                Debug.Log($"Loaded AudioClip: {clip.name}");
            }
            else
            {
                Debug.LogWarning($"Failed to load AudioClip at: {relativePath}");
            }
        }
#else
    Debug.Log($"Loading audio files in Build from: {audioFolderPath}");
    if (Directory.Exists(audioFolderPath))
    {
        string[] wavFiles = Directory.GetFiles(audioFolderPath, "*.wav");
        foreach (string filePath in wavFiles)
        {
            StartCoroutine(LoadAudioClip(filePath));
        }
    }
#endif
    }


    private System.Collections.IEnumerator LoadAudioClip(string filePath)
    {
        string relativePath = filePath.Replace("\\", "/").Replace(Application.dataPath, "Assets");
#if UNITY_EDITOR

#else
        // Use UnityWebRequest to load audio clip at runtime
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + relativePath, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                Oneshotclips.Add(clip);

                Debug.Log($"Loaded AudioClip: {clip.name}");
                AddClipAndCreateButton(clip);
            }
            else
            {
                Debug.LogError($"Failed to load AudioClip: {relativePath}");
            }
        }
#endif
    }

    public void LoadExistingWavFiles()
    {
        if (!clean)
        {
            RemoveOldStuff();
        }
        else
        {
            Debug.Log($"Triggered LoadExistingWavFiles. AudioFolderPath: {audioFolderPath}");

            if (!Directory.Exists(audioFolderPath))
            {
                Debug.LogWarning($"The specified directory does not exist: {audioFolderPath}");
                return;
            }

            string[] wavFiles = Directory.GetFiles(audioFolderPath, "*.wav");
            Debug.Log($"Found {wavFiles.Length} .wav files in {audioFolderPath}");

            foreach (string filePath in wavFiles)
            {
#if UNITY_EDITOR
                string relativePath = filePath.Replace(Application.dataPath, "Assets");
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);

                Debug.Log("Loading Oneshot Files");

                if (clip != null)
                {
                    Oneshotclips.Add(clip);
                    PreloadedOneshots++;

                    // Create a button for each loaded clip
                    GameObject newOneshotButton = Instantiate(OneshotButtonPrefab, OneshotButtonGroup.transform);
                    newOneshotButton.GetComponentInChildren<TMP_Text>().text = clip.name;
                    newOneshotButton.name = "Button " + clip.name;

                    //AudioSource audioSource = newOneshot.GetComponent<AudioSource>();
                  //  audioSource.clip = clip;
                 //   OneshotAudioSources.Add(audioSource);

                    int buttonIndex = OneshotAudioSources.Count - 1;
                   // buttonComponent.onClick.AddListener(() => playOneShot(buttonIndex));

                    Debug.Log($"Loaded Oneshot: {clip.name}");
                }
#else
                StartCoroutine(LoadAudioClip(filePath));
#endif
            }

            Debug.Log("Finished loading oneshot files.");
        }
    }

    private void AddClipAndCreateButton(AudioClip clip)
    {
        Oneshotclips.Add(clip);
        PreloadedOneshots++;

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
        buttonComponent.onClick.AddListener(() => PlayOneShot(buttonIndex));

        Debug.Log($"Loaded Oneshot: {clip.name}");
    }

    private void RemoveOldStuff()
    {
        foreach (var button in OneshotButtons)
        {
            Destroy(button.gameObject);
        }
        OneshotButtons.Clear();

        foreach (var audioSource in OneshotAudioSources)
        {
            Destroy(audioSource.gameObject);
        }
        OneshotAudioSources.Clear();

        Oneshotclips.Clear();

        NewOneshotInt = 0;
        PreloadedOneshots = 0;
        clean = true;

        LoadExistingWavFiles();
    }
}
