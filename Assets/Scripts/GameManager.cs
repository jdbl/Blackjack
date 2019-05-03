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
            DealGame()
            Deal()
            UpdateCardValues()
            ChangePlayerTurn()

        Private Methods:
            UpdateBetText()
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
    private Slider betSlider;
    [SerializeField]
    private Text betText;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private DealerController dealer;
    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private DeckOfCards deck;
    [SerializeField]
    private Text playerHandText;
    [SerializeField]
    private Text dealerHandText;
    [SerializeField]
    private Button betButton;
    [SerializeField]
    private Button standButton;
    [SerializeField]
    private Button splitButton;
    [SerializeField]
    private Button hitButton;
    [SerializeField]
    private Text creditText;

    private List<Text> playerHandValuesText;
    public static int deckNumbers = 1;
    private bool playerTurn = true;

    

    // Start is called before the first frame update
    void Start()
    {
        deck.BuildDeck();
        deck.ShuffleDeck();
        player.SetCredit(100);
        player.ResetHand();
        playerHandValuesText = new List<Text>();
        creditText.text = "Credit: " + player.GetCredit().ToString();
        standButton.interactable = false;
        splitButton.interactable = false;
        hitButton.interactable = false;
    }

  

    /// <summary>
    /// Update user bet on display
    /// </summary>
    public void UpdateBetText()
    {
        betText.text = System.Math.Round(betSlider.value, 2).ToString();
    }

    /// <summary>
    /// First deal of the game.
    /// </summary>
    public void DealGame()
    {
        ResetGame();
        betSlider.interactable = false;
        betButton.interactable = false;
        player.PlaceBet();
        standButton.interactable = true;
        hitButton.interactable = true;
        deck.BuildDeck();
        deck.ShuffleDeck();
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
        if(player.Splitable())
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

        if (player.Splitable())
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
        foreach(Text tempText in playerHandValuesText)
        {
            Destroy(tempText.gameObject);
        }

        playerHandText.text = "Player: ";
        dealerHandText.text = "Dealer: ";
        int value = 0;
        for (int index = 0; index < player.GetHandValues().Count; index++)
        {
            value = player.GetHandValues()[index];
            if(value > 21)
            {
                playerHandText.text += System.Environment.NewLine +
                value.ToString() + " BUST";

            }
            else if(value == 21 && player.GetHand()[index].Count == 2)
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
        value = dealer.GetHandValue();
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

        while (!dealer.GetHandFinished() )
        {
            Deal();
        }
        CalculateWinnings();
        betButton.interactable = true;
        betSlider.interactable = true;
        ChangePlayerTurn();
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
        for(int index = 0; index < player.GetHandValues().Count; index++ )
        {
            if(player.GetHandValues()[index] == dealer.GetHandValue() )
            {
                player.SetCredit(player.GetCredit() + player.GetBets()[index]);
            }
            else if (player.GetHandValues()[index] == 21 && player.GetHand()[index].Count == 2)
            {
                player.SetCredit((float)System.Math.Round(player.GetCredit() + player.GetBets()[index] * 2.5f, 2));

            }
            else if(player.GetHandValues()[index] > dealer.GetHandValue() && player.GetHandValues()[index] <=21)
            {
                player.SetCredit(player.GetCredit() + (player.GetBets()[index]) * 2);
            }
            else if(player.GetHandValues()[index] <= 21 && dealer.GetHandValue() > 21)
            {
                player.SetCredit(player.GetCredit() + (player.GetBets()[index]) * 2);
            }
            
        }
        creditText.text = "Credit: " + player.GetCredit().ToString();
    }

    public void Stand()
    {
        player.NextSplitHand();
        if (player.GetHandFinished()[player.GetHandCount()])
        {
            standButton.interactable = false;
            splitButton.interactable = false;
            hitButton.interactable = false;
            ChangePlayerTurn();
        }
        if (player.Splitable())
        {
            splitButton.interactable = true;
        }
        else
        {
            splitButton.interactable = false;
        }
    }
    public void Hit()
    {
        Deal();
        if (player.GetHandFinished()[player.GetHandCount()])
        {
            standButton.interactable = false;
            splitButton.interactable = false;
            hitButton.interactable = false;
            ChangePlayerTurn();
        }
    }
    public void Split()
    {
        player.Split(deck.Deal(), deck.Deal());
        if (player.Splitable())
        {
            splitButton.interactable = true;
        }
        else
        {
            splitButton.interactable = false;
        }
    }
}
