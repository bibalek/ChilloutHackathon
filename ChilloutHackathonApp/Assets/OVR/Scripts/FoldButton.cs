using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class FoldButton : VRInteractiveItem
{

    [SerializeField]
    GameManager gameManager;

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public override void Click(Vector3 clickedPoint)
    {
        gameManager.Fold();
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
