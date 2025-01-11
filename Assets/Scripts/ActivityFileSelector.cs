using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActivityFileSelector : MonoBehaviour
{
    [Tooltip("Specify the folder (relative to the Assets folder) where the selected .wav files will be copied.")]
    [HideInInspector] public string targetFolderPath = "ActivityAudioFiles";

    [Tooltip("The selected file paths (for debugging purposes).")]
    [HideInInspector] public List<string> selectedFilePaths = new List<string>();

    [Tooltip("Scene name to organize custom audio.")]
    public string SceneName;

    [Tooltip("List of loaded audio clips.")]
    public List<AudioClip> ActivityaudioClips = new List<AudioClip>();

    public GameObject NewActivity;
    [HideInInspector] public string ActivityName;

    public void OpenFileDialog() // Open the file inspector and select files
    {
        //Sets the activity name
        ActivityName = gameObject.GetComponent<CustomActivitiesSetup>().ActivityName;

#if UNITY_EDITOR
        // Setup the audio folder path
        targetFolderPath = "CustomAudio/" + SceneName + "/Activities/" + ActivityName;

        bool selectMoreFiles = true;

        // Use a loop to allow selecting multiple files
        while (selectMoreFiles)
        {
            string filePath = EditorUtility.OpenFilePanel("Select a WAV File", "", "wav");
            if (!string.IsNullOrEmpty(filePath))
            {
                selectedFilePaths.Add(filePath);
            }

            // Ask if the user wants to select more files
            selectMoreFiles = EditorUtility.DisplayDialog("Select More Files?", "Do you want to select another WAV file?", "Yes", "No");
        }
#else
    // Setup the audio folder path
    targetFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "Activities", ActivityName);

    // Use System.Windows.Forms for standalone builds and select multiple wav files
    using (var fileDialog = new System.Windows.Forms.OpenFileDialog())
    {
        fileDialog.Filter = "WAV Files (*.wav)|*.wav";
        fileDialog.Title = "Select WAV Files";
        fileDialog.Multiselect = true;

        if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            selectedFilePaths.AddRange(fileDialog.FileNames);
        }
    }
#endif

        // If files are selected, copy them to the target folder
        if (selectedFilePaths.Count > 0)
        {
            foreach (string filePath in selectedFilePaths)
            {
                CopyFileToTargetFolder(filePath);
            }
        }
        else
        {
            Debug.LogWarning("No files were selected.");
        }
    }

    private void CopyFileToTargetFolder(string filePath) // Copy the file to the selected place
    {
        // Logs errors if there is no target folder or Activity name
        if (string.IsNullOrEmpty(ActivityName))
        {
            Debug.LogError("Activity is not set. Please provide a valid name.");
            return;
        }
        if (string.IsNullOrEmpty(targetFolderPath))
        {
            Debug.LogError("Target folder path is not set.");
            return;
        }

        // Create the full path in the Unity project
        string targetPath = Path.Combine(Application.dataPath, targetFolderPath);

        // Ensure the folder exists
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }

        // Get the original file name
        string fileName = Path.GetFileName(filePath);
        string destinationPath = Path.Combine(targetPath, fileName);

        try
        {
            // Copy the file without renaming
            File.Copy(filePath, destinationPath, true);
            Debug.Log($"File successfully copied: {destinationPath}");

#if UNITY_EDITOR
            // Refresh the Asset Database so Unity detects the new file
            AssetDatabase.Refresh();
#endif
            // Load the AudioClip and add it to the list
            StartCoroutine(AddAudioClipToList(fileName));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to copy file: {ex.Message}");
        }
    }

    private IEnumerator AddAudioClipToList(string fileName)
    {
        string relativePath = Path.Combine("Assets", targetFolderPath, fileName);

#if UNITY_EDITOR
        // Use the AssetDatabase in the editor to load the clip
        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);

        if (clip != null)
        {
            // Assign the clip to the various other scripts
            gameObject.GetComponent<CustomActivitiesSetup>().ActivityClips.Add(clip);
            ActivityaudioClips.Add(clip);

            Debug.Log($"AudioClip successfully added: {clip.name}");
        }
        else
        {
            Debug.LogWarning($"Failed to load AudioClip: {fileName} at {relativePath}. Ensure it is in the correct folder.");
        }
#else
        // For builds, load the audio clip using UnityWebRequest
        string filePath = Path.Combine(Application.streamingAssetsPath, targetFolderPath, fileName);

        // Ensure the file path starts with file:// for UnityWebRequest
        string fileUrl = "file:///" + filePath.Replace("\\", "/");

        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.WAV);
        yield return www.SendWebRequest(); // Wait for the request to finish

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            if (clip != null)
            {
                // Assign the clip to the local list and the Tracks list
                ActivityaudioClips.Add(clip);
                NewActivity.GetComponent<ActivityController>().Tracks.Add(clip);
            }
            else
            {
                Debug.LogWarning($"Failed to load AudioClip: {fileName} from path {filePath}. Ensure the file is in the correct folder.");
            }
        }
        else
        {
            Debug.LogWarning($"Failed to load AudioClip: {fileName} from path {filePath}. Error: {www.error}");
        }
#endif
        yield break; // Ensures the coroutine exits cleanly
    }

    public void NewActivityClipLoader()
    {
        //Set each audio clip into ActivityController
        foreach (AudioClip clip in ActivityaudioClips)
        {
            NewActivity.GetComponent<ActivityController>().Tracks.Add(clip);
        }
    }
}
