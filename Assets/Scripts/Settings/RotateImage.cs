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
            // Rotate around the Z-axis
            rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}

