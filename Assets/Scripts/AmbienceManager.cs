using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] AmbientAudioSources;

    [SerializeField]
    private GameObject[] AmbientButtons;

    public float FadeDuration = 5;

    private int Amb;
    public bool Pressed = false;

    public void PlayAmbientAudio(int AmbientButton)
    {
        //Ensures the button always plays a sound even if there's more buttons than sound.
        if (AmbientButton >= AmbientAudioSources.Length)
        {
            Amb = AmbientAudioSources.Length - 1;
        }
        else if (AmbientButton < AmbientAudioSources.Length)
        {
            Amb = AmbientButton;
        }

        //Triggers the ambient sound to begin or stop.
        AmbientAudioSources[Amb].GetComponent<AmbientController>().TriggerAmbient();

        AmbientButtons[AmbientButton].GetComponent<ButtonToggle>().ToggleButton();
    }
}
