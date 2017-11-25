using UnityEngine;
using UnityEngine.UI;

namespace VRStandardAssets.Utils
{
    // The reticle is a small point at the centre of the screen.
    // It is used as a visual aid for aiming. The position of the
    // reticle is either at a default position in space or on the
    // surface of a VRInteractiveItem as determined by the VREyeRaycaster.
    public class Reticle : MonoBehaviour
    {
        [SerializeField] private float defaultDistance = 5f;      // The default distance away from the camera the reticle is placed.
        [SerializeField] private bool useNormal;                  // Whether the reticle should be placed parallel to a surface.
        [SerializeField] private Transform centerCamera;          // The reticle is always placed relative to the camera.

        private Image reticleImage;                               // Reference to the image component that represents the reticle.
        private Transform reticleTransform;                       // We need to affect the reticle's transform.
        private Animator animator;
        private Vector3 originalScale;                            // Since the scale of the reticle changes, the original scale needs to be stored.
        private Quaternion originalRotation;                      // Used to store the original rotation of the reticle.

        private const string ANIMATION_POPUP_BOOL = "Popup";

        public bool UseNormal
        {
            get { return useNormal; }
            set { useNormal = value; }
        }

        public Transform ReticleTransform { get { return reticleTransform; } }

        private void Awake()
        {
            // Store the original scale and rotation.
            reticleImage = GetComponent<Image>();
            animator = GetComponent<Animator>();
            reticleTransform = transform;
            originalScale = reticleTransform.localScale;
            originalRotation = reticleTransform.localRotation;
        }


        public void Hide()
        {
            reticleImage.enabled = false;
        }


        public void Show()
        {
            reticleImage.enabled = true;
        }


        // This overload of SetPosition is used when the VREyeRaycaster has hit anything with collider
        public void SetPositionOnCollider (RaycastHit hit)
        {
            reticleImage.transform.position = hit.point;
            animator.SetBool(ANIMATION_POPUP_BOOL, false);
        }


        // This overload of SetPosition is used when the VREyeRaycaster has hit interactable
        public void SetPosition (RaycastHit hit)
        {
            reticleImage.transform.position = hit.point;
            animator.SetBool(ANIMATION_POPUP_BOOL, true);
        }
    }
}