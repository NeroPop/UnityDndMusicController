using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    //public GameObject[] Scenes;
    public List<GameObject> Scenes = new List<GameObject>();
    public GameObject SceneSelectUI;

    public int CurScene;
    public int PrevScene;
    public int NewSceneInt;

    private void Start()
    {
        CurScene = 0;
        Scenes[CurScene].SetActive(true);
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

            Debug.Log("Selected Scene " + CurScene);
        }
        else if (PrevScene == CurScene)
        {
            SceneSelectUI.SetActive(false);
        }
    }

    public void SelectSceneUI()
    {
        SceneSelectUI.SetActive(true);
    }
}
