using System.Collections;
using UnityEditor;
using UnityEngine;

namespace MusicMixer
{
    public class Quit : MonoBehaviour
    {
        [SerializeField] private AudioSource AudioSource;

        public void QuitApplication()
        {
            AudioSource.Play();
            StartCoroutine(WaitForSFX());
        }

        private IEnumerator WaitForSFX()
        {
            yield return new WaitForSeconds(2);
            ActualQuit();
        }

        private void ActualQuit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
