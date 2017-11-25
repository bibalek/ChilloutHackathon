using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class ResetBetButton : VRInteractiveItem
{
    public Text moneyText;
    private GameManager manager;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public override void Click(Vector3 clickedPoint)
    {
        manager.ResetBet();
        moneyText.text = manager.bet.ToString() + " $";
    }

}
