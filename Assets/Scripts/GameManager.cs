using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Controls main game functionality. 
/// </summary>
/// 

    
public class GameManager : MonoBehaviour
{
	/*
        This class provides services related to all aspects of the game. It controls the shuffle and dealing.
        It also controls player turns and available actions on the UI

        Public Methods:
			UpdateBetText()
			DealGame()
			Deal()
			UpdateCardValues()
			ChangePlayerTurn()
			Stand()
			Hit()
			Split()
			InsuranceNo()
			InsuranceYes()
			Replay()
			NextHand()
			Double()

        Private Methods:
            PlayDealer()
            ResetGame()
            CalculateWinnings()

        Public Properties:
            deckNumbers: (Read only, static, int)
        
  

   Public Properties:
      MailSubject:         (Write only, String)
      MailMessage:         (Write only, String)
      MailAttachments:   (Write only, String)

   Usage:   All game controlling methods and play tracking is done here. 
           Starting the game, changing the players, turn, resetting and ending the game. 

    */

	[SerializeField]
	private Slider betSlider = null;
	[SerializeField]
	private Text betText = null;
	[SerializeField]
	private PlayerController player = null;
	[SerializeField]
	private DealerController dealer = null;
	[SerializeField]
	private Canvas mainCanvas;
	[SerializeField]
	private DeckOfCards deck = null;
	[SerializeField]
	private Text playerHandText = null;
	[SerializeField]
	private Text dealerHandText = null;
	[SerializeField]
	private Button betButton = null;
	[SerializeField]
	private Button standButton = null;
	[SerializeField]
	private Button splitButton = null;
	[SerializeField]
	private Button hitButton = null;
	[SerializeField]
	private Text creditText = null;
	[SerializeField]
	private Text insuranceText = null;
	[SerializeField]
	private Button insuranceYesButton = null;
	[SerializeField]
	private Button insuranceNoButton = null;
	[SerializeField]
	private Image gameOverImage = null;
	[SerializeField]
	private Button doubleButton = null;

	private List<Text> handResultsText;
	public static int deckNumbers = 1;
	private bool playerTurn = true;
	private bool insuranceBlackjack = false;
	private const int STARTING_CREDIT = 100;
	private int lastBet = 0;

	// Start is called before the first frame update
	void Start()
	{
		deck.BuildDeck();
		deck.ShuffleDeck();
		player.SetCredit(STARTING_CREDIT);
		player.ResetHand();
		handResultsText = new List<Text>();
		creditText.text = "Credit: " + player.GetCredit().ToString();
		standButton.interactable = false;
		splitButton.interactable = false;
		hitButton.interactable = false;
		doubleButton.interactable = false;
		gameOverImage.gameObject.SetActive(false);
	}

  

	/// <summary>
	/// Update user bet on display
	/// </summary>
	public void UpdateBetText()
	{
		betText.text = "Bet: " + System.Math.Round(betSlider.value, 2).ToString();
	}

	/// <summary>
	/// First deal of the game.
	/// </summary>
	public void DealGame()
	{

		ResetGame();
		betSlider.interactable = false;
		betButton.interactable = false;
		lastBet = (int)betSlider.value;
		player.PlaceBet();
		standButton.interactable = true;
		hitButton.interactable = true;
		if(player.GetBets()[0] <= player.GetCredit())
		{
			doubleButton.interactable = true;
		}
		

		deck.BuildDeck();
		deck.ShuffleDeck();
		dealerHandText.text = "DEALER: ";
		playerHandText.text = "PLAYER: ";
		insuranceText.text = "Do you want insurance?";
		insuranceBlackjack = false;

		for (int index = 0; index < handResultsText.Count; index++)
		{
			Destroy(handResultsText[index].gameObject);
		}
		handResultsText.Clear();
		for (int index = 0; index <= 3; index++)
		{
			Card newCard = deck.Deal();
			int temp = index % 2;
			if(index % 2 == 0)
			{
				player.AddToHand(newCard);
			}
			else
			{
				dealer.AddToHand(newCard);
			}
		}
		if(dealer.GetHand()[0].GetFaceValue() == 11)
		{
			if(player.GetCredit() >= (lastBet/2))
			{
				insuranceText.transform.gameObject.SetActive(true);
				insuranceYesButton.gameObject.SetActive(true);
				insuranceNoButton.gameObject.SetActive(true);
				doubleButton.interactable = false;
				splitButton.interactable = false;
				standButton.interactable = false;
				hitButton.interactable = false;
			}
			else
			{
				InsuranceNo();
			}
			
		}        
		else if(player.Splitable() && !insuranceBlackjack && player.GetBets()[0] <= player.GetCredit())
		{
			splitButton.interactable = true;
		}
		else
		{
			splitButton.interactable = false;
		}
		UpdateCardValues();

	}

