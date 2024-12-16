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
        // Ensure SceneName is assigned before continuing
        if (string.IsNullOrEmpty(SceneName))
        {
            Debug.LogWarning("SceneName is not set. Aborting scene activation.");
            return;
        }

        Debug.Log($"Activating Scene: {SceneName}");

        // Notify UnityActivityManager of the new scene
        MusicManager.GetComponent<UnityActivityManager>().NewScene();

        // Set the SceneName for the OneShotManager
        var oneShotManager = MusicManager.GetComponent<OneShotManager>();
        oneShotManager.SceneName = SceneName;

        // Set the SceneName for OneshotFileSelector
        var oneShotFileSelector = MusicManager.GetComponent<OneshotFileSelector>();
        oneShotFileSelector.SceneName = SceneName;

        // Ensure audioFolderPath is initialized correctly
#if UNITY_EDITOR
        oneShotManager.audioFolderPath = $"Assets/StreamingAssets/CustomAudio/{SceneName}/One-Shots";
#else
    oneShotManager.audioFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomAudio", SceneName, "One-Shots");
#endif

        Debug.Log($"Audio folder path set to: {oneShotManager.audioFolderPath}");

        // Attempt to load existing wav files
        oneShotManager.LoadExistingWavFiles();

        //Setup the OneshotFileSelector script
        oneShotFileSelector.Initialise();
    }

    public void DeactivateScene()
    {
        MusicManager.GetComponent<OneShotManager>().clean = false;
        Debug.Log("Scene Deactivated");
    }
}
