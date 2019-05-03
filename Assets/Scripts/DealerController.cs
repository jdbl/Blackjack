using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Control for dealer play.
/// </summary>
public class DealerController : MonoBehaviour
{

    /*
        This class controls all dealer actions and monitors dealers cards.

        Public Methods:
            GetHand()
            GetHandValue()
            AddToHand()
            FlipCard()
            ResetHand()
            GetHandFinished()
            CardCount()
        Private Methods:
            DisplayCard(Card newCard)
                newCard: Card to instantiate on table
            DisplayCard(Card newCard, bool second)
                newCard: Card to instantiate on table
                second: bool determining if new card should be face down
    */

    [SerializeField]
    private List<Card> hand = new List<Card>();


    private int cardCount = 0;
    private int handValue = 0;
    private bool handFinished = false;
    private List<GameObject> handPrefabs = new List<GameObject>();
    
    /// <summary>
    /// Returns current dealer hand.
    /// </summary>
    /// <returns>List(Card)</returns>
    public List<Card> GetHand()
    {
        return hand;
    }

    /// <summary>
    /// Returns current value of dealers current hand.
    /// </summary>
    /// <returns>int</returns>
    public int GetHandValue()
    {
        return handValue;
    }

    /// <summary>
    /// Add Card to dealer hand.
    /// </summary>
    /// <param name="newCard">New card to be added to the hand.</param>
    public void AddToHand(Card newCard)
    {//Add new card dealt to players hand
        
        if (handValue + newCard.GetFaceValue() > 21 && (newCard.GetFaceValue() == 11))
        {
            newCard.ChangeAceFaceValue();
        }
        else if (handValue + newCard.GetFaceValue() > 21)
        {
            for (int index = 0; index < hand.Count; index++)
            {
                if (hand[index].GetFaceValue() == 11)
                {
                    hand[index].ChangeAceFaceValue();
                    handValue -= 10;
                    if (handValue + newCard.GetFaceValue() <= 21)
                    {
                        index = hand.Count;
                    }
                }
            }
        }
        handValue += newCard.GetFaceValue();
        hand.Add(newCard);
        if(cardCount == 1)
        {
            DisplayCard(newCard, true);
        }
        else
        {
            DisplayCard(newCard);
        }
        
        cardCount++;
        if (handValue >= 16)
        {
            handFinished = true;
        }

        

    }

    /// <summary>
    /// Instantiate card and render gameobject.
    /// </summary>
    /// <param name="newCard">New card to be rendered.</param>
    private void DisplayCard(Card newCard)
    {
        handPrefabs.Add(Instantiate(newCard.GetPrefab(), this.transform));
        handPrefabs[handPrefabs.Count-1].transform.SetPositionAndRotation(new Vector3((float)hand.Count, 3.0f, -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
        handPrefabs[handPrefabs.Count - 1].transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
    }

    /// <summary>
    /// Instantiate card and render gameobject face down.
    /// </summary>
    /// <param name="newCard">New card to be rendered.</param>
    /// <param name="second">Render card face down</param>
    private void DisplayCard(Card newCard, bool second)
    {
        handPrefabs.Add(Instantiate(newCard.GetPrefab(), this.transform));
        handPrefabs[1].transform.SetPositionAndRotation(new Vector3((float)hand.Count, 3.0f, -1.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        handPrefabs[1].transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
    }

    /// <summary>
    /// Flip second dealer card facing up.
    /// </summary>
    public void FlipCard()
    {
        handPrefabs[1].transform.SetPositionAndRotation(new Vector3((float)hand.Count, 3.0f, -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
    }

    /// <summary>
    /// Reset hand to empty default.
    /// </summary>
    public void ResetHand()
    {
        hand.Clear();
        handFinished = false;
        handValue = 0;
        cardCount = 0;
        try
        {
            foreach (GameObject temp in handPrefabs)
            {
                Destroy(temp);
            }
            handPrefabs.Clear();
        }
        catch { }
    }

    /// <summary>
    /// Return dealers current hand status.
    /// </summary>
    /// <returns>bool</returns>
    public bool GetHandFinished()
    {
        return handFinished;
    }
    public int CardCount()
    {
        return cardCount;
    }
}