	/// <summary>
	/// Gives player or dealer a new card and updates their hand value. 
	/// </summary>
	public void Deal()
	{
		Card newCard = deck.Deal();
		if(playerTurn)
		{
			player.AddToHand(newCard);
            
		}
		else
		{
			dealer.AddToHand(newCard);
		}

		if (player.Splitable() && player.GetBets()[0] <= player.GetCredit())
		{
			splitButton.interactable = true;
		}
		else
		{
			splitButton.interactable = false;
		}
		UpdateCardValues();
	}

	/// <summary>
	/// Updates players hand values and displays special hand results
	/// </summary>
	public void UpdateCardValues()
	{
		playerHandText.text = "Player: ";
		int value = 0;
		if (playerTurn)
		{
			for (int index = player.GetHandValues().Count - 1; index >= 0; index--)
			{
				value = player.GetHandValues()[index];
				if (value > 21)
				{
					playerHandText.text += System.Environment.NewLine +
					value.ToString() + " BUST";

				}
				else if (value == 21 && player.GetHand()[index].Count == 2)
				{
					playerHandText.text += System.Environment.NewLine +
					value.ToString() + " BLACKJACK";
					Stand();
				}
				else
				{
					playerHandText.text += System.Environment.NewLine +
					value.ToString();
				}

			}
		}
		else
		{
			for (int index = player.GetHandValues().Count - 1; index >= 0; index--)
			{
				value = player.GetHandValues()[index];
				if (value > 21)
				{
					playerHandText.text += System.Environment.NewLine + value.ToString() + " BUST";

				}
				else if (value == 21 && player.GetHand()[index].Count == 2)
				{
					playerHandText.text += System.Environment.NewLine + value.ToString() + " BLACKJACK";
				}
				else if (value == dealer.GetHandValue())
				{
					playerHandText.text += System.Environment.NewLine + value.ToString() + " PUSH";
				}
				else if (value > dealer.GetHandValue() || dealer.GetHandValue() > 21)
				{
					playerHandText.text += System.Environment.NewLine + value.ToString() + " WIN";
				}
				else
				{
					playerHandText.text += System.Environment.NewLine + value.ToString() + " LOSE";
				}
			}
		}

	}

	/// <summary>
	/// Switch between dealer and player turns.
	/// </summary>
	public void ChangePlayerTurn()
	{
		if(playerTurn)
		{
			playerTurn = false;
			PlayDealer();
			playerTurn = true;
		}
		
	}

	/// <summary>
	/// Plays dealer hand a finish condition is met.
	/// </summary>
	private void PlayDealer()
	{
		if(dealer.CardCount() == 2)
		{
			dealer.FlipCard();
		}

		bool allBlackJack = true;
		for(int index = 0; index < player.GetHand().Count; index++)
		{
			if (player.GetHandValues()[index] != 21)
			{
				allBlackJack = false;
			}
			if(player.GetHand()[index].Count != 2)
			{
				allBlackJack = false;
			}

		}
		if(allBlackJack)
		{
			dealer.FinishHand();
		}


		int bustCount = 0;
		for(int index = 0; index < player.GetHand().Count; index++)
		{
			if(player.GetHandValues()[index] > 21)
			{
				bustCount++;
			}
		}
		if(bustCount == player.GetHand().Count)
		{
			dealer.FinishHand();
		}


		while (!dealer.GetHandFinished())
		{
			Deal();
		}
        
		int value = dealer.GetHandValue();
		dealerHandText.text = "DEALER: ";
		if (value > 21)
		{
			dealerHandText.text += System.Environment.NewLine +
			value.ToString() + " BUST";

		}
		else if (value == 21 && dealer.GetHand().Count == 2)
		{
			dealerHandText.text += System.Environment.NewLine +
			value.ToString() + " BLACKJACK";
		}
		else 
		{
			dealerHandText.text += System.Environment.NewLine +
			value.ToString();
		}
		UpdateCardValues();
		CalculateWinnings();
		betSlider.maxValue = player.GetCredit();
		betButton.interactable = true;
		betSlider.interactable = true;
		splitButton.interactable = false;
		doubleButton.interactable = false;
		standButton.interactable = false;
	}

	/// <summary>
	/// Resets the game to default state.
	/// </summary>
	private void ResetGame()
	{
		betSlider.interactable = true;
		betButton.interactable = true;
		player.ResetHand();
		dealer.ResetHand();
        
	}


	/// <summary>
	/// Calculate player winnings for each hand. 
	/// </summary>
	private void CalculateWinnings()
	{
		//int index = player.GetHandIndex();

		for(int index = 0; index < player.GetHandValues().Count; index++ )
		{
		if (player.GetHandValues()[index] == dealer.GetHandValue() )
		{//PUSH
			player.SetCredit(player.GetCredit() + player.GetBets()[index]);
		}
		else if (player.GetHandValues()[index] == 21 && player.GetHand()[index].Count == 2)
		{//BLACKJACK
			player.SetCredit((int)(player.GetCredit() + player.GetBets()[index] * 2.5f));

		}
		else if(player.GetHandValues()[index] > dealer.GetHandValue() && player.GetHandValues()[index] <=21)
		{//WIN
			player.SetCredit(player.GetCredit() + (player.GetBets()[index]) * 2);

		}
		else if(player.GetHandValues()[index] <= 21 && dealer.GetHandValue() > 21)
		{//WIN
			player.SetCredit(player.GetCredit() + (player.GetBets()[index]) * 2);

		}

            
		}
		creditText.text = "Credit: " + player.GetCredit().ToString() + System.Environment.NewLine + "Last Bet: " + lastBet.ToString();

		if (player.GetCredit() < 5)
		{
			gameOverImage.gameObject.SetActive(true);
			betButton.interactable = false;
			betSlider.interactable = false;
		}
	}

