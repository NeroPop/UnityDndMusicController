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
    private GameObject CustomisationMenuUI;

    [SerializeField]
    private GameObject NewOneshotUI;

    [SerializeField]
    private TMP_InputField OneshotNameInput;

    [Header("Audio")]

    [SerializeField]
    private AudioSource[] OneshotAudioSources;

    [Header("New Oneshot Properties")]

    [SerializeField]
    private string OneshotName;

    [SerializeField]
    private int NewOneshotInt;

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