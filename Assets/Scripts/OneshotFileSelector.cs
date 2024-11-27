using UnityEngine;
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
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to copy file: {ex.Message}");
        }
    }
}
