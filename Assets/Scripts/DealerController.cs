using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DealerController : MonoBehaviour
{
    public bool HasCards = false;

    [SerializeField]
    private Slider BetSlider;
    [SerializeField]
    private int Bet = 0;
    [SerializeField]
    private List<Card> hand = new List<Card>();


    private int cardIndex = 0;
    private int handIndex = 0;
    private int handCount = 0;
    private int handValue = 0;
    private bool handFinished = false;
    private List<GameObject> handPrefabs = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public List<Card> GetHand()
    {
        return hand;
    }
    public int GetHandValue()
    {
        return handValue;
    }

    public void PlaceBet()
    {
        Bet = (int)BetSlider.value;

    }
    public int GetBet()
    {
        return Bet;
    }

    public void AddToHand(Card newCard)
    {//Add new card dealt to players hand

        if (hand.Count == 0)
        {//if hand is currently empty
            if (newCard.GetFaceValue() == 1)
            {//if new card is an ace set value to 11 and add
                newCard.ChangeAceFaceValue();
                hand.Add(newCard);
                DisplayCard(newCard);
            }
            else
            {
                hand.Add(newCard);
                DisplayCard(newCard);
            }
        }
        else
        {
            if (hand[0].GetFaceValue() != 11 && newCard.GetFaceValue() == 1)
            {//if first card in hand is not an 11 value ace.
                newCard.ChangeAceFaceValue();
                hand.Add(newCard);
                if(hand.Count == 1)
                {
                    DisplayCard(newCard, true);
                }
                else
                {
                    DisplayCard(newCard);
                }
                
            }
            else
            {
                hand.Add(newCard);
                if (hand.Count == 2)
                {
                    DisplayCard(newCard, true);
                }
                else
                {
                    DisplayCard(newCard);
                }
            }

        }


        if ((handValue + newCard.GetFaceValue() > 21) && (newCard.GetFaceValue() == 11))
        {//Add new value to hand
         //If new value is an ace that puts it into bust it sets it to 1 and adds
            hand[0].ChangeAceFaceValue();
            handValue += 1;
        }
        else
        {
            handValue += newCard.GetFaceValue();
        }

        if (handValue >= 16)
        {
            handFinished = true;
        }

        cardIndex++;

    }

    private void DisplayCard(Card newCard)
    {
        handPrefabs.Add(Instantiate(newCard.GetPrefab(), this.transform));
        handPrefabs[handPrefabs.Count-1].transform.SetPositionAndRotation(new Vector3((float)hand.Count, 3.0f, -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
        handPrefabs[handPrefabs.Count - 1].transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
    }

    private void DisplayCard(Card newCard, bool second)
    {
        handPrefabs.Add(Instantiate(newCard.GetPrefab(), this.transform));
        handPrefabs[1].transform.SetPositionAndRotation(new Vector3((float)hand.Count, 3.0f, -1.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        handPrefabs[1].transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
    }

    public void FlipCard()
    {
        handPrefabs[1].transform.SetPositionAndRotation(new Vector3((float)hand.Count, 3.0f, -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
    }

    public void ResetHand()
    {
        hand.Clear();
        handFinished = true;
        cardIndex = 0;
        handIndex = 0;
        handCount = 0;
        handValue = 0;
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
    public bool GetHandFinished()
    {
        return handFinished;
    }
}
