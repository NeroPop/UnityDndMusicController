using TMPro;
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

    [SerializeField]
    private int NewSceneInt;

    private SceneManager SceneManager;

    private void Start()
    {
        SceneManager = GetComponent<SceneManager>();
    }

    public void NewScene()
    {
        NewSceneInt = NewSceneInt + 1;

        //sets the scene name to the written text
        NewSceneName = SceneNameInput.text;

        // Instantiate the SceneButtonPrefab as a child of ScenesButtonGroup
        GameObject newSceneButton = Instantiate(SceneButtonPrefab, ScenesButtonGroup.transform);

        newSceneButton.GetComponent<Button>().onClick.AddListener(() =>
        {
           SceneManager.ChangeScene(NewSceneInt);
        });

        //Changes the button name text
        newSceneButton.GetComponentInChildren<TMP_Text>().text = NewSceneName;
        //newSceneButton.name = NewSceneName; not sure this is necessary (changes the button name in unity hierarchy

        GameObject newScene = Instantiate(ScenePrefab, gameObject.transform);
        newScene.name = NewSceneName;
        newScene.GetComponent<SceneController>().SceneName = NewSceneName;
        SceneManager.Scenes.Add(newScene);
        newScene.SetActive(false);
    }
}
