using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class TestInteractableItem : VRInteractiveItem
{
    public static event Action<Vector3, GameObject> EventInteract;

    public override void Over()
    {
        base.Over();
        Debug.Log("Pacze na obiekt");
    }

    public override void Click(Vector3 clickedPoint)
    {
        Destroy(gameObject);
    }

}
