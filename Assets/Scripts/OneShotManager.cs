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

    [SerializeField]
    private string OneshotName;

    public int NewOneshotInt;

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
        NewOneshotInt = NewOneshotInt + 1;

        LoadAudioFiles();

        GameObject newOneshotButton = Instantiate(OneshotButtonPrefab, OneshotButtonGroup.transform);
        newOneshotButton.GetComponentInChildren<TMP_Text>().text = OneshotName;
        newOneshotButton.name = "Button " + OneshotName;
        OneshotButtons.Add(newOneshotButton.GetComponent<Button>());
        newOneshotButton.GetComponent<OneshotButtonController>().ButtonIndex = NewOneshotInt - 1;
        OneshotButtons[NewOneshotInt - 1].onClick.AddListener(() => playOneShot(newOneshotButton.GetComponent<OneshotButtonController>().ButtonIndex));

        GameObject newOneshot = Instantiate(OneshotPrefab, Oneshots.transform);
        newOneshot.name = OneshotName;
        OneshotAudioSources.Add(newOneshot.GetComponent<AudioSource>());
        newOneshot.GetComponent<AudioSource>().clip = Oneshotclips[NewOneshotInt - 1];

        CustomisationMenuUI.SetActive(false);
        NewOneshotUI.SetActive(false);
        Debug.Log("Created new Oneshot " + NewOneshotInt);
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

                NewOneshotInt = NewOneshotInt + 1;

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
