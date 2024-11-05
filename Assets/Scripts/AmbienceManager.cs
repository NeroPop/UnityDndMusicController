using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] AmbientAudioSources;

    public float FadeDuration = 5;

    public void PlayAmbientAudio(int AmbientAudio)
    {
        AmbientAudioSources[AmbientAudio].GetComponent<AmbientController>().TriggerAmbient();
    }
}
