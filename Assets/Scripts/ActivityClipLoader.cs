using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class ActivityClipLoader : MonoBehaviour
{
    [Header("Activity Properties")]
    public List<AudioClip> ActivityClips = new List<AudioClip>();
    public string ActivityName;

    [Header("References")]
    [HideInInspector] public string ActivityFolderPath = "ActivityAudioFiles";
    [HideInInspector] public string SceneName;

    private void Start()
    {
       // LoadClips();
    }

    public void LoadClips()
    {
        Debug.Log("LoadClips triggered in " + ActivityName);
        ActivityController ActivityController = gameObject.GetComponent<ActivityController>();
        SceneName = GetComponentInParent<SceneNameHolder>().SceneName;
#if UNITY_EDITOR
        //Sets the folder path to find the audio clips
        ActivityFolderPath = "Assets/CustomAudio/" + SceneName + "/Activities/" + ActivityName;
        Debug.Log("ActivityFolderPath: " + ActivityFolderPath);

        //Finds the wav files in the folder
        string[] wavFiles = Directory.GetFiles(ActivityFolderPath, "*.wav");

        foreach (string filePath in wavFiles) //Does the following for each wav file
        {
            //Checks if its in the correct place and then uses it as a clip
            string relativePath = filePath.Replace(Application.dataPath, "Assets");
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);

            if (clip != null)
            {
                ActivityClips.Add(clip); //Add the clip to the list of clips
                Debug.Log("Number of ActivityClips: " + ActivityClips.Count);
                ActivityController.Tracks.Add(clip); //Add the clip to the Activity Controllers list of clips
                Debug.Log("Added Clip " + clip.name);
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogWarning($"Failed to load AudioClip at path: {relativePath} Failed when loading clips");
            }
        }

#else
        //Sets the folder path to find the audio clips
        ActivityFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "Activities", ActivityName);

        //Finds the wav files in the directory
        string[] Wavfiles = Directory.GetFiles(ActivityFolderPath, "*.wav");

        foreach (string filePath in Wavfiles) //loads the audioclip and adds them to the ActivityController
        {
            StartCoroutine(LoadAudioClip(filePath));
        }
#endif
        ActivityController.loadCustomTrack(); //Load the custom tracks in ActivityController
    }

    private IEnumerator LoadAudioClip(string filePath)
    {
        string url = $"file://{filePath}";
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);

                // Extract file name from the path
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                clip.name = fileName; // Manually set the clip name

                ActivityClips.Add(clip); // Add the clip to the list of clips
                Debug.Log("Number of ActivityClips: " + ActivityClips.Count);
                gameObject.GetComponent<ActivityController>().Tracks.Add(clip); //Add the clip to the Activity Controllers list of clips
                Debug.Log("Added Clip " + clip.name);
            }
            else
            {
                Debug.LogError($"Failed to load AudioClip: {filePath}. Error: {www.error}");
            }
        }
    }
}
