using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicMixer.Activities
{
    public class ActivityButtonController : MonoBehaviour
    {
        public int ButtonIndex;

        public void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
