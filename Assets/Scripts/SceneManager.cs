using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //public GameObject[] Scenes;
    public List<GameObject> Scenes = new List<GameObject>();
    public GameObject SceneSelectUI;

    public int CurScene;
    public int PrevScene = 999;
    public int NewSceneInt;

    private void Start()
    {
        if (Scenes.Count > 0)
        {
            CurScene = 0;
            Scenes[CurScene].SetActive(true);
        }
        SceneSelectUI.SetActive(true);
    }
    public void ChangeScene(int SelScene)
    {
        PrevScene = CurScene;
        CurScene = SelScene;

        if (PrevScene != CurScene)
        {
            Scenes[PrevScene].SetActive(false);
            Scenes[CurScene].SetActive(true);
            SceneSelectUI.SetActive(false);
            Scenes[CurScene].GetComponent<SceneController>().ActivateScene();
            Scenes[PrevScene].GetComponent<SceneController>().DeactivateScene();
        }

        else if (PrevScene == CurScene)
        {
            SceneSelectUI.SetActive(false);
            Scenes[CurScene].SetActive(true);
        }
    }

    public void SelectSceneUI()
    {
        SceneSelectUI.SetActive(true);
    }
}
