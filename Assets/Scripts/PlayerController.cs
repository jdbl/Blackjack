using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Control for player.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /*
       This class controls all player actions and monitors players cards.

        Public Methods:
            GetHand()
            GetHandValues()
            PlaceBet()
            GetBets()
            AddToHand(Card newCard)
            newCard: Card dealt to player to add to hand.
            Split()
            GetHandCount()
            ResetHand()
            NextSplitHand()
            GetHandFinished()
            GetCredit()
            SetCredit(int _credit)
                _credit: Users credit in system

        Private Methods:
            DisplayCard(Card newCard)
                newCard: Card to instantiate on table
            DisplayCard(Card newCard, bool second)
                newCard: Card to instantiate on table
                second: bool determining if new card should be face down
   */


    [SerializeField]
    private Slider BetSlider;
    [SerializeField]
    private List<float> bets = new List<float>();
    [SerializeField]
    private List<List<Card>> hand = new List<List<Card>>();
    [SerializeField]
    private Text creditText;

    private int cardIndex = 0;
    private int handIndex = 0;
    private int handCount = 0;
    private List<int> handValues = new List<int>();
    private bool[] handFinished = { false, false, false, false };
    private bool finishTurn = false;
    private float credit = 0.0f;
    private List<List<GameObject>> handPrefabs = new List<List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        hand.Add(new List<Card>());

    }

    /// <summary>
    /// Returns current player hands.
    /// </summary>
    /// <returns>List(List(Card))</returns>
    public List<List<Card>> GetHand()
    {
        return hand;
    }

    /// <summary>
    /// Returns current value of all player hands.
    /// </summary>
    /// <returns>List(int)</returns>
    public List<int> GetHandValues()
    {
        return handValues;
    }

    /// <summary>
    /// Sets players bet from slider value.
    /// </summary>
    public void PlaceBet()
    {
        bets.Add((float) System.Math.Round((double)BetSlider.value, 2));
        credit -= (int)BetSlider.value;
        creditText.text = "Credit: " + credit.ToString();
        BetSlider.maxValue = credit;
    }

    /// <summary>
    /// Returns players bets for each hand.
    /// </summary>
    /// <returns>List(int)</returns>
    public List<float> GetBets()
    {
        return bets;
    }

    /// <summary>
    /// Add Card to dealer hand.
    /// </summary>
    /// <param name="newCard">New card to be added to the hand.</param>
    public void AddToHand(Card newCard)
    {//Add new card dealt to players hand
        if(hand.Count == 0)
        {
            hand.Add(new List<Card>());
            handPrefabs.Add(new List<GameObject>());
        }
        

              

        if((handValues[handIndex] + newCard.GetFaceValue() > 21) && (newCard.GetFaceValue() == 11))
        {//Add new value to hand
         //If new value is an ace that puts it into bust it sets it to 1 and adds
            newCard.ChangeAceFaceValue();
            
            handValues[handIndex] += 1;
        }
        else if (handValues[handIndex] + newCard.GetFaceValue() > 21)
        {
            for(int index = 0; index < hand[handIndex].Count; index++)
            {
                if(hand[handIndex][index].GetFaceValue() == 11)
                {
                    hand[handIndex][index].ChangeAceFaceValue();
                    handValues[handIndex] -= 10;
                    if(handValues[handIndex] + newCard.GetFaceValue() <= 21)
                    {
                        index = hand[handIndex].Count;
                    }
                }
            }
        }
        handValues[handIndex] += newCard.GetFaceValue();
        hand[handIndex].Add(newCard);
        DisplayCard(newCard);

        if (handValues[handIndex] >= 21)
        {
            handFinished[handIndex] = true;
            NextSplitHand();
        }

        cardIndex++;

    }

    /// <summary>
    /// Instantiate card and render gameobject.
    /// </summary>
    /// <param name="newCard">New card to be rendered.</param>
    private void DisplayCard(Card newCard)
    {
        /*if (handPrefabs.Add(new List<GameObject>())) 
        
        GameObject temp = Instantiate(hand[handIndex][hand[handIndex].Count - 1].GetPrefab(), this.transform);
        temp.transform.SetPositionAndRotation(new Vector3((float)hand[handIndex].Count, -3.0f, -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
        temp.transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
         */
        handPrefabs[handIndex].Add(Instantiate(newCard.GetPrefab(), this.transform));
        handPrefabs[handIndex][handPrefabs[handIndex].Count-1].transform.SetPositionAndRotation(new Vector3((float)hand[handIndex].Count, -3.0f, -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
        handPrefabs[handIndex][handPrefabs[handIndex].Count-1].transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
        
    }

    /// <summary>
    /// Splits 2 card hand into 2 seperate hands. Max 4
    /// </summary>
    public void Split()
    {
        hand.Add(new List<Card>());
        hand[handIndex+1].Add(hand[handIndex][cardIndex]);
        handValues[handIndex + 1] = hand[handIndex][1].GetFaceValue();
        hand[handIndex].RemoveAt(1);
        
        handCount++;
    }

    /// <summary>
    /// Retuns players total number of hands.
    /// </summary>
    /// <returns>int</returns>
    public int GetHandCount()
    {
        return handCount;
    }

    /// <summary>
    /// Reset hand to empty default.
    /// </summary>
    public void ResetHand()
    {
        try
        {
            foreach(List<GameObject> tempList in handPrefabs)
            {
                foreach(GameObject temp in tempList)
                {
                    Destroy(temp);
                }
            }

            /*foreach (List<Card> tempHand in hand)
            {
                foreach (Card temp in tempHand)
                {
                    Destroy(temp.GetPrefab());
                }
            }*/
        }
        catch
        { }
        hand.Clear();
        handFinished = new bool[] { false, false, false, false };
        cardIndex = 0;
        handIndex = 0;
        handCount = 0;
        handValues.Clear();
        handValues.Add(0);
        bets.Clear();
        
        
    }

    /// <summary>
    /// Changes to next player hand if any.
    /// </summary>
    public void NextSplitHand()
    {
        handFinished[handIndex] = true;
        handIndex++;
        cardIndex = 0; 
    }

    /// <summary>
    /// Return array for players completd hands.
    /// </summary>
    /// <returns>bool[]</returns>
    public bool[] GetHandFinished()
    {
        return handFinished;
    }

    /// <summary>
    /// Get players machine credit
    /// </summary>
    /// <returns>int</returns>
    public float GetCredit()
    {
        return credit;
    }

    /// <summary>
    /// Assigns player credit to reflect changes.
    /// </summary>
    /// <param name="_credit"></param>
    public void SetCredit(float _credit)
    {
        this.credit = _credit;
    }
}
