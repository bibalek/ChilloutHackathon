using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform[] playerCardsPoints;
    [SerializeField]
    Transform cardsPoint;
    [SerializeField]
    private RawConnectionManager connectionManager;
    [SerializeField]
    GameObject foldMsg;

    private bool handshake = false;
    private bool start = false;
    private List<GameObject> cards = new List<GameObject>();
  
    private void Start ()
    {
          
    }

    private void Update()
    {
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
                handshake = true;
            }
            if (connectionManager.update)
            {
                connectionManager.update = false;
                if (connectionManager.ptpHeader.integer < 0)
                {
                    if (connectionManager.ptpHeader.integer == -3) Show3Cards();
                    if (connectionManager.ptpHeader.integer == -4) Show4Cards();
                    if (connectionManager.ptpHeader.integer == -5) Show5Cards();
                    if (connectionManager.ptpHeader.integer == -10) NewRound();
                }
            }
        }   
    }

  

    public void Fold()
    {
        PTPHeader msg = new PTPHeader(0, false, false, false, true, "");
        string serializer = JsonUtility.ToJson(msg);
        connectionManager.Send(serializer);
        StartCoroutine(ShowFoldMsg());
    }

   

    public void Raise()
    {
      
    }
    public void Check()
    {
        
    }

    public void Call()
    {
        PTPHeader msg = new PTPHeader(0,true,false,false,false, "");
        string serializer = JsonUtility.ToJson(msg);
        connectionManager.Send(serializer);
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

    private IEnumerator ShowCards()
    {
        SpawnCards("2C;TD;4C;KH;7C;8H;AC");
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
        throw new NotImplementedException();
    }

}
