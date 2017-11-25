using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class VRPlayButton : VRInteractiveItem
{
    MenuManager menuManager;
    Animator animatorReference;

    private void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        animatorReference = GetComponent<Animator>();
        animatorReference.SetTrigger("Show");
    }

    public override void Over()
    {
        base.Over();
        animatorReference.SetBool("Moving", true);
    }

    public override void Out()
    {
        base.Out();
        animatorReference.SetBool("Moving", false);
    }

    public override void Click(Vector3 clickedPoint)
    {
        menuManager.PrepareAndStart();
    }

}
