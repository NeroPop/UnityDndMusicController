using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActivityFileSelector : MonoBehaviour
{
    [Tooltip("Specify the folder (relative to the Assets folder) where the selected .wav file will be copied.")]
    [HideInInspector] public string targetFolderPath = "ActivityAudioFiles";

    [Tooltip("The selected file path (for debugging purposes).")]
    [HideInInspector] public string selectedFilePath;

    [Tooltip("Scene name to organize custom audio.")]
    public string SceneName;

    [Tooltip("List of loaded audio clips.")]
    public List<AudioClip> ActivityaudioClips = new List<AudioClip>();

    [Tooltip("New Activity")]
    public GameObject NewActivity;

    [HideInInspector] public string ActivityName;

    public void OpenFileDialog() //Open the file inspector and select a file
    {
#if UNITY_EDITOR
        // Setup the audio folder path
        targetFolderPath = "CustomAudio/" + SceneName + "/Activities/" + ActivityName;

        // Use UnityEditor file dialog for editor and select only wav files
        selectedFilePath = EditorUtility.OpenFilePanel("Select a WAV File", "", "wav");
#else
        // Setup the audio folder path
        targetFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "Activities", ActivityName);

        // Use System.Windows.Forms for standalone builds and select only wav files
        using (var fileDialog = new System.Windows.Forms.OpenFileDialog())
        {
            fileDialog.Filter = "WAV Files (*.wav)|*.wav";
            fileDialog.Title = "Select a WAV File";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedFilePath = fileDialog.FileName;
            }
        }
#endif
        // If a file is selected, copy it to the target folder
        if (!string.IsNullOrEmpty(selectedFilePath))
        {
            CopyFileToTargetFolder(selectedFilePath);
        }
        else
        {
            Debug.LogWarning("No file was selected.");
        }
    }

    private void CopyFileToTargetFolder(string filePath) //Copy the file over to the selected place and rename
    {
        // Get the ActivityName from the CustomActivitiesSetup
        ActivityName = gameObject.GetComponent<CustomActivitiesSetup>().ActivityName;

        //Logs errors if there is no target folder or Activity name
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
        string targetPath = Path.Combine(Application.dataPath, targetFolderPath, ActivityName);

        // Ensure the folder exists
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }

        // Get the extension of the original file (e.g., ".wav")
        string fileExtension = Path.GetExtension(filePath);

        // Set the destination file name to use ActivityName
        string destinationFileName = $"{ActivityName}{fileExtension}";
        string destinationPath = Path.Combine(targetPath, destinationFileName);

        try
        {
            // Copy the file and rename it
            File.Copy(filePath, destinationPath, true);
            Debug.Log($"File successfully copied and renamed to: {destinationPath}");

#if UNITY_EDITOR
            // Refresh the Asset Database so Unity detects the new file
            AssetDatabase.Refresh();
#endif
            // Load the renamed AudioClip and add it to the list
            StartCoroutine(AddAudioClipToList(destinationFileName));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to copy and rename file: {ex.Message}");
        }
    }

    private IEnumerator AddAudioClipToList(string fileName)
    {
        string relativePath = Path.Combine("Assets", targetFolderPath, ActivityName, fileName);

#if UNITY_EDITOR
        // Use the AssetDatabase in the editor to load the clip
        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);

        if (clip != null)
        {
            //Assign the clip to the various other scripts
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
                //Assign the clip to the local list and the Tracks list
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
}
