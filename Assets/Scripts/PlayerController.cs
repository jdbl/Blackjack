using System;
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
			Splitable()
			GetHandPrefabs(int index)
				index: Index of hand to return

        Private Methods:
            DisplayCard(Card newCard)
                newCard: Card to instantiate on table

   */


	[SerializeField]
    private Text betText = null;
    [SerializeField]
    private List<int> bets = new List<int>();
    [SerializeField]
    private List<List<Card>> hand = new List<List<Card>>();
    [SerializeField]
    private Text creditText = null;
	[SerializeField]
	private GameObject arrow = null;

    private int cardIndex = 0;
    private int handIndex = 0;
    private int handCount = 0;
    private List<int> handValues = new List<int>();
    private bool[] handFinished = { false, false, false, false };
    private int credit;
    private List<List<GameObject>> handPrefabs = new List<List<GameObject>>();

	// Start is called before the first frame update
	void Start()
	{
		hand.Add(new List<Card>());
	}

	public int Credit
	{
		get { return credit; }
		set { credit = value; }
	}

	/// <summary>
	/// Returns all player hands.
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
	/// 
	public void PlaceBet()
	{
		int bet = Convert.ToInt32(betText.text);
		bets.Add(bet);
		credit -= bet;
		creditText.text = "Credit: " + credit.ToString() + System.Environment.NewLine + "Last Bet: " + betText.text;
	}

	/// <summary>
	/// Double players current bet.
	/// </summary>
	/// <param name="index">Index of where to double bet.</param>
	public void DoubleBet(int index)
	{
		credit -= bets[index];
		bets[index] *= 2;
		creditText.text = "Credit: " + credit.ToString() + System.Environment.NewLine + "Last Bet: " + bets[index];
	}
	/// <summary>
	/// Returns players bets for each hand.
	/// </summary>
	/// <returns>List(int)</returns>
	public List<int> GetBets()
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
		handPrefabs[handIndex].Add(Instantiate(newCard.GetPrefab(), this.transform));
		handPrefabs[handIndex][handPrefabs[handIndex].Count - 1].transform.SetPositionAndRotation(new Vector3((float)((hand[handIndex].Count - 1) * 0.5f),
			-2.25f + ((handIndex) * 1.35f), -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
		handPrefabs[handIndex][handPrefabs[handIndex].Count - 1].transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
		float temp = (hand[handIndex].Count - 1) * (0.1f);
		handPrefabs[handIndex][handPrefabs[handIndex].Count - 1].transform.Translate(new Vector3(0.0f, 0.0f, ((hand[handIndex].Count - 1) * 0.1f)));
	}
	/// <summary>
	/// Splits 2 card hand into 2 seperate hands. Max 4
	/// </summary>
	public void Split(Card newCard1)
	{
		hand.Add(new List<Card>());
		hand[handCount + 1].Add(hand[handIndex][1]);
		if(hand[handCount + 1][0].GetFaceValue() == 1)
		{
			hand[handCount + 1][0].ChangeAceFaceValue();
		}
		hand[handIndex].RemoveAt(1);
		hand[handIndex].Add(newCard1);


		handValues[handIndex] = hand[handIndex][0].GetFaceValue() + hand[handIndex][1].GetFaceValue();
		handValues.Add(hand[handCount + 1][0].GetFaceValue());

		handPrefabs.Add(new List<GameObject>());
		handPrefabs[handCount + 1].Add(handPrefabs[handIndex][1]);
		handPrefabs[handIndex].RemoveAt(1);
		handPrefabs[handIndex].Add(Instantiate(newCard1.GetPrefab(), this.transform));


		handPrefabs[handIndex][1].transform.SetPositionAndRotation(new Vector3((float)((hand[handIndex].Count - 1) * 0.5f),
			-2.25f + ((handIndex) * 1.35f), -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
		handPrefabs[handIndex][1].transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
		handPrefabs[handIndex][1].transform.Translate(new Vector3(0.0f, 0.0f, ((hand[handCount + 1].Count - 1) * 0.1f)));

		handPrefabs[handCount + 1][0].transform.SetPositionAndRotation(new Vector3(0.0f,
			-2.25f + ((handCount + 1) * 1.35f), -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
		handPrefabs[handCount + 1][0].transform.Translate(new Vector3(0.0f, 0.0f, 0.1f));

		PlaceBet();
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
		}
		catch
		{ }
		hand.Clear();
		handFinished = new bool[] { false, false, false, false };
		cardIndex = 0;
		handIndex = 0;
		handCount = 0;
		handPrefabs.Clear();
		handValues.Clear();
		handValues.Add(0);
		bets.Clear();
		arrow.transform.position = new Vector3(-2.0f, -2.25f, 0.0f);
        
	}

	/// <summary>
	/// Changes to next player hand if any.
	/// </summary>
	public void NextSplitHand()
	{
		try
		{
			if(handIndex < 4)
			{
				handFinished[handIndex] = true;
			}
			
			cardIndex = 0;
			if(handCount > handIndex )
			{
				handIndex++;
				arrow.transform.Translate(new Vector3(-1.35f, 0.0f, 0.0f));
			}
		}
		catch(System.IndexOutOfRangeException e)
		{
			Debug.Log(e.Message);
			throw e;
		}
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
	/// Determines if current hand can be split.
	/// </summary>
	/// <returns></returns>
	public bool Splitable()
	{
       	if (hand.Count > handIndex && hand.Count < 4 && hand[handIndex].Count == 2 && (hand[handIndex][0].GetFaceValue() == hand[handIndex][1].GetFaceValue() 
			|| hand[handIndex][1].GetFaceValue() == 1))
		{
			return true;
		}
		else
		{
			return false;
		}
   
	}

	/// <summary>
	/// Returns prefabs of all cards in hand.
	/// </summary>
	/// <param name="index"></param>
	/// <returns>List(GameObject)</returns>
	public List<GameObject> GetHandPrefabs(int index)
	{
		return handPrefabs[index];
	}
	
	/// <summary>
	/// Returns the current player hand number
	/// </summary>
	/// <returns>int</returns>
	public int GetHandIndex()
	{
		return handIndex;
	}
}
