using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneButtonScript : MonoBehaviour
{
    private GameObject SceneManager;
    public int NewSceneInt;
    private void Start()
    {
        SceneManager = GameObject.Find("/SceneManager");
        NewSceneInt = SceneManager.GetComponent<NewSceneGenerator>().NewSceneInt;

        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.GetComponent<SceneManager>().ChangeScene(NewSceneInt);
        });
    }
}
