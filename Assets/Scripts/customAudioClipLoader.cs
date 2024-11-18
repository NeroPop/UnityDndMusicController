using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class customAudioClipLoader : MonoBehaviour
{
    //public enum SeekDirection { Forward, Backward }

    public List<AudioClip> clips = new List<AudioClip>();

    [SerializeField][HideInInspector] private int currentIndex = 0;

    private FileInfo[] soundFiles;
    private List<string> validExtensions = new List<string> { ".ogg", ".wav" }; // Don't forget the "." i.e. "ogg" won't work - cause Path.GetExtension(filePath) will return .ext, not just ext.
    public string absolutePath = "./Activities"; // relative path to where the app is running - change this to "./music" in your case
    [HideInInspector] public string Scene; //Loads the files for that particular scene (only works in editor atm)
    

    private int loadedFildCounter;    
    private int activtiyNum;
    private int curantFile = 0;


    void Start()
    {

        //being able to test in unity
        if (Application.isEditor) absolutePath = "Assets/CustomAudio/" + Scene;

        //Adds every activity to the activites list based on the number of children under activityParent
        foreach (Transform child in activityParent.transform)
        {
            activities.Add(child.gameObject);
        }

        //Gets number of activtiy folders witin the scene
        DirectoryInfo dir = new DirectoryInfo(absolutePath);
        DirectoryInfo[] info = dir.GetDirectories("*.*");
        int count = dir.GetDirectories().Length;
        for (int i = 0; i < count; i++)
        {
            Debug.Log("Found Directory: " + info[i]);
            activtiyNum++;
        }

        //sets the clips for each activtiy for the number of activitynums
        for (int i = 0;i < activtiyNum; i++)
        {
            clips.Clear();
            loadedFildCounter = 0;
            absolutePath = "Assets/CustomAudio/" + Scene + "/Activities/" + curantFile;
            ReloadSounds();
        }

        temp();
    }

    void ReloadSounds()
    {
        

        // get all valid files
        var info = new DirectoryInfo(absolutePath);
        soundFiles = info.GetFiles()
            .Where(f => IsValidFileType(f.Name))
            .ToArray();

        // and load them
        foreach (var s in soundFiles)
            StartCoroutine(LoadFile(s.FullName));
    }

    bool IsValidFileType(string fileName)
    {
        return validExtensions.Contains(Path.GetExtension(fileName));
        // Alternatively, you could go fileName.SubString(fileName.LastIndexOf('.') + 1); that way you don't need the '.' when you add your extensions
    }

    

    IEnumerator LoadFile(string path)
    {
        WWW www = new WWW("file://" + path);
        print("loading " + path);

        AudioClip clip = www.GetAudioClip(false);
        while (!clip.isReadyToPlay)
            yield return www;

        print("done loading");
        clip.name = Path.GetFileName(path);
        clips.Add(clip);

 
        loadedFildCounter++; //counts the number of sound files loaded and compares it to the total number of files in the folder. Once all clips are loaded it triggers the allClipsLoaded Method.
        if (loadedFildCounter >= soundFiles.Length)
        {
            print("Sound Files Leanth: " + soundFiles.Length);
            customAudioSetter();
        }
           

       
    }

    //--------------------------------------------------------------------------------

    [Header("Audio Setter")]
    [SerializeField] private GameObject activityParent;
    [SerializeField] private List<GameObject> activities = new List<GameObject>();

    [SerializeField] AudioSource[] activitiesSources;


    void customAudioSetter()
    {
        
        int actLeanth = clips.Count; //gets the number of clips to be used in for loop
        print("ActLeanth: " + actLeanth);
        activities[curantFile].GetComponent<ActivityController>().Tracks.Clear();
        for (int i = 0; i < actLeanth; i++)
        {
            activities[curantFile].GetComponent<ActivityController>().Tracks.Add(clips[i]);
            print("Clip: " + clips[i]);

        }
        activities[curantFile].GetComponent<ActivityController>().loadCustomTrack();
    }


    //--------------------------------------------------------------------------------

    [Header("Activtiy Builder")]
    [SerializeField] private GameObject ActivityButtonGroup;
    [SerializeField] private GameObject ActivityPreFab;

    void temp()
    {
        Instantiate(ActivityPreFab, ActivityButtonGroup.transform);
    }
}
