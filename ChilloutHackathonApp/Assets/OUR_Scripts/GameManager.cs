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
    [SerializeField]
    Text myPotText;
    [SerializeField]
    Text enemyPotText;

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
    public int pot = 0;
    public bool isDealer = false;
    public int deltaPot = 75;
    public int enemyPot = 0;
    private int toCall;


    private void Start ()
    {

        menuManager = FindObjectOfType<MenuManager>();
        menuManager.ShowCards.AddListener(ShowStartCards);
    }

    private void ShowStartCards()
    {
        StartCoroutine(ShowCards());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            connectionManager.connected = true;

 //       if (connectionManager.connected)
//        {
            if (start == false)
            {
                start = true;
                StartGame();
            }
            //if (handshake == false && connectionManager.handshake)
            if (handshake == false)
            {
            //SpawnCards(connectionManager.serializer.cards);
          
            //SpawnCards("AD;KD");
                //Show2Cards();
                //money = connectionManager.serializer.money;

                // myPot = connectionManager.serializer.isBigBlind ? 50 : 25;
                //myPot = 0;
                //deltaPot = 75;
                //isDealer = connectionManager.serializer.isDealer;
               
                //money -= connectionManager.serializer.isBigBlind ? 50 : 25;
                //myPot = connectionManager.serializer.isBigBlind ? 50 : 25; ;
                //enemyMoney -= connectionManager.serializer.isBigBlind ? 25 : 50;
                //enemyPot = connectionManager.serializer.isBigBlind ? 25 : 50;
                //CalcToCall(pot);
                //UpdateMoney();
                handshake = true;
                //UpdatedBet();
            }
            if (connectionManager.update)
            {
                //UpdateMoney();
                UpdateButtons();
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
                   
                   
                }
            }
//        }   
    }

  

    public void Fold()
    {
        PTPHeader msg = new PTPHeader(0, false, false, false, true, "", pot, deltaPot);
        string serializer = JsonUtility.ToJson(msg);
        connectionManager.Send(serializer);
        StartCoroutine(ShowFoldMsg());
    }

   

    public void Raise()
    {
        CheckBet();
        myPot += bet;
        deltaPot += bet;
        money -= bet;
       
        PTPHeader msg = new PTPHeader(0, false, true, false, false, "", pot, deltaPot);
        string serializer = JsonUtility.ToJson(msg);
        connectionManager.Send(serializer);

        StartCoroutine(ShowFoldMsg());
    }
    public void Check()
    {
        PTPHeader msg = new PTPHeader(0, false, false, true, false, "", pot, deltaPot);
        string serializer = JsonUtility.ToJson(msg);
        connectionManager.Send(serializer);
        StartCoroutine(ShowFoldMsg());
    }

    public void Call()
    {
        if ( money < toCall )
        {
            myPot += money;
            deltaPot += money;
            money -= money;
           
            PTPHeader msg = new PTPHeader(-13, true, false, false, false, "", pot, deltaPot);
            string serializer = JsonUtility.ToJson(msg);
            connectionManager.Send(serializer);
        }
        else
        {
            myPot += toCall;
            deltaPot += toCall;
            money -= toCall;

            PTPHeader msg = new PTPHeader(0, true, false, false, false, "", pot, deltaPot);
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
                //card.transform.parent = playerCardsPoints[i].transform;
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
            pot += deltaPot;
            //UpdateMoney();
        }  
    }

    public void Show4Cards()
    {       
        cards[5].SetActive(true);
        pot += deltaPot;
        //UpdateMoney();
    }

    public void Show5Cards()
    {
        cards[6].SetActive(true);
        pot += deltaPot;
        //UpdateMoney();
    }

    public void ShowEnemyCards()
    {
        cards[7].SetActive(true);
        cards[8].SetActive(true);
    }

    public IEnumerator ShowCards() //only for test
    {
        SpawnCards("2C;TD;4C;KH;7C;8H;AC;3C;5C");
        Show2Cards();
        Debug.Log("elo");
        yield return new WaitForSeconds(1);
        Debug.Log("elo");
        Show3Cards();
        yield return new WaitForSeconds(1);
        Debug.Log("elo");
        Show4Cards();
        yield return new WaitForSeconds(1);
        Debug.Log("elo");
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
        foldButton.SetActive(connectionManager.ptpHeader.fold);

        colors = raiseButton.GetComponent<Button>().colors;
        colors.normalColor = connectionManager.ptpHeader.raise ? Color.white : Color.gray;
        raiseButton.GetComponent<BoxCollider>().enabled = connectionManager.ptpHeader.raise;
        raiseButton.SetActive(connectionManager.ptpHeader.raise);

        colors = checkButton.GetComponent<Button>().colors;
        colors.normalColor = connectionManager.ptpHeader.check ? Color.white : Color.gray;
        checkButton.GetComponent<BoxCollider>().enabled = connectionManager.ptpHeader.check;
        checkButton.SetActive(connectionManager.ptpHeader.check);

        colors = callButton.GetComponent<Button>().colors;
        colors.normalColor = connectionManager.ptpHeader.call ? Color.white : Color.gray;
        callButton.GetComponent<BoxCollider>().enabled = connectionManager.ptpHeader.call;
        callButton.SetActive(connectionManager.ptpHeader.call);
    }

    private void UpdateMoney()
    {       
        moneyText.text = money.ToString();
        enemyMoneyText.text = enemyMoney.ToString();
        potText.text = pot.ToString();
        myPotText.text = myPot.ToString();
        enemyPotText.text = enemyPot.ToString();

    }

    private void CalcToCall(int deltaPot)
    {
        enemyPot = deltaPot - myPot;
        toCall = enemyPot - myPot;
    }

    private void EndRound()
    {
        pot += deltaPot;
        //UpdateMoney();
        StartCoroutine(WaitForScore());
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

    private IEnumerator WaitForScore()
    {
        yield return new WaitForSeconds(2);
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
