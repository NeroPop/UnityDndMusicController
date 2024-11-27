using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    [Header("Audio")]

    public List<AudioSource> OneshotAudioSources = new List<AudioSource>();

    private string FilePath;

    [Header("New Oneshot Properties")]

    [SerializeField]
    private string OneshotName;

    [SerializeField]
    private int NewOneshotInt;

    private void Start()
    {
        FilePath = gameObject.GetComponent<OneshotFileSelector>().targetFolderPath;
    }

    public void playOneShot(int OneShotNumber)
    {
        OneshotAudioSources[OneShotNumber].Play();
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

        GameObject newOneshotButton = Instantiate(OneshotButtonPrefab, OneshotButtonGroup.transform);
        newOneshotButton.GetComponentInChildren<TMP_Text>().text = OneshotName;

        GameObject newOneshot = Instantiate(OneshotPrefab, Oneshots.transform);
        newOneshot.name = OneshotName;
        //newOneshot.GetComponent<AudioSource>().clip = 

        CustomisationMenuUI.SetActive(false);
        NewOneshotUI.SetActive(false);
        Debug.Log("Created new Oneshot");
    }

    public void CancelNew()
    {
        CustomisationMenuUI.SetActive(false);
        NewOneshotUI.SetActive(false);
        Debug.Log("Cancelled new Oneshot");
    }
}