using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
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

    private List<Text> playerHandValuesText;
    public static int deckNumbers = 1;
    private bool playerTurn = true;

    

    // Start is called before the first frame update
    void Start()
    {

        deck.BuildDeck();
        deck.ShuffleDeck();
        player.ResetHand();
        playerHandValuesText = new List<Text>();
        standButton.interactable = false;
        splitButton.interactable = false;
        hitButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBetText();
    }

    public void StartGame()
    {
        
    }

    private void UpdateBetText()
    {
        betText.text = betSlider.value.ToString();
    }

    public void DealGame()
    {
        ResetGame();
        betSlider.interactable = false;
        betButton.interactable = false;
        standButton.interactable = true;
        hitButton.interactable = true;
        for (int index = 0; index <= 3; index++)
        {
            if (deck.GetDeckSize() <= (52 * deckNumbers / 2))
            {
                deck.BuildDeck();
                deck.ShuffleDeck();
            }
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
        UpdateCardValues();
    }

    public void Deal()
    {
        if(deck.GetDeckSize() <= (52*deckNumbers/2))
        {
            deck.BuildDeck();
            deck.ShuffleDeck();
        }
        Card newCard = deck.Deal();
        if(playerTurn)
        {
            player.AddToHand(newCard);
        }
        else
        {
            dealer.AddToHand(newCard);
        }
        
        UpdateCardValues();
    }

    public void UpdateCardValues()
    {
        foreach(Text tempText in playerHandValuesText)
        {
            Destroy(tempText.gameObject);
        }

        playerHandValuesText.Clear();

        playerHandText.text = "Player: ";


        for (int index = 0; index < player.GetHandValues().Count; index++)
        {
            int value = player.GetHandValues()[index];
            if(value > 21)
            {
                playerHandText.text += System.Environment.NewLine +
                value.ToString() + " BUST";

            }
            else if(value == 21 && player.GetHand()[index].Count == 2)
            {
                playerHandText.text += System.Environment.NewLine +
                value.ToString() + " BLACKJACK";

            }
            else
            {
                playerHandText.text += System.Environment.NewLine +
                value.ToString();
            }

        }
        if (player.GetHandFinished()[player.GetHandCount()] == true)
        {
            standButton.interactable = false;
            splitButton.interactable = false;
            hitButton.interactable = false;
            ChangePlayerTurn();
        }
    }


    public void ChangePlayerTurn()
    {
        if(playerTurn)
        {
            playerTurn = false;
            PlayDealer();
        }
        else
        {
            playerTurn = true;
        }
    }

    private void PlayDealer()
    {
        dealer.FlipCard();
        while (!dealer.GetHandFinished() )
        {
            Deal();
        }
        CalculateWinnings();
        betButton.interactable = true;
    }
    private void ResetGame()
    {
        betSlider.interactable = true;
        betButton.interactable = true;
        player.ResetHand();
        dealer.ResetHand();

    }

    private void CalculateWinnings()
    {
        for(int index = 0; index < player.GetHandValues().Count; index++ )
        {
            if(player.GetHandValues()[index] == dealer.GetHandValue() )
            {
                player.SetCredit(player.GetCredit() + player.GetBets()[index]);
            }
        }
    }
}
