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

    private void Start()
    {
        targetFolderPath = "CustomAudio/" + SceneName + "/One-Shots";
    }

    public void OpenFileDialog()
    {
#if UNITY_EDITOR
        // Use UnityEditor file dialog for editor
        selectedFilePath = EditorUtility.OpenFilePanel("Select a WAV File", "", "wav");
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

        // Copy the file
        string fileName = Path.GetFileName(filePath);
        string destinationPath = Path.Combine(targetPath, fileName);

        try
        {
            File.Copy(filePath, destinationPath, true);
            Debug.Log($"File successfully copied to: {destinationPath}");

#if UNITY_EDITOR
            // Refresh the Asset Database so Unity detects the new file
            AssetDatabase.Refresh();
#endif

            // Load the AudioClip and add it to the list
            AddAudioClipToList(fileName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to copy file: {ex.Message}");
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
        AudioClip clip = Resources.Load<AudioClip>(resourcePath);
#endif

        if (clip != null)
        {
            audioClips.Add(clip);
            Debug.Log($"AudioClip successfully added: {clip.name}");
        }
        else
        {
            Debug.LogWarning($"Failed to load AudioClip: {fileName}. Ensure it is in the correct folder.");
        }
    }
}
