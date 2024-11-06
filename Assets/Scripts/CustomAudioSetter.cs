using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudioSetter : MonoBehaviour
{
    [SerializeField] AudioSource[] activities;
    private AudioSource[] _oneShots;
    private AudioSource[] _ambiance;

    public void LoadClips()
    {
        print("Loaded");

        int actLeanth = GetComponent<customAudioClipLoader>().clips.Count;
        for (int i = 0; i < actLeanth; i++)
        {
            activities[i].clip = GetComponent<customAudioClipLoader>().clips[i];
            activities[i].GetComponent<AudioSource>().Stop();
            activities[i].GetComponent<AudioSource>().Play();


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
