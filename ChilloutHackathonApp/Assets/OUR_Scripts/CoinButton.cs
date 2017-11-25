using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class CoinButton : VRInteractiveItem
{
    public Text moneyText;
    public int moneyValue;
    private GameManager manager;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public override void Click(Vector3 clickedPoint)
    {
        moneyText.gameObject.SetActive(true);
        manager.RaiseBet(moneyValue);
        moneyText.text = manager.bet.ToString() + " $";
        Debug.Log(manager.bet);
    }

}
