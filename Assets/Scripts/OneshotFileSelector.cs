using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

public class OneshotFileSelector : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Specify the folder (relative to StreamingAssets) where the selected .wav file will be copied.")]
    public string targetFolderPath;

    [Tooltip("The selected file path (for debugging purposes).")]
    public string selectedFilePath;

    [Tooltip("Scene name to organize custom audio.")]
    public string SceneName;

    [Header("Audio Clips")]
    [Tooltip("List of loaded audio clips.")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    public string OneshotName;

    public void Initialise()
    {
        // Set up the audio folder path
#if UNITY_EDITOR
        targetFolderPath = $"Assets/StreamingAssets/CustomAudio/{SceneName}\\One-Shots";
#else
    targetFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "One-Shots");
#endif
        // Ensure the folder exists
        if (!Directory.Exists(targetFolderPath))
        {
            Directory.CreateDirectory(targetFolderPath);
            Debug.Log($"Created target folder: {targetFolderPath}");
        }
    }

    public void OpenFileDialog()
    {
#if UNITY_EDITOR
        // Use UnityEditor file dialog for editor
        selectedFilePath = UnityEditor.EditorUtility.OpenFilePanel("Select a WAV File", "", "wav");
#else
        // Use System.Windows.Forms for standalone builds
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

    private void CopyFileToTargetFolder(string filePath)
    {
        // Get the OneshotName from the OneShotManager
        OneshotName = gameObject.GetComponent<OneShotManager>().OneshotName;

        if (string.IsNullOrEmpty(OneshotName))
        {
            Debug.LogError("OneshotName is not set. Please provide a valid name.");
            return;
        }

        // Get the extension of the original file (e.g., ".wav")
        string fileExtension = Path.GetExtension(filePath);

        // Set the destination file name to use OneshotName
        string destinationFileName = $"{OneshotName}{fileExtension}";
        string destinationPath = Path.Combine(targetFolderPath, destinationFileName);

        try
        {
            // Copy the file and rename it
            File.Copy(filePath, destinationPath, true);
            Debug.Log($"File successfully copied and renamed to: {destinationPath}");

            // Load the renamed AudioClip and add it to the list
            AddAudioClipToList(destinationPath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to copy and rename file: {ex.Message}");
        }
    }

    private void AddAudioClipToList(string filePath)
    {
        StartCoroutine(LoadAudioClip(filePath));
    }

    private IEnumerator LoadAudioClip(string filePath)
    {
        string url = "file://" + filePath;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioClips.Add(clip);
                Debug.Log($"AudioClip successfully added: {clip.name}");
            }
            else
            {
                Debug.LogWarning($"Failed to load AudioClip from {filePath}: {www.error}");
            }
        }
    }
}
