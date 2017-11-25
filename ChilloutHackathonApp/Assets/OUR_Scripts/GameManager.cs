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

    private List<GameObject> cards = new List<GameObject>();
  
    private void Start ()
    {
        StartCoroutine(ShowCards());    
    }
   

    public void SpawnCards(string cardsName)
    {
        Char delimiter = ';';
        String[] cardNames = cardsName.Split(delimiter);
        for (int i = 0; i < cardNames.Length; i++)
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

}
