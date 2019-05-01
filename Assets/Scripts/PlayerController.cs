using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public bool HasCards = false;

    [SerializeField]
    private Slider BetSlider;
    [SerializeField]
    private int Bet = 0;
    [SerializeField]
    private List<List<Card>> hand = new List<List<Card>>();


    private int cardIndex = 0;
    private int handIndex = 0;
    private int handCount = 0;
    private List<int> handValues = new List<int>();
    private List<bool> bust = new List<bool>();
    // Start is called before the first frame update
    void Start()
    {
        hand.Add(new List<Card>());
        bust.Add(false);
    }

    public List<List<Card>> GetHand()
    {
        return hand;
    }
    public List<int> GetHandValues()
    {
        return handValues;
    }

    public void PlaceBet()
    {
        Bet = (int) BetSlider.value;
        
    }
    public int GetBet()
    {
        return Bet;
    }

    public void AddToHand(Card newCard)
    {//Add new card dealt to players hand
        if(hand.Count == 0)
        {
            hand.Add(new List<Card>());
        }
        if(hand[handIndex].Count == 0)
        {//if hand is currently empty
            if(newCard.GetFaceValue() == 1)
            {//if new card is an ace set value to 11 and add
                newCard.ChangeAceFaceValue();
                hand[handIndex].Add(newCard);
                DisplayCard(newCard);
            }
            else
            {
                hand[handIndex].Add(newCard);
                DisplayCard(newCard);
            }
        }
        else
        {
            if (hand[handIndex][0].GetFaceValue() != 11 && newCard.GetFaceValue() == 1)
            {//if first card in hand is not an 11 value ace.
                newCard.ChangeAceFaceValue();
                hand[handIndex].Insert(0, newCard);
                DisplayCard(newCard);
            }
            else 
            {
                hand[handIndex].Add(newCard);
                DisplayCard(newCard);
            }
            
        }
       

        if((handValues[handIndex] + newCard.GetFaceValue() > 21) && (newCard.GetFaceValue() == 11))
        {//Add new value to hand
         //If new value is an ace that puts it into bust it sets it to 1 and adds
            hand[handIndex][0].ChangeAceFaceValue();
            handValues[handIndex] += 1;
        }
        else
        {
            handValues[handIndex] += newCard.GetFaceValue();
        }

        if(handValues[handIndex] > 21)
        {
            bust[handIndex] = true;
        }
        cardIndex++;

    }

    private void DisplayCard(Card newCard)
    {
        GameObject temp = Instantiate(newCard.GetPrefab(), this.transform);
        temp.transform.localScale = new Vector3(38.0f, 38.0f, 1.0f);
        temp.transform.localRotation = new Quaternion(180.0f, 0.0f, 0.0f, 0.0f);
    }

    public void Split()
    {
        hand.Add(new List<Card>());
        hand[handIndex+1].Add(hand[handIndex][cardIndex]);
        handValues[handIndex + 1] = hand[handIndex][1].GetFaceValue();
        hand[handIndex].RemoveAt(1);
        
        handCount++;
    }


    public void ResetHand()
    {
        hand.Clear();
        bust.Clear();
        bust.Add(false);
        cardIndex = 0;
        handIndex = 0;
        handCount = 0;
        handValues.Clear();
        handValues.Add(0);
    }
    public void NextSplitHand()
    {
        handIndex++;
        cardIndex = 0;
    }
    
}
