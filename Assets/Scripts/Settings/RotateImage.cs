using UnityEngine;

namespace MusicMixer
{
    public class RotateImage : MonoBehaviour
    {
        // Rotation speed in degrees per second
        [SerializeField] private float rotationSpeed = 100f;

        private RectTransform rectTransform;

        void Start()
        {
            // Cache the RectTransform for efficiency
            rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            if (gameObject.activeInHierarchy) //checks that the gameobject is active so we don't rotate it when its not being used
            {
                // Rotate around the Z-axis
                rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
        }
    }
}

