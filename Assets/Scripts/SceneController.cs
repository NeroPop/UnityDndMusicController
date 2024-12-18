using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject MusicManager;

    [SerializeField]
    private TMP_Text SceneTitleTxt;

    [SerializeField]
    private Button SceneTitle;

    public string SceneName;

    [SerializeField]
    private GameObject SceneManager;

    private GameObject SceneSelectCanvas;

    private void Start()
    {
        SceneManager = GameObject.Find("/SceneManager");
        MusicManager.GetComponent<customAudioClipLoader>().Scene = SceneName;
        MusicManager.GetComponent<OneshotFileSelector>().SceneName = SceneName;
        MusicManager.GetComponent<OneShotManager>().SceneName = SceneName;
        SceneTitleTxt.text = SceneName;
        SceneTitle.onClick.AddListener(SceneManager.GetComponent<SceneManager>().SelectSceneUI);
        Debug.Log("Scene " + SceneName + " has started");
    }
    public void ActivateScene()
    {
        MusicManager.GetComponent<UnityActivityManager>().NewScene();
        MusicManager.GetComponent<OneShotManager>().SceneName = SceneName;
        MusicManager.GetComponent<OneShotManager>().LoadExistingWavFiles();
    }

    public void DeactivateScene()
    {
        MusicManager.GetComponent<OneShotManager>().clean = false;
        Debug.Log("Scene Deactivated");
    }
}
