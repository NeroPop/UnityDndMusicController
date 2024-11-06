using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] AmbientAudioSources;

    public float FadeDuration = 5;

    private int Amb;
    public bool Pressed = false;

    public void PlayAmbientAudio(int AmbientAudio)
    {
        //Ensures the button always plays a sound even if there's more buttons than sound.
        if (AmbientAudio >= AmbientAudioSources.Length)
        {
            Amb = AmbientAudioSources.Length - 1;
        }
        else if (AmbientAudio < AmbientAudioSources.Length)
        {
            Amb = AmbientAudio;
        }

        //Triggers the ambient sound to begin or stop.
        AmbientAudioSources[Amb].GetComponent<AmbientController>().TriggerAmbient();
    }
}
