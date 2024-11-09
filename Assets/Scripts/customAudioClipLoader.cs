using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    public string Scene; //Loads the files for that particular scene (only works in editor atm)

    void Start()
    {
        //being able to test in unity
        if (Application.isEditor) absolutePath = "Assets/CustomAudio/" + Scene;

        ReloadSounds();
    }


    //void Seek(SeekDirection d)
    //{
    //    if (d == SeekDirection.Forward)
    //        currentIndex = (currentIndex + 1) % clips.Count;
    //    else
    //    {
    //        currentIndex--;
    //        if (currentIndex < 0) currentIndex = clips.Count - 1;
    //    }
    //}

    void ReloadSounds()
    {
        clips.Clear();

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

        GetComponent<CustomAudioSetter>().LoadClips();
    }
}
