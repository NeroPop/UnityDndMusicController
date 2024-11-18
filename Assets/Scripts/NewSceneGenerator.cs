using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSceneGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject ScenePrefab;

    [SerializeField]
    private GameObject SceneButtonPrefab;

    [SerializeField]
    private GameObject ScenesButtonGroup;

    public void NewScene()
    {
        // Instantiate the SceneButtonPrefab as a child of ScenesButtonGroup
        GameObject newSceneButton = Instantiate(SceneButtonPrefab, ScenesButtonGroup.transform);

        // Optionally, reset the local position, rotation, or scale if needed
        newSceneButton.transform.localPosition = Vector3.zero; // Adjust as per requirement
        newSceneButton.transform.localRotation = Quaternion.identity; // Reset rotation if necessary
        newSceneButton.transform.localScale = Vector3.one; // Reset scale if necessary
    }
}
