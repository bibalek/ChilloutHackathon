using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform[] playerCardsPoints;
    [SerializeField]
    private RawConnectionManager connectionManager;
    [SerializeField]
    GameObject foldMsg;
    [SerializeField]
    GameObject foldButton;
    [SerializeField]
    GameObject raiseButton;
    [SerializeField]
    GameObject checkButton;
    [SerializeField]
    GameObject callButton;
    [SerializeField]
    Text moneyText;
    [SerializeField]
    Text enemyMoneyText;
    [SerializeField]
    Text potText;

    private bool handshake = false;
    private bool start = false;
    private List<GameObject> cards = new List<GameObject>();
    private MenuManager menuManager;
    private int myPot = 0;
    private int money = 1000;
    public int enemyMoney = 1000;
    public int bet = 25;
    public int minBet;
    public int maxBet;
    public int pot = 75;
    public bool isDealer = false;
    private int toCall;


    private void Start ()
    {
        menuManager = FindObjectOfType<MenuManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            connectionManager.connected = true;

        if (connectionManager.connected)
        {
            if (start == false)
            {
                start = true;
                StartGame();
            }
            if (handshake == false && connectionManager.handshake)
            {
                SpawnCards(connectionManager.serializer.cards);
                Show2Cards();
                UpdateMoney(connectionManager.serializer.money);
                myPot = connectionManager.serializer.isBigBlind ? 50 : 25;
                isDealer = connectionManager.serializer.isDealer;
                CalcToCall(pot);
                money -= connectionManager.serializer.isBigBlind ? 50 : 25;
                enemyMoney -= connectionManager.serializer.isBigBlind ? 25 : 50;
                handshake = true;
                UpdatedBet();
            }
            if (connectionManager.update)
            {
                connectionManager.update = false;
                if (connectionManager.ptpHeader.integer < 0)
                {
                    if (connectionManager.ptpHeader.integer == -3) Show3Cards();
                    if (connectionManager.ptpHeader.integer == -4) Show4Cards();
                    if (connectionManager.ptpHeader.integer == -5) Show5Cards();
                    if (connectionManager.ptpHeader.integer == -10) NewRound(); //fold
                    if (connectionManager.ptpHeader.integer == -20) EndRound();
                }
                else
                {
                    CalcToCall(connectionManager.ptpHeader.pot);
                   
                    UpdateButtons();
                }
            }
        }   
    }

  

    public void Fold()
    {
        PTPHeader msg = new PTPHeader(0, false, false, false, true, "", pot);
        string serializer = JsonUtility.ToJson(msg);
        connectionManager.Send(serializer);
        StartCoroutine(ShowFoldMsg());
    }

   

    public void Raise()
    {
        CheckBet();
        myPot += bet;
        pot += bet;
        money -= bet;
        PTPHeader msg = new PTPHeader(0, false, true, false, false, "", pot);
        string serializer = JsonUtility.ToJson(msg);
        connectionManager.Send(serializer);
        StartCoroutine(ShowFoldMsg());
    }
    public void Check()
    {
        PTPHeader msg = new PTPHeader(0, false, false, true, false, "", pot);
        string serializer = JsonUtility.ToJson(msg);
        connectionManager.Send(serializer);
        StartCoroutine(ShowFoldMsg());
    }

    public void Call()
    {
        if ( money < toCall )
        {
            myPot += money;
            pot += money;
            money -= money;

            PTPHeader msg = new PTPHeader(-13, true, false, false, false, "", pot);
            string serializer = JsonUtility.ToJson(msg);
            connectionManager.Send(serializer);
        }
        else
        {
            myPot += toCall;
            pot += toCall;
            money -= toCall;

            PTPHeader msg = new PTPHeader(0, true, false, false, false, "", pot);
            string serializer = JsonUtility.ToJson(msg);
            connectionManager.Send(serializer);
        }      
    }

    public void SpawnCards(string cardsName)
    {
        Char delimiter = ';';
        String[] cardNames = cardsName.Split(delimiter);
        for (int i = 0; i < cardNames.Length; i++)
        {
            if (cardNames[i].Length > 0)
            {
                var card = (GameObject)Instantiate(Resources.Load("Cards/" + cardNames[i]));
                var cardParent = new GameObject();
                card.transform.parent = cardParent.transform;
                cards.Add(cardParent);
                cards[i].transform.position = playerCardsPoints[i].position;
                cards[i].transform.rotation = playerCardsPoints[i].rotation;
                cards[i].SetActive(false);
            }           
        }
    }

    public void Show2Cards()
    {
        for (int i = 0; i < 2; i++)
        {
            cards[i].SetActive(true);          
        }
    }

    public void Show3Cards()
    {               
        for (int i=2; i<5; i++) 
        {
            cards[i].SetActive(true);         
        }  
    }

    public void Show4Cards()
    {       
        cards[5].SetActive(true);
    }

    public void Show5Cards()
    {
        cards[6].SetActive(true);
    }

    public void ShowEnemyCards()
    {
        cards[7].SetActive(true);
        cards[8].SetActive(true);
    }

    private IEnumerator ShowCards() //only for test
    {
        SpawnCards("2C;TD;4C;KH;7C;8H;AC;3C;5C");
        Show2Cards();
        yield return new WaitForSeconds(1);
        Show3Cards();
        yield return new WaitForSeconds(1);
        Show4Cards();
        yield return new WaitForSeconds(1);
        Show5Cards();
    }

    private void NewRound()
    {
        connectionManager.handshake = false;
    }

    private IEnumerator ShowFoldMsg()
    {
        if (foldMsg != null)
        {
            foldMsg.SetActive(true);
            yield return new WaitForSeconds(3);
            foldMsg.SetActive(false);
        }
    }

    private void StartGame()
    {
        menuManager.PlayerConnected();
    }

    private void UpdateButtons()
    {
        var colors = foldButton.GetComponent<Button>().colors;
        colors.normalColor = connectionManager.ptpHeader.fold ? Color.white : Color.gray;
        foldButton.GetComponent<BoxCollider>().enabled = connectionManager.ptpHeader.fold;

        colors = raiseButton.GetComponent<Button>().colors;
        colors.normalColor = connectionManager.ptpHeader.raise ? Color.white : Color.gray;
        foldButton.GetComponent<BoxCollider>().enabled = connectionManager.ptpHeader.raise;

        colors = checkButton.GetComponent<Button>().colors;
        colors.normalColor = connectionManager.ptpHeader.check ? Color.white : Color.gray;
        foldButton.GetComponent<BoxCollider>().enabled = connectionManager.ptpHeader.check;

        colors = callButton.GetComponent<Button>().colors;
        colors.normalColor = connectionManager.ptpHeader.call ? Color.white : Color.gray;
        foldButton.GetComponent<BoxCollider>().enabled = connectionManager.ptpHeader.call;
       
    }

    private void UpdateMoney(int newMoney)
    {
        money = newMoney;
        moneyText.text = newMoney.ToString();
    }

    private void CalcToCall(int pot)
    {
        int enemyPot = pot - myPot;
        toCall = enemyPot - myPot;
    }

    private void EndRound()
    {
        if (connectionManager.ptpHeader.call)  // hasdealerwon ?
        {
            if(isDealer)
            {
                money += pot;
                enemyMoney -= pot;
            }
            else
            {
                money -= pot;
                enemyMoney += pot;
            }
        }          
    }

    private void UpdatedBet()
    {
        minBet = toCall;
        if (enemyMoney > money)
        {
            maxBet = money;
        }
        else
        {
            maxBet = enemyMoney;
        }
       
        bet = minBet;
    }

    public void RaiseBet(int value)
    {
        bet += value;
        //CheckBet();
    }

    public void ResetBet()
    {
        bet = connectionManager.serializer.isBigBlind ? 25 : 50;
    }

    private void CheckBet() //Sprawdzac po kliknieciu na suwak
    {
        if (bet > maxBet) bet = maxBet;
        if (bet < minBet) bet = minBet;
    }
}
