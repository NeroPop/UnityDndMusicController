using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicMixer.Ambience
{
    public class AmbientButtonController : MonoBehaviour
    {
        public int ButtonIndex;

        public void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
