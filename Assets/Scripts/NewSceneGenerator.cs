using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    private string FilePath = "Assets\\CustomAudio";

    private string folderName;

    private void Start()
    {
        SceneManager = GetComponent<SceneManager>();

#if UNITY_EDITOR
        AssetDatabase.Refresh();
        string[] folders = AssetDatabase.FindAssets("t:Folder", new[] { FilePath });

        foreach (string folderGUID in folders)
        {
            // Convert folder GUID to a path
            string folderPath = AssetDatabase.GUIDToAssetPath(folderGUID);

            // Extract folder name
            folderName = System.IO.Path.GetFileName(folderPath);

            // Check if the folder is directly under the specified directory
            if (System.IO.Path.GetDirectoryName(folderPath) == FilePath)
            {
                LoadScene(); //loads the scene
            }
        }
#endif
    }

    public void NewScene()
    {
        NewSceneInt = NewSceneInt + 1;

        //sets the scene name to the written text
        NewSceneName = SceneNameInput.text;

        // Instantiate the SceneButtonPrefab as a child of ScenesButtonGroup
        GameObject newSceneButton = Instantiate(SceneButtonPrefab, ScenesButtonGroup.transform);

        //Changes the button name text
        newSceneButton.GetComponentInChildren<TMP_Text>().text = NewSceneName;

        GameObject newScene = Instantiate(ScenePrefab, gameObject.transform);
        newScene.name = NewSceneName;
        newScene.GetComponent<SceneController>().SceneName = NewSceneName;
        SceneManager.Scenes.Add(newScene);
        newScene.SetActive(false);

#if UNITY_EDITOR
        string folderPath = AssetDatabase.GenerateUniqueAssetPath(FilePath + "/" + NewSceneName);

        AssetDatabase.CreateFolder(FilePath, NewSceneName);
        AssetDatabase.Refresh();
#endif
    }

    private void LoadScene()
    {
        NewSceneInt = NewSceneInt + 1;

        NewSceneName = folderName;

        // Instantiate the SceneButtonPrefab as a child of ScenesButtonGroup
        GameObject newSceneButton = Instantiate(SceneButtonPrefab, ScenesButtonGroup.transform);

        //Changes the button name text
        newSceneButton.GetComponentInChildren<TMP_Text>().text = NewSceneName;

        GameObject newScene = Instantiate(ScenePrefab, gameObject.transform);
        newScene.name = NewSceneName;
        newScene.GetComponent<SceneController>().SceneName = NewSceneName;
        SceneManager.Scenes.Add(newScene);
        newScene.SetActive(false);
    }
}
