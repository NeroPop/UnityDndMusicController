using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudioSetter : MonoBehaviour
{
    [SerializeField] private GameObject activityParent;
    [SerializeField] private List<GameObject> activities = new List<GameObject>();
    
    [SerializeField] AudioSource[] activitiesSources;
    private AudioSource[] _oneShots;
    private AudioSource[] _ambiance;

    public void LoadClips()
    {
        

        foreach (Transform child in activityParent.transform)
        {
            activities.Add(child.gameObject);
        }

        int actLeanth = GetComponent<customAudioClipLoader>().clips.Count;
        //print(actLeanth);
        for (int i = 0; i < actLeanth; i++)
        {
            activities[0].GetComponent<ActivityController>().Tracks.Add(GetComponent<customAudioClipLoader>().clips[i]);
            activities[0].GetComponent<ActivityController>().loadCustomTrack();
            activities[0].GetComponent<AudioSource>().Stop();
            activities[0].GetComponent<AudioSource>().Play();
        }
    }
}
