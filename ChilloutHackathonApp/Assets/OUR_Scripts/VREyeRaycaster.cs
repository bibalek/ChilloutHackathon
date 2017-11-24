using System;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    // In order to interact with objects in the scene
    // this class casts a ray into the scene and if it finds
    // a VRInteractiveItem it exposes it for other classes to use.
    // This script should be generally be placed on the camera.
    public class VREyeRaycaster : MonoBehaviour
    {
        public event Action<RaycastHit> EventOnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.

        [SerializeField]
        private Transform centerCamera;
        [SerializeField]
        private LayerMask exclusionLayers;           // Layers to exclude from the raycast.
        [SerializeField]
        private Reticle reticle;                     // The reticle, if applicable.
        [SerializeField]
        private VRInput vrInput;                     // Used to call input based events on the current VRInteractiveItem.
        [SerializeField]
        private bool showDebugRay;                   // Optionally show the debug ray.
        [SerializeField]
        private float debugRayLenght = 5f;           // Debug ray length.
        [SerializeField]
        private float debugRayDuration = 1f;         // How long the Debug ray will remain visible.
        [SerializeField]
        private float rayLenght = 500f;              // How far into the scene the ray is cast.

        private const int INTERACTABLE_LAYERMASK_NUMBER = 8;

        private VRInteractiveItem currentInteractiveItem;                //The current interactive item
        private VRInteractiveItem lastInteractiveItem;                   //The last interactive item
        private RaycastHit hit;


        // Utility for other classes to get the current interactive item
        public VRInteractiveItem CurrentInteractible
        {
            get { return currentInteractiveItem; }
        }


        private void OnEnable()
        {
            vrInput.OnClick += HandleClick;
            vrInput.OnDoubleClick += HandleDoubleClick;
            vrInput.OnUp += HandleUp;
            vrInput.OnDown += HandleDown;
        }


        private void OnDisable()
        {
            vrInput.OnClick -= HandleClick;
            vrInput.OnDoubleClick -= HandleDoubleClick;
            vrInput.OnUp -= HandleUp;
            vrInput.OnDown -= HandleDown;
        }


        private void Update()
        {
            EyeRaycast();
        }


        private void EyeRaycast()
        {
            // Show the debug ray if required
            if (showDebugRay)
            {
                Debug.DrawRay(centerCamera.position, centerCamera.forward * debugRayLenght, Color.blue, debugRayDuration);
            }

            // Create a ray that points forwards from the camera.
            Ray ray = new Ray(centerCamera.position, centerCamera.forward);
            
            // Do the raycast forweards to see if we hit an interactive item
            if (Physics.Raycast(ray, out hit, rayLenght, ~exclusionLayers))
            {
                VRInteractiveItem interactible = hit.collider.GetComponent<VRInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
                currentInteractiveItem = interactible;

                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != lastInteractiveItem)
                    interactible.Over();

                // Deactive the last interactive item 
                if (interactible != lastInteractiveItem)
                    DeactiveLastInteractible();

                lastInteractiveItem = interactible;

                // Check if interactable item was hit
                if (reticle && hit.collider.gameObject.layer == INTERACTABLE_LAYERMASK_NUMBER)
                    reticle.SetPosition(hit);
                else if (reticle)
                    reticle.SetPositionOnCollider(hit);

                if (EventOnRaycasthit != null)
                {
                    EventOnRaycasthit(hit);
                }
            }

        }

        private void DeactiveLastInteractible()
        {
            if (lastInteractiveItem == null)
                return;

            lastInteractiveItem.Out();
            lastInteractiveItem = null;
        }


        private void HandleUp()
        {
            if (currentInteractiveItem != null)
                currentInteractiveItem.Up();
        }


        private void HandleDown()
        {
            if (currentInteractiveItem != null)
            {
                currentInteractiveItem.Down();
            }
        }


        private void HandleClick()
        {
            if (currentInteractiveItem != null)
            {
                currentInteractiveItem.Click(hit.point);
            }
            //else
            //    FindObjectOfType<MenuManager>().HideMenus();
        }


        private void HandleDoubleClick()
        {
            if (currentInteractiveItem != null)
                currentInteractiveItem.DoubleClick();

        }
    }
}