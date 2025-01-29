using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MusicMixer.Activities
{
    public class ActivityButtonController : MonoBehaviour, IPointerDownHandler
    {
        public int ButtonIndex;

        public void DestroyMe()
        {
            Destroy(gameObject);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Button Was Right Clicked");
            }

        }
    }
}
