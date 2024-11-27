using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public List<Button> OneshotButtons = new List<Button>();

    [Header("Audio")]

    public List<AudioSource> OneshotAudioSources = new List<AudioSource>();

    public List<AudioClip> Oneshotclips = new List<AudioClip>();

    private string audioFolderPath;

    [Header("New Oneshot Properties")]

    [SerializeField]
    private string OneshotName;

    public int NewOneshotInt;

    private void Start()
    {
        audioFolderPath = gameObject.GetComponent<OneshotFileSelector>().targetFolderPath;
    }

    public void playOneShot(int OneShotNumber)
    {
        OneshotAudioSources[OneShotNumber].Play();
        Debug.Log("Playing Oneshot number " +  OneShotNumber);
    }

    public void SetupNewOneshot()
    {
        CustomisationMenuUI.SetActive(true);
        NewOneshotUI.SetActive(true);
    }

    public void NewOneShotName()
    {
        OneshotName = OneshotNameInput.text;
        Debug.Log("New Oneshot Name is " +  OneshotName);
    }

    public void NewOneShot()
    {
        NewOneshotInt = NewOneshotInt + 1;

        LoadAudioFiles();

        GameObject newOneshotButton = Instantiate(OneshotButtonPrefab, OneshotButtonGroup.transform);
        newOneshotButton.GetComponentInChildren<TMP_Text>().text = OneshotName;
        newOneshotButton.name = "Button " + OneshotName;
        OneshotButtons.Add(newOneshotButton.GetComponent<Button>());
        OneshotButtons[NewOneshotInt - 1].onClick.AddListener(() => playOneShot(NewOneshotInt - 1));

        GameObject newOneshot = Instantiate(OneshotPrefab, Oneshots.transform);
        newOneshot.name = OneshotName;
        OneshotAudioSources.Add(newOneshot.GetComponent<AudioSource>());
        newOneshot.GetComponent<AudioSource>().clip = Oneshotclips[NewOneshotInt - 1];

        CustomisationMenuUI.SetActive(false);
        NewOneshotUI.SetActive(false);
        Debug.Log("Created new Oneshot");
    }

    private void LoadAudioFiles()
    {
       Oneshotclips = gameObject.GetComponent<OneshotFileSelector>().audioClips;
    }

    public void CancelNew()
    {
        CustomisationMenuUI.SetActive(false);
        NewOneshotUI.SetActive(false);
        Debug.Log("Cancelled new Oneshot");
    }
}