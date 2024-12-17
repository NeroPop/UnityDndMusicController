using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmbienceManager : MonoBehaviour
{
    [Header("Lists")]
    public List<AudioSource> AmbientAudioSources = new List<AudioSource>();
    public List<Button> AmbientButtons = new List<Button>();

    [Header("Ambience Properties")]
    public float FadeDuration = 5;
    private int Amb;

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
