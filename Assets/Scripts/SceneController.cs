using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject MusicManager;

    [SerializeField]
    private TMP_Text SceneTitle;

    public string SceneName;

    private void Start()
    {
        MusicManager.GetComponent<customAudioClipLoader>().Scene = SceneName;
        SceneTitle.text = SceneName;
    }
    public void ActivateScene()
    {
        MusicManager.GetComponent<UnityActivityManager>().NewScene();
    }
}
