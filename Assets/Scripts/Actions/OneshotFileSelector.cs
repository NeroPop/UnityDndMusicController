using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MusicMixer.Actions
{
    public class OneshotFileSelector : MonoBehaviour
    {
        [Tooltip("Specify the folder (relative to the Assets folder) where the selected audio file will be copied.")]
        [HideInInspector] public string targetFolderPath = "OneshotAudioFiles";

        [Tooltip("The selected file path (for debugging purposes).")]
        [HideInInspector] public string selectedFilePath;

        [Tooltip("Scene name to organize custom audio.")]
        [HideInInspector] public string SceneName;

        [Tooltip("List of loaded audio clips.")]
        public List<AudioClip> OneshotaudioClips = new List<AudioClip>();

        [HideInInspector] public string OneshotName;

        [Header("Customisation UI References")]
        [SerializeField] private GameObject ButtonDone;
        [SerializeField] private GameObject ButtonCancel;

        public void OpenFileDialog()
        {
#if UNITY_EDITOR
            // Setup the audio folder path
            targetFolderPath = "CustomAudio/" + SceneName + "/One-Shots";

            // Use UnityEditor file dialog for editor and allow selecting both WAV and MP3 files
            selectedFilePath = EditorUtility.OpenFilePanel("Select an Audio File", "", "wav,mp3");
#else
        // Setup the audio folder path
        targetFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "One-Shots");

        // Use System.Windows.Forms for standalone builds and select both WAV and MP3 files
        using (var fileDialog = new System.Windows.Forms.OpenFileDialog())
        {
            fileDialog.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3";
            fileDialog.Title = "Select an Audio File";

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

            // Get the extension of the original file (e.g., ".wav" or ".mp3")
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
                StartCoroutine(AddAudioClipToList(destinationFileName));
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to copy and rename file: {ex.Message}");
            }
        }

        private IEnumerator AddAudioClipToList(string fileName)
        {
            string filePath = Path.Combine(Application.dataPath, targetFolderPath, fileName);
            string fileUrl = "file:///" + filePath.Replace("\\", "/");

            // Determine the audio type (WAV or MP3) based on file extension
            string fileExtension = Path.GetExtension(fileName).ToLower();
            AudioType audioType = fileExtension == ".mp3" ? AudioType.MPEG : AudioType.WAV;

            // Load the audio clip using UnityWebRequest
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileUrl, audioType);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    OneshotaudioClips.Add(clip);
                    Debug.Log($"AudioClip successfully added: {clip.name}");
                }
                else
                {
                    Debug.LogWarning($"Failed to load AudioClip from path {filePath}. Ensure the file is in the correct folder.");
                }
            }
            else
            {
                Debug.LogError($"Error loading AudioClip: {www.error}");
            }

            ControlCustomiseUI(false);
        }

        public void ControlCustomiseUI(bool active)
        {
            ButtonDone.SetActive(active);
            ButtonCancel.SetActive(active);
        }
    }
}
