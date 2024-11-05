using UnityEngine;

public class OneShotManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] OneshotAudioSources;

    public void playOneShot(int OneShotNumber)
    {
        OneshotAudioSources[OneShotNumber].Play();
    }
}
