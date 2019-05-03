using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// TODO
/// Issue with shuffling when cards are in play
/// Needs to remember which cards are in play during shuffle
/// Verify GameManager.deckNumbers is working

/// <summary>
/// Creates a full deck of cards and deals and reshuffles wherever needed.
/// </summary>
public class DeckOfCards : MonoBehaviour
{
    [SerializeField]
    private GameObject[] deckPrefabs;

    private int numberOfCards = 52 * GameManager.deckNumbers;
    private List<Card> deck = new List<Card>();

    /// <summary>
    /// Creates the deck of cards depending on the number of decks specified in GameManager.
    /// </summary>
    public void BuildDeck()
    {
        deck.Clear();
        for (int suitIndex = Card.SPADE; suitIndex <= Card.DIAMOND * GameManager.deckNumbers; suitIndex++  )
        {
            for(int cardFace = 1; cardFace <= 13; cardFace++)
            {
                if(cardFace == 1)
                {
                    deck.Add(new Card(11, suitIndex % Card.DIAMOND * GameManager.deckNumbers, deckPrefabs[((13 * (suitIndex - 1)) + (cardFace - 1))]));
                }
                else if(cardFace < 10)
                {
                    deck.Add(new Card(cardFace, suitIndex % Card.DIAMOND * GameManager.deckNumbers, deckPrefabs[((13*(suitIndex-1))+(cardFace-1))]));
                }
                else
                {
                    deck.Add(new Card(10, suitIndex % Card.DIAMOND * GameManager.deckNumbers, deckPrefabs[((13 * (suitIndex-1)) + (cardFace-1))]));
                }
                
            }
        }
    }

    /// <summary>
    /// Randomly shuffles cards in deck.
    /// </summary>
    public void ShuffleDeck()
    {
        System.Random random = new System.Random();
        int currentCount = deck.Count;
        
        for(int index = deck.Count -1; index > 1; index--)
        {
            int randomNumber = random.Next(index + 1);

            Card value = deck[randomNumber];
            deck[randomNumber] = deck[index];
            deck[index] = value;
            
        }
    }

    /// <summary>
    /// Pulls the first card in the deck and returns it.
    /// </summary>
    public Card Deal()
    {
        Card newCard = deck[0];
        deck.RemoveAt(0);
        return newCard;

    }

    /// <summary>
    /// Returns remaing number of cards in the deck.
    /// </summary>
    public int GetDeckSize()
    {
        return deck.Count;
    }

    /// <summary>
    /// Returns the Prefabs of each card in the deck.
    /// </summary>
    public GameObject[] GetDeckPrefabs()
    {
        return deckPrefabs;
    }
}
