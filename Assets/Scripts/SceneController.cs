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

    private GameObject SceneSelectCanvas;

    private void Start()
    {
        SceneSelectCanvas = GameObject.Find("Scene Select Canvas");
        MusicManager.GetComponent<customAudioClipLoader>().Scene = SceneName;
        SceneTitleTxt.text = SceneName;
        SceneTitle.onClick.AddListener(SceneSelect);
    }
    public void ActivateScene()
    {
        MusicManager.GetComponent<UnityActivityManager>().NewScene();
    }

    private void SceneSelect()
    {
        SceneSelectCanvas.SetActive(true);
    }
}
