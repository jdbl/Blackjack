using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
			UpdateBet()
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

	//[SerializeField]
	//private Slider betSlider = null;
	[SerializeField]
	private Text betText = null;
	[SerializeField]
	private PlayerController player = null;
	[SerializeField]
	private DealerController dealer = null;
	//[SerializeField]
	//private Canvas mainCanvas;
	[SerializeField]
	private DeckOfCards deck = null;

	[SerializeField]
	private Button betButton = null;
	[SerializeField]
	private Button redoBetButton = null;
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
	[SerializeField]
	private GameObject BetArea = null;
	[SerializeField]
	private AllChips allChips = null;
	[SerializeField]
	private Text testText = null;


	
	private List<Text> handResultsText;
	public static int deckNumbers = 1;
	private bool playerTurn = true;
	private bool insuranceBlackjack = false;
	private const int STARTING_CREDIT = 10000;
	private int lastBet = 0;
	private bool canBet = false;
	private int currentBet = 0;
	private List<Chip> betChips;
	private readonly System.Random random = new System.Random();

	// Start is called before the first frame update
	void Start()
	{

		deck.BuildDeck();
		deck.ShuffleDeck();
		allChips.MakeChips();
		player.Credit = STARTING_CREDIT;
		player.ResetHand();
		handResultsText = new List<Text>();
		betChips = new List<Chip>();
		creditText.text = ConvertToDollars(player.Credit);
		canBet = true;
		standButton.gameObject.SetActive(false);
		splitButton.gameObject.SetActive(false);
		hitButton.gameObject.SetActive(false);
		doubleButton.gameObject.SetActive(false);
		gameOverImage.gameObject.SetActive(false);
		betButton.gameObject.SetActive(false);
		redoBetButton.gameObject.SetActive(false);
	}

    private void Update()
    {
		if(canBet)
		{
			ChangeBet();
		}
		
    }

	/// <summary>
	/// Add chip to betting pool 
	/// </summary>
	private void ChangeBet()
	{
		
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Ended)
			{
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
				{ 
					GameObject hitObject = hit.collider.gameObject;
					Chip hitChip = allChips.FindChipByName(hitObject.name);
					
					if (hitChip != null)
					{
						if(hitObject.transform.parent.name == "ChipBlock")
						{
							UpdateBet(hitChip.ChipValue, true, hitChip);
						}
						else if(hitObject.transform.parent.name == "BetArea")
						{
							UpdateBet(hitChip.ChipValue, false, hitObject);
						}
						if (currentBet <= 0)
						{
							betButton.gameObject.SetActive(false);
							EnableRedoBet();
						}
						else if (!betButton.gameObject.activeSelf)
						{
							betButton.gameObject.SetActive(true);
							redoBetButton.gameObject.SetActive(false);
						}
					}
				}
			}
		}
		if(Input.anyKey)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
				{
					GameObject hitObject = hit.collider.gameObject;
					Chip hitChip = allChips.FindChipByName(hitObject.name);
					if (hitChip != null)
					{
						if (hitObject.transform.parent.name == "ChipBlock")
						{
							UpdateBet(hitChip.ChipValue, true, hitChip);
						}
						else if (hitObject.transform.parent.name == "BetArea")
						{
							UpdateBet(hitChip.ChipValue, false, hitObject);
						}
						if (currentBet <= 0)
						{
							betButton.gameObject.SetActive(false);
							EnableRedoBet();
						}
						else if (!betButton.gameObject.activeSelf)
						{
							betButton.gameObject.SetActive(true);
							redoBetButton.gameObject.SetActive(false);
						}
					}
				}
			}
		}
        
	}

    /// <summary>
    /// Update user bet on display
    /// </summary>
    private void UpdateBet(int betValue, bool increase, object chip)
	{
		//betText.text = "Bet: " + System.Math.Round(betSlider.value, 2).ToString();

		if (increase)
		{
			if (currentBet + betValue <= player.Credit)
			{
				AddChipToBet((Chip)chip);
				currentBet += betValue;
			}
			
		}
		else
		{
			RemoveChipFromBet((GameObject)chip);
			currentBet -= betValue;
		}
		betText.text = ConvertToDollars(currentBet);
		creditText.text = ConvertToDollars(player.Credit - currentBet);
		
	}

	public void RedoLastBet()
	{
		foreach(Chip chip in betChips)
		{
			UpdateBet(chip.ChipValue, true, chip);
		}
		redoBetButton.gameObject.SetActive(false);
		betButton.gameObject.SetActive(true);
	}


	/// <summary>
	/// Creates a new chip 
	/// </summary>
	/// <param name="chip"></param>
	private void AddChipToBet(Chip chip)
	{
		//Instantiate(chip.Prefab, new Vector3(0.0f, -1.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), BetArea.transform);
		GameObject newChip = Instantiate(chip.Prefab);
		newChip.transform.SetParent(BetArea.transform);
        int angle = random.Next(1440);
        Renderer betAreaRenderer = BetArea.GetComponent<Renderer>();
        float randomX = Random.Range(0.0f, betAreaRenderer.bounds.extents.x) * Mathf.Cos(angle);
        float randomZ = Random.Range(0.0f, betAreaRenderer.bounds.extents.z) * Mathf.Sin(angle);
        
        newChip.transform.localPosition = new Vector3(randomX, BetArea.transform.childCount * 12, randomZ);
        newChip.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
		newChip.transform.localScale = new Vector3(.33f, 10.0f, .33f);
	}

	/// <summary>
	/// Removes tapped chip from bet area
	/// </summary>
	/// <param name="chip"></param>
	private void RemoveChipFromBet(GameObject chip)
	{
        for(int index = chip.transform.GetSiblingIndex()+1; index < BetArea.transform.childCount; index++)
        {
            BetArea.transform.GetChild(index).transform.localPosition = 
                new Vector3(BetArea.transform.GetChild(index).transform.localPosition.x,
                index * 12,
                BetArea.transform.GetChild(index).transform.localPosition.z);
        }

        Destroy(chip);
	}

	public string ConvertToDollars(int money)
	{
		string tempMoney = money.ToString();
		int index = tempMoney.Length - 3;
		while(index > 0)
		{
			tempMoney = tempMoney.Insert(index, ",");
			index -= 3;
		}
		tempMoney = "$" + tempMoney;
		return tempMoney;
	}
	/// <summary>	
	/// First deal of the game.
	/// </summary>
	public void DealGame()
	{

		ResetGame();
		canBet = false;
		//betSlider.interactable = false;
		betButton.gameObject.SetActive(false);
		redoBetButton.gameObject.SetActive(false);
		//lastBet = (int)betSlider.value;
		betChips.Clear();
		foreach(Transform chip in BetArea.transform)
		{
			betChips.Add(allChips.FindChipByName(chip.gameObject.name));
		}
		lastBet = currentBet;
		player.PlaceBet(currentBet);
		standButton.gameObject.SetActive(true);
		hitButton.gameObject.SetActive(true);
		if(player.GetBets()[0] <= player.Credit)
		{
			doubleButton.gameObject.SetActive(true);
		}
		

		deck.BuildDeck();
		deck.ShuffleDeck();
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
			if(player.Credit >= (lastBet/2))
			{
				insuranceText.transform.gameObject.SetActive(true);
				insuranceYesButton.gameObject.SetActive(true);
				insuranceNoButton.gameObject.SetActive(true);
				doubleButton.gameObject.SetActive(false);
				splitButton.gameObject.SetActive(false);
				standButton.gameObject.SetActive(false);
				hitButton.gameObject.SetActive(false);
			}
			else
			{
				InsuranceNo();
			}
			
		}        
		else if(player.Splitable() && !insuranceBlackjack && player.GetBets()[0] <= player.Credit)
		{
			splitButton.gameObject.SetActive(true);
		}
		else
		{
			splitButton.gameObject.SetActive(false);
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

		if (player.Splitable() && player.GetBets()[0] <= player.Credit)
		{
			splitButton.gameObject.SetActive(true);
		}
		else
		{
			splitButton.gameObject.SetActive(false);
		}
		UpdateCardValues();
	}

	/// <summary>
	/// Updates players hand values and displays special hand results
	/// </summary>
	public void UpdateCardValues()
	{
		int value = 0;
		if (playerTurn)
		{
			for (int index = player.GetHandValues().Count - 1; index >= 0; index--)
			{
				value = player.GetHandValues()[index];
				if (player.ScoreText[index] != null)
				{
					player.ScoreText[index].GetComponent<Text>().text = value.ToString();
				}
				dealer.ScoreText.GetComponent<Text>().text = dealer.GetHand()[0].GetFaceValue().ToString();
				if (value == 21 && player.GetHand()[index].Count == 2)
				{

					if (player.GetHandIndex() == index)
					{
						Stand();
					}

				}
				/*if (value > 21)
				{

				}
				else if (value == 21 && player.GetHand()[index].Count == 2)
				{

					if(player.GetHandIndex() == index)
					{
						Stand();
					}
					
				}
				else
				{

				}*/

			}
		}
		else
		{
			dealer.ScoreText.GetComponent<Text>().text = dealer.GetHandValue().ToString();
			/*for (int index = player.GetHandValues().Count - 1; index >= 0; index--)
			{
				value = player.GetHandValues()[index];
				if (value > 21)
				{

				}
				else if (value == 21 && player.GetHand()[index].Count == 2)
				{
				}
				else if (value == dealer.GetHandValue())
				{
				}
				else if (value > dealer.GetHandValue() || dealer.GetHandValue() > 21)
				{
				}
				else
				{
				}
			}*/

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

		if (value > 21)
		{


		}
		else if (value == 21 && dealer.GetHand().Count == 2)
		{

		}
		else 
		{

		}
		UpdateCardValues();
		CalculateWinnings();
		//betSlider.maxValue = player.Credit;
		EnableRedoBet();
		canBet = true;
		//betSlider.interactable = true;
		splitButton.gameObject.SetActive(false);
		doubleButton.gameObject.SetActive(false);
		standButton.gameObject.SetActive(false);
	}

	/// <summary>
	/// Resets the game to default state.
	/// </summary>
	private void ResetGame()
	{
		//betSlider.interactable = true;
		betButton.gameObject.SetActive(false);
		canBet = true;
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
				player.Credit = player.Credit + player.GetBets()[index];
			}
			else if (player.GetHandValues()[index] == 21 && player.GetHand()[index].Count == 2)
			{//BLACKJACK
				player.Credit = (int)(player.Credit + player.GetBets()[index] * 2.5f);

			}
			else if(player.GetHandValues()[index] > dealer.GetHandValue() && player.GetHandValues()[index] <=21)
			{//WIN
				player.Credit = player.Credit + (player.GetBets()[index]) * 2;

			}
			else if(player.GetHandValues()[index] <= 21 && dealer.GetHandValue() > 21)
			{//WIN
				player.Credit = player.Credit + (player.GetBets()[index]) * 2;

			}

            
		}
		creditText.text = ConvertToDollars(player.Credit);

		if (player.Credit <= 0)
		{
			gameOverImage.gameObject.SetActive(true);
			betButton.gameObject.SetActive(false);
		}
		foreach(Transform chip in BetArea.transform)
		{
			Destroy(chip.gameObject);
		}
		currentBet = 0;
		betText.text = ConvertToDollars(currentBet);
		EnableRedoBet();
	}

	/// <summary>
	/// Ends turn on hand and changes turn if last hand
	/// </summary>
	public void Stand()
	{
		NextHand();
		doubleButton.gameObject.SetActive(false);
		if (player.GetHandFinished()[player.GetHandCount()])
		{
			standButton.gameObject.SetActive(false);
			splitButton.gameObject.SetActive(false);
			hitButton.gameObject.SetActive(false);
			ChangePlayerTurn();
			if (playerTurn && !dealer.GetHandFinished())
			{
				
			}
		}
		else
		{
			if (player.Splitable() && player.GetBets()[0] <= player.Credit)
			{
				splitButton.gameObject.SetActive(true);
			}
			else
			{
				splitButton.gameObject.SetActive(false);
			}
		}
		
	}

	/// <summary>
	/// Deals another hand to player and changes turn if 21 or bust
	/// </summary>
	public void Hit()
	{
		doubleButton.gameObject.SetActive(false);
		Deal();
		if (player.GetHandFinished()[player.GetHandCount()])
		{
			standButton.gameObject.SetActive(false);
			splitButton.gameObject.SetActive(false);
			hitButton.gameObject.SetActive(false);

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
		if (player.Splitable() && player.GetBets()[0] <= player.Credit)
		{
			splitButton.gameObject.SetActive(true);
		}
		else
		{
			splitButton.gameObject.SetActive(false);
		}
		doubleButton.gameObject.SetActive(false);
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
			doubleButton.gameObject.SetActive(true);
			standButton.gameObject.SetActive(true);
			hitButton.gameObject.SetActive(true);
			if (player.Splitable() && player.GetBets()[0] <= player.Credit)
			{
				splitButton.gameObject.SetActive(true);
			}
			else
			{
				splitButton.gameObject.SetActive(false);
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
			player.Credit = player.Credit + player.GetBets()[0];
			insuranceBlackjack = true;
			Stand();
		}
		else
		{
			doubleButton.gameObject.SetActive(true);
			standButton.gameObject.SetActive(true);
			hitButton.gameObject.SetActive(true);
			player.Credit = player.Credit - (int)(player.GetBets()[0] / 2);
			if (player.Splitable() && player.GetBets()[0] <= player.Credit)
			{
				splitButton.gameObject.SetActive(true);
			}
			else
			{
				splitButton.gameObject.SetActive(false);
			}
		}

		creditText.text = "Credit: " + player.Credit.ToString() + System.Environment.NewLine + "Last Bet: " + lastBet.ToString();
	}

	/// <summary>
	/// Starts game over with default credit amount
	/// </summary>
	public void Replay()
	{
		player.Credit = STARTING_CREDIT;
		creditText.text = "Credit: " + player.Credit.ToString();
		//betSlider.maxValue = STARTING_CREDIT;
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
		betButton.gameObject.SetActive(false);
		EnableRedoBet();

	}

	private void EnableRedoBet()
	{
		if (player.Credit < lastBet)
		{
			redoBetButton.gameObject.SetActive(false);
		}
		else if (lastBet != 0)
		{

			redoBetButton.gameObject.SetActive(true);

		}
	}
}
