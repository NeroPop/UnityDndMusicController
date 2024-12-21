using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NewSceneGenerator : MonoBehaviour
{
    [Header("Prefabs")]

    [SerializeField]
    private GameObject ScenePrefab;

    [SerializeField]
    private GameObject SceneButtonPrefab;

    [Header("References")]

    [SerializeField]
    private GameObject ScenesButtonGroup;

    [SerializeField]
    private TMP_InputField SceneNameInput;

    [Header("New Scene Properties")]

    [SerializeField]
    private string NewSceneName;

    public int NewSceneInt;

    private SceneManager SceneManager;

    [SerializeField]
    private string EditorFilePath = "Assets\\CustomAudio";

    private string BuildFilePath = Path.Combine(Application.streamingAssetsPath, "CustomAudio");

    private string folderName;

    private void Start()
    {
        SceneManager = GetComponent<SceneManager>();

#if UNITY_EDITOR
        EditorFilePath = "Assets\\CustomAudio";

        AssetDatabase.Refresh();
        string[] folders = AssetDatabase.FindAssets("t:Folder", new[] { EditorFilePath });

        foreach (string folderGUID in folders)
        {
            // Convert folder GUID to a path
            string folderPath = AssetDatabase.GUIDToAssetPath(folderGUID);

            // Extract folder name
            folderName = System.IO.Path.GetFileName(folderPath);

            // Check if the folder is directly under the specified directory
            if (System.IO.Path.GetDirectoryName(folderPath) == EditorFilePath)
            {
                LoadScene();
            }
            else
            {
               // Debug.LogWarning("Folder not found in specified directory " + System.IO.Path.GetDirectoryName(folderPath));
            }
        }
#else
// Load folders at runtime in build
        if (Directory.Exists(BuildFilePath))
        {
            string[] folders = Directory.GetDirectories(BuildFilePath);

            foreach (string folderPath in folders)
            {
                folderName = Path.GetFileName(folderPath);

                Debug.Log("Found folder " + folderName + " at " + folderPath);

                // Ensure folder exists and is valid
                if (!string.IsNullOrEmpty(folderName))
                {
                    LoadScene();
                }
                else
                {
                    Debug.LogWarning("Folder not found in specified directory. " +  Directory.GetDirectories(BuildFilePath));
                }
            }
        }
        else
        {
            Debug.LogError($"CustomAudio directory not found at: {BuildFilePath}");
        }
#endif
    }

    public void NewScene()
    {
        NewSceneInt = NewSceneInt + 1;

        //sets the scene name to the written text
        NewSceneName = SceneNameInput.text;

        //Instantiate the SceneButtonPrefab as a child of ScenesButtonGroup
        GameObject newSceneButton = Instantiate(SceneButtonPrefab, ScenesButtonGroup.transform);
        newSceneButton.GetComponentInChildren<TMP_Text>().text = NewSceneName;

        //Instantiate the Scene Prefab
        GameObject newScene = Instantiate(ScenePrefab, gameObject.transform);
        newScene.name = NewSceneName;
        newScene.GetComponent<SceneController>().SceneName = NewSceneName;
        SceneManager.Scenes.Add(newScene);
        newScene.SetActive(false);

#if UNITY_EDITOR
        string folderPath = AssetDatabase.GenerateUniqueAssetPath(EditorFilePath + "/" + NewSceneName);

        AssetDatabase.CreateFolder(EditorFilePath, NewSceneName);
        AssetDatabase.Refresh();
#else
         // Build-specific folder creation (runtime)
        string folderPath = Path.Combine(BuildFilePath, NewSceneName);

        // Create the folder at runtime
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
#endif
    }

    private void LoadScene()
    {
        NewSceneInt = NewSceneInt + 1;

        NewSceneName = folderName;

        //Instantiate the SceneButtonPrefab
        GameObject newSceneButton = Instantiate(SceneButtonPrefab, ScenesButtonGroup.transform);
        newSceneButton.GetComponentInChildren<TMP_Text>().text = NewSceneName;

        //Instantiate the ScenePrefab
        GameObject newScene = Instantiate(ScenePrefab, gameObject.transform);
        newScene.name = NewSceneName;
        newScene.GetComponent<SceneController>().SceneName = NewSceneName;

        //Add to the SceneManager list
        SceneManager.Scenes.Add(newScene);
        newScene.SetActive(false);
    }
}
