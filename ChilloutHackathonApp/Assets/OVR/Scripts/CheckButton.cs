using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class CheckButton : VRInteractiveItem
{

    [SerializeField]
    GameManager gameManager;

    public override void Click(Vector3 clickedPoint)
    {
        gameManager.Check();
    }

    public override void Over()
    {
        GetComponent<Image>().color = Color.green;
    }

    public override void Out()
    {
        GetComponent<Image>().color = Color.white;
    }
}
