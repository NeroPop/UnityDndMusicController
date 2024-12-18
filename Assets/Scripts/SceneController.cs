using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject MusicManager;

    [SerializeField]
    private GameObject Activities;

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
        SceneManager = GameObject.Find("/SceneManager"); //Finds Scene Manager

        //Sets the Scene Name for various scripts
        MusicManager.GetComponent<UnityActivityManager>().SceneName = SceneName;
        Activities.GetComponent<SceneNameHolder>().SceneName = SceneName;
        //MusicManager.GetComponent<customAudioClipLoader>().Scene = SceneName;
        MusicManager.GetComponent<OneshotFileSelector>().SceneName = SceneName;
        MusicManager.GetComponent<AmbienceFileSelector>().SceneName = SceneName;
        MusicManager.GetComponent<OneShotManager>().SceneName = SceneName;
        MusicManager.GetComponent<AmbienceManager>().SceneName = SceneName;
        SceneTitleTxt.text = SceneName;

        //Adds listener to the scene title button for Scene switching
        SceneTitle.onClick.AddListener(SceneManager.GetComponent<SceneManager>().SelectSceneUI);
        Debug.Log("Scene " + SceneName + " has started");
    }
    public void ActivateScene() //Triggered when a scene is activated
    {
        //Gives Activity Manager the Scene name and triggers it to restart
        MusicManager.GetComponent<UnityActivityManager>().SceneName = SceneName;
        MusicManager.GetComponent<UnityActivityManager>().NewScene();

        //Sets the activities scene name
        Activities.GetComponent<SceneNameHolder>().SceneName = SceneName;

        //Gives Oneshot Manager the Scene name and triggers it to load existing Oneshots
        MusicManager.GetComponent<OneShotManager>().SceneName = SceneName;
        MusicManager.GetComponent<OneShotManager>().LoadExistingWavFiles();

        //Gives Ambience Manager the Scene name and triggers it to load existing Ambience
        MusicManager.GetComponent<AmbienceManager>().SceneName = SceneName;
        MusicManager.GetComponent<AmbienceManager>().LoadExistingWavFiles();
    }

    public void DeactivateScene() //Triggered when the scene is deactivated
    {
        //Tells the Oneshots and Ambience managers that they need to be cleaned when loaded back in
        MusicManager.GetComponent<OneShotManager>().clean = false;
        MusicManager.GetComponent<AmbienceManager>().clean = false;
        Debug.Log("Scene Deactivated");
    }
}
