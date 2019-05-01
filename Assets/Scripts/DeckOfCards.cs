using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// TODO
/// Issue with shuffling when cards are in play
/// Needs to remember which cards are in play during shuffle
public class DeckOfCards : MonoBehaviour
{
    [SerializeField]
    private GameObject[] deckPrefabs;

    private int numberOfCards = 52 * GameManager.deckNumbers;
    private List<Card> deck = new List<Card>();

    public void BuildDeck()
    {
        deck.Clear();
        for (int suitIndex = Card.SPADE; suitIndex <= Card.DIAMOND * GameManager.deckNumbers; suitIndex++  )
        {
            for(int cardFace = 1; cardFace <= 13; cardFace++)
            {
                
                if(cardFace < 10)
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

    public Card Deal()
    {
        Card newCard = deck[0];
        deck.RemoveAt(0);
        return newCard;

    }
    public int GetDeckSize()
    {
        return deck.Count;
    }
    
    public GameObject[] GetDeckPrefabs()
    {
        return deckPrefabs;
    }
}
