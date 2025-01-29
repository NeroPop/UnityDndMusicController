using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using System.Drawing.Text;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MusicMixer.Activities
{
    public class ActivityFileSelector : MonoBehaviour
    {
        [Tooltip("Specify the folder (relative to the Assets folder) where the selected .wav files will be copied.")]
        [HideInInspector] public string targetFolderPath = "ActivityAudioFiles";

        [Tooltip("The selected file paths (for debugging purposes).")]
        [HideInInspector] public List<string> selectedFilePaths = new List<string>();

        [Tooltip("Scene name to organize custom audio.")]
        [HideInInspector] public string SceneName;

        [Tooltip("List of loaded audio clips.")]
        public List<AudioClip> ActivityaudioClips = new List<AudioClip>();

        [HideInInspector] public GameObject NewActivity;
        [HideInInspector] public string ActivityName;

        [Header("Customisation UI References")]
        [SerializeField] private GameObject ButtonDone;
        [SerializeField] private GameObject ButtonCancel;

        private string MP3FilePath;
        private string wavFilePath;
        private string destinationFileName;
        private string destinationPath;
        private string oldDestinationPath;
        private string oldFileName;
        private bool sameName = false;

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
                string filePath = EditorUtility.OpenFilePanel("Select an Audio File", "", "wav,mp3");
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

            // Use System.Windows.Forms for standalone builds and select both WAV and MP3 files
            using (var fileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                fileDialog.Filter = "Audio Files (*.wav;*.mp3)|*.wav;*.mp3";
                fileDialog.Title = "Select an Audio File";
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

                //Temporarily renames the file with _tmp to ensure its different to the source
                oldFileName = Path.GetFileNameWithoutExtension(filePath);
                destinationFileName = $"{oldFileName}_tmp{fileExtension}";
                destinationPath = Path.Combine(targetPath, destinationFileName);
                Debug.Log("named new file with tmp");
            }
            else
            {
                // Get the original file name and set the path
                destinationFileName = Path.GetFileName(filePath);
                destinationPath = Path.Combine(targetPath, destinationFileName);
            }

            try
            {
                // Ensure the file is completely free before copying
                System.Threading.Thread.Sleep(100); // Small delay

                // Copy the file without renaming
                File.Copy(filePath, destinationPath, true);
                Debug.Log($"File successfully copied: {destinationPath}");

                // Delete the original MP3 file
                if (File.Exists(MP3FilePath))
                {
                    File.Delete(MP3FilePath);
                }

                if (fileExtension.ToLower() == ".mp3")
                {
                    //Renames the Activity back to normal and deletes the old one
                    oldDestinationPath = destinationPath;
                    destinationFileName = $"{oldFileName}{fileExtension}";
                    destinationPath = Path.Combine(targetPath, destinationFileName);
                    File.Copy(oldDestinationPath, destinationPath, true);

                    //Deletes the duplicate file
                    File.Delete(oldDestinationPath);
                    Debug.Log($"Renamed file back to normal and deleted old file");
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
                Debug.LogError($"Failed to copy file: {ex.Message}");
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
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileUrl, audioType))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    if (clip != null)
                    {
                        clip.name = Path.GetFileNameWithoutExtension(filePath);
                        ActivityaudioClips.Add(clip);
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
            }

            // Force Garbage Collection to ensure file release
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            ControlCustomiseUI(false);
        }

        public void NewActivityClipLoader()
        {
            //Set each audio clip into ActivityController
            foreach (AudioClip clip in ActivityaudioClips)
            {
                NewActivity.GetComponent<ActivityController>().Tracks.Add(clip);
            }

            //Clears all the clips in the list
            ActivityaudioClips.Clear();
            gameObject.GetComponent<CustomActivitiesSetup>().ActivityClips.Clear();
            selectedFilePaths.Clear();
        }

        //Function to enable or disable the cancel and done buttons on the customisation interface
        public void ControlCustomiseUI(bool active)
        {
            if (active)
            {
                ButtonDone.SetActive(true);
                ButtonCancel.SetActive(true);
            }
            else
            {
                ButtonDone.SetActive(false);
                ButtonCancel.SetActive(false);
            }
        }
    }
}