	/// <summary>
	/// Ends turn on hand and changes turn if last hand
	/// </summary>
	public void Stand()
	{
		NextHand();
		doubleButton.interactable = false;
		if (player.GetHandFinished()[player.GetHandCount()])
		{
			standButton.interactable = false;
			splitButton.interactable = false;
			hitButton.interactable = false;
			ChangePlayerTurn();
			if (playerTurn && !dealer.GetHandFinished())
			{
				
			}
		}
		else
		{
			if (player.Splitable() && player.GetBets()[0] <= player.GetCredit())
			{
				splitButton.interactable = true;
			}
			else
			{
				splitButton.interactable = false;
			}
		}
		
	}

	/// <summary>
	/// Deals another hand to player and changes turn if 21 or bust
	/// </summary>
	public void Hit()
	{
		doubleButton.interactable = false;
		Deal();
		if (player.GetHandFinished()[player.GetHandCount()])
		{
			standButton.interactable = false;
			splitButton.interactable = false;
			hitButton.interactable = false;

			if (playerTurn)
			{
				ChangePlayerTurn();
			}
		}
		else if(player.GetHand()[player.GetHandIndex()].Count == 1)
		{
			Deal();
		}
	}

	/// <summary>
	/// Splits hand in 2 and creates a second hand
	/// </summary>
	public void Split()
	{
		player.Split(deck.Deal());
		if (player.Splitable() && player.GetBets()[0] <= player.GetCredit())
		{
			splitButton.interactable = true;
		}
		else
		{
			splitButton.interactable = false;
		}
		doubleButton.interactable = false;
		UpdateCardValues();
	}

	/// <summary>
	/// If player turns down insurance
	/// </summary>
	public void InsuranceNo()
	{
		insuranceNoButton.gameObject.SetActive(false);
		insuranceYesButton.gameObject.SetActive(false);
		insuranceText.gameObject.SetActive(false);
		

		if (dealer.GetHandValue() == 21)
		{
			insuranceBlackjack = true;
			Stand();
		}
		else
		{
			doubleButton.interactable = true;
			standButton.interactable = true;
			hitButton.interactable = true;
			if (player.Splitable() && player.GetBets()[0] <= player.GetCredit())
			{
				splitButton.interactable = true;
			}
			else
			{
				splitButton.interactable = false;
			}
		}
	}

	/// <summary>
	/// If player accepts insurance
	/// </summary>
	public void InsuranceYes()
	{
		insuranceNoButton.gameObject.SetActive(false);
		insuranceYesButton.gameObject.SetActive(false);
		insuranceText.gameObject.SetActive(false);


		if (dealer.GetHandValue() == 21)
		{
			player.SetCredit(player.GetCredit() + player.GetBets()[0]);
			insuranceBlackjack = true;
			Stand();
		}
		else
		{
			doubleButton.interactable = true;
			standButton.interactable = true;
			hitButton.interactable = true;
			player.SetCredit(player.GetCredit() - (int)(player.GetBets()[0] / 2));
			if (player.Splitable() && player.GetBets()[0] <= player.GetCredit())
			{
				splitButton.interactable = true;
			}
			else
			{
				splitButton.interactable = false;
			}
		}

		creditText.text = "Credit: " + player.GetCredit().ToString() + System.Environment.NewLine + "Last Bet: " + lastBet.ToString();
	}

	/// <summary>
	/// Starts game over with default credit amount
	/// </summary>
	public void Replay()
	{
		player.SetCredit(STARTING_CREDIT);
		creditText.text = "Credit: " + player.GetCredit().ToString();
		betSlider.maxValue = STARTING_CREDIT;
		ResetGame();
		gameOverImage.gameObject.SetActive(false);
	}

	/// <summary>
	///Begins playing next player hand if split
	/// </summary>
	private void NextHand()
	{
		player.NextSplitHand();

		if ((player.GetHand().Count > player.GetHandIndex()) && player.GetHand()[player.GetHandIndex()].Count == 1)
		{
			
			Deal();
		}
	}

	/// <summary>
	/// User doubles bet and only deals one card
	/// </summary>
	public void Double()
	{
		player.DoubleBet(0);
		Deal();
		Stand();
		betButton.interactable = true;
		betSlider.interactable = true;
	}
}
