using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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

    public void NewScene()
    {
        NewSceneName = SceneNameInput.text;

        // Instantiate the SceneButtonPrefab as a child of ScenesButtonGroup
        GameObject newSceneButton = Instantiate(SceneButtonPrefab, ScenesButtonGroup.transform);
        //Changes the button name text
        newSceneButton.GetComponentInChildren<TMP_Text>().text = NewSceneName;

       //newSceneButton.name = NewSceneName; not sure this is necessary (changes the button name in unity hierarchy)
    }
}
