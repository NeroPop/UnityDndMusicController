using UnityEditor;
using UnityEngine;

namespace MusicMixer
{
    public class Quit : MonoBehaviour
    {
        public void QuitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
