using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UnityActivityManager : MonoBehaviour
{
    [Header("Audio Sources")]

    [SerializeField]
    private AudioSource ExploreAudio;

    [SerializeField]
    private AudioSource CombatAudio;

    [SerializeField]
    private AudioSource VictoryAudio;

    [Header("Audio Mixers")]

    public AudioMixerGroup ExploreMixer;

    public AudioMixerGroup CombatMixer;

    public AudioMixerGroup VictoryMixer;
}
