using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject MusicManager;

    public void ActivateScene()
    {
        MusicManager.GetComponent<UnityActivityManager>().NewScene();
    }
}
