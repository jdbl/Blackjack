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
    // Start is called before the first frame update
    void Start()
    {
        hand.Add(new List<Card>());
    }

    public List<List<Card>> GetHand()
    {
        return hand;
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
    {

        hand[handIndex][cardIndex] = newCard;
        if(newCard.faceValue != 1)
        {
            handValues[handIndex] += hand[handIndex][cardIndex].faceValue;
        }
        else if((handValues[handIndex] + 11 > 21 && (hand[handIndex].Contains(new Card(1, Card.SPADE)) || 
            hand[handIndex].Contains(new Card(1, Card.CLUB)) ||
            hand[handIndex].Contains(new Card(1, Card.HEART)) ||
            hand[handIndex].Contains(new Card(1, Card.DIAMOND)))))
        {
            
        }
        else if(handValues[handIndex] + 11 > 21)
        {
            handValues[handIndex] += 1;
        }
        cardIndex++;

    }
    public void Split()
    {
        hand.Add(new List<Card>());
        hand[handIndex+1][0] = hand[handIndex][cardIndex];
        hand[handIndex].RemoveAt(1);
        handCount++;
    }
    public void ResetHand()
    {
        hand.Clear();
        cardIndex = 0;
        handIndex = 0;
        handCount = 0;
    }
    public void NextSplitHand()
    {
        handIndex++;
        cardIndex = 0;
    }
}
