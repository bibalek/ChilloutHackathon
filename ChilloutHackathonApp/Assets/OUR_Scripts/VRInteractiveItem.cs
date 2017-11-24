using System;
using UnityEngine;

namespace VRStandardAssets.Utils
{
    // This class should be added to any gameobject in the scene
    // that should react to input based on the user's gaze.
    // It contains events that can be subscribed to by classes that
    // need to know about input specifics to this gameobject.
    public abstract class VRInteractiveItem : MonoBehaviour
    {
        public event Action OnOver;             // Called when the gaze moves over this object
        public event Action OnOut;              // Called when the gaze leaves this object
        public event Action OnClick;            // Called when click input is detected whilst the gaze is over this object.
        public event Action OnDoubleClick;      // Called when double click input is detected whilst the gaze is over this object.
        public event Action OnUp;               // Called when Fire1 is released whilst the gaze is over this object.
        public event Action OnDown;             // Called when Fire1 is pressed whilst the gaze is over this object.

        private Vector3 clickedPoint = Vector3.zero;

        protected bool isOver;
        public bool IsOver
        {
            get { return isOver; }              // Is the gaze currently over this object?
        }

        // The below functions are called by the VREyeRaycaster when the appropriate input is detected.
        // They in turn call the appropriate events should they have subscribers.
        public virtual void Over()
        {
            isOver = true;
            if (OnOver != null)
                OnOver();
        }


        public virtual void Out()
        {
            isOver = false;

            if (OnOut != null)
                OnOut();
        }


        public virtual void Click(Vector3 clickedPoint)
        {
            if (OnClick != null)
                OnClick();
        }

        public virtual void DoubleClick()
        {
            if (OnDoubleClick != null)
                OnDoubleClick();
        }


        public virtual void Up()
        {
            if (OnUp != null)
                OnUp();
        }


        public virtual void Down()
        {
            if (OnDown != null)
                OnDown();
        }        
    }
}