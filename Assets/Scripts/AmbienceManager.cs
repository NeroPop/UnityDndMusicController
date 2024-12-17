using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [HideInInspector] public string SceneName;
    public float FadeDuration = 5;
    public int Amb;
    public string AmbientName;
    private int PreloadedAmbience;
    private string FilePath;
    private string AudioFolderPath;
    public bool clean = true;

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
}
