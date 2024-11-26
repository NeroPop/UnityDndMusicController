using UnityEngine;

public class OneShotManager : MonoBehaviour
{
    [Header("Prefabs")]

    [SerializeField]
    private GameObject OneShotPrefab;

    [SerializeField]
    private GameObject OneShotButtonPrefab;

    [Header("References")]

    [SerializeField]
    private GameObject OneShotButtonGroup;

    [Header("Audio")]

    [SerializeField]
    private AudioSource[] OneshotAudioSources;

    public void playOneShot(int OneShotNumber)
    {
        OneshotAudioSources[OneShotNumber].Play();
    }
}