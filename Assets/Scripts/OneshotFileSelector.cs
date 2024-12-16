using UnityEngine;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class OneshotFileSelector : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Specify the folder (relative to the Assets folder) where the selected .wav file will be copied.")]
    public string targetFolderPath = "OneshotAudioFiles";

    [Tooltip("The selected file path (for debugging purposes).")]
    public string selectedFilePath;

    [Tooltip("Scene name to organize custom audio.")]
    public string SceneName;

    [Header("Audio Clips")]
    [Tooltip("List of loaded audio clips.")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    public string OneshotName;

    public void OpenFileDialog()
    {
#if UNITY_EDITOR
        // Setup the audio folder path
        targetFolderPath = "CustomAudio/" + SceneName + "/One-Shots";

        // Use UnityEditor file dialog for editor
        selectedFilePath = EditorUtility.OpenFilePanel("Select a WAV File", "", "wav");
#else
        // Setup the audio folder path
        targetFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "One-Shots");

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

        // Get the extension of the original file (e.g., ".wav")
        string fileExtension = Path.GetExtension(filePath);

        // Set the destination file name to use OneshotName
        string destinationFileName = $"{OneshotName}{fileExtension}";
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
            AddAudioClipToList(destinationFileName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to copy and rename file: {ex.Message}");
        }
    }

    private void AddAudioClipToList(string fileName)
    {
        string relativePath = Path.Combine("Assets", targetFolderPath, fileName);

#if UNITY_EDITOR
        // Use the AssetDatabase in the editor to load the clip
        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);
#else
        // For builds, use Resources.Load
        string resourcePath = Path.Combine(targetFolderPath, Path.GetFileNameWithoutExtension(fileName));
        AudioClip clip = Resources.Load<AudioClip>(relativePath);
#endif

        if (clip != null)
        {
            audioClips.Add(clip);
            Debug.Log($"AudioClip successfully added: {clip.name}");
        }
        else
        {
            Debug.LogWarning($"Failed to load AudioClip: {fileName} at {relativePath}. Ensure it is in the correct folder.");
        }
    }
}
