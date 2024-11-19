using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneButtonScript : MonoBehaviour
{
    private GameObject SceneManager;

    [SerializeField]
    private int siblingIndex;
    private void Start()
    {
        SceneManager = GameObject.Find("/SceneManager");

        // Get the index of this GameObject under its parent
        siblingIndex = transform.GetSiblingIndex();

        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.GetComponent<SceneManager>().ChangeScene(siblingIndex -1);
        });
    }
}
