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
    // Start is called before the first frame update
    void Start()
    {
        hand.Add(new List<Card>());
    }

    // Update is called once per frame
    void Update()
    {
        
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
