using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MusicMixer.Ambience
{
    public class AmbienceFileSelector : MonoBehaviour
    {
        [Tooltip("Specify the folder (relative to the Assets folder) where the selected .wav file will be copied.")]
        [HideInInspector] public string targetFolderPath = "AmbientAudioFiles";

        [Tooltip("The selected file path (for debugging purposes).")]
        [HideInInspector] public string selectedFilePath;

        [Tooltip("Scene name to organize custom audio.")]
        [HideInInspector] public string SceneName;

        [Tooltip("List of loaded audio clips.")]
        public List<AudioClip> AmbientaudioClips = new List<AudioClip>();

        [HideInInspector] public string AmbientName;

        [Header("Customisation UI References")]
        [SerializeField] private GameObject ButtonDone;
        [SerializeField] private GameObject ButtonCancel;

        private string MP3FilePath;
        private string wavFilePath;
        private string destinationFileName;
        private string destinationPath;
        private string oldDestinationPath;
        private bool sameName = false;

        //Open the file inspector and select a file
        public void OpenFileDialog()
        {
#if UNITY_EDITOR
            // Setup the audio folder path
            targetFolderPath = "CustomAudio/" + SceneName + "/Ambience";

            // Use UnityEditor file dialog for editor and allow selecting both WAV and MP3 files
            selectedFilePath = EditorUtility.OpenFilePanel("Select an Audio File", "", "wav,mp3");
#else
            // Setup the audio folder path
            targetFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "Ambience");

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

        //Copy the file over to the selected place and rename
        private void CopyFileToTargetFolder(string filePath)
        {
            // Get the AmbientName from the AmbienceManager
            AmbientName = gameObject.GetComponent<AmbienceManager>().AmbientName;

            //Logs errors if there is no target folder or ambient name
            if (string.IsNullOrEmpty(AmbientName))
            {
                Debug.LogError("Ambient is not set. Please provide a valid name.");
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

            // If it's an MP3, convert to WAV before copying
            if (fileExtension.ToLower() == ".mp3")
            {
                try
                {
                    wavFilePath = AudioConverter.ConvertMp3ToWav(filePath, targetPath);
                    Debug.Log($"Converted MP3 to WAV: {wavFilePath}");

                    //Gets the original file name and creates a path to where it'll copy the file before converting || Used for deleting the file after
                    string mp3Name = Path.GetFileNameWithoutExtension(filePath);
                    MP3FilePath = Path.Combine($"Assets/{targetFolderPath}/{mp3Name}.wav");

                    // Set the destination file path to the WAV file
                    filePath = wavFilePath;
                    fileExtension = ".wav"; // Update the extension to WAV
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to convert MP3 to WAV: {ex.Message}");
                    return; // If conversion fails, exit the method
                }
            }

            // Set the destination file name to use AmbientName
            if (Path.GetFileNameWithoutExtension(filePath) != AmbientName) //Check if the written name is the same as the file name
            {
                destinationFileName = $"{AmbientName}{fileExtension}";
                destinationPath = Path.Combine(targetPath, destinationFileName);
            }
            else //Temporarily renames the file with _tmp to ensure its different to the source
            {
                destinationFileName = $"{AmbientName}_tmp{fileExtension}";
                destinationPath = Path.Combine(targetPath, destinationFileName);
                Debug.Log("named new file with tmp");
            }

            try
            {
                // Copy the file and rename it
                File.Copy(filePath, destinationPath, true);

                // Delete the original MP3 file
                if (File.Exists(MP3FilePath))
                {
                    if (MP3FilePath != wavFilePath) //check to make sure the file name isnt the same otherwise it causes an error when deleted
                    {
                        File.Delete(MP3FilePath);
                    }
                }

                //If the ambient name is the same as the file name it renames the ambient back to normal and deletes the old one
                if (Path.GetFileNameWithoutExtension(filePath) == AmbientName)
                {
                    sameName = true;
                    oldDestinationPath = destinationPath;
                    destinationFileName = $"{AmbientName}{fileExtension}";
                    destinationPath = Path.Combine(targetPath, destinationFileName);

                    File.Copy(oldDestinationPath, destinationPath, true);

                    Debug.Log($"Renamed file back to {Path.GetFileNameWithoutExtension(destinationPath)} in {destinationPath}");
                }

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
                    AmbientaudioClips.Add(clip);
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

            if (sameName)
            {
                File.Delete(oldDestinationPath);
                sameName = false;
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
