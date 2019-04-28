using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// TODO
/// Issue with shuffling when cards are in play
/// Needs to remember which cards are in play during shuffle
public class DeckOfCards : MonoBehaviour
{
    private int numberOfCards = 52 * GameManager.DeckNumbers;
    private List<Card> deck = new List<Card>();

    public void BuildDeck()
    {
        for(int suitIndex = Card.SPADE; suitIndex <= Card.DIAMOND * GameManager.DeckNumbers; suitIndex++  )
        {
            for(int cardFace = 1; cardFace <= 13; cardFace++)
            {
                deck.Clear();
                deck.Add(new Card(cardFace, suitIndex % Card.DIAMOND * GameManager.DeckNumbers));
            }
        }
    }
    public void ShuffleDeck()
    {
        System.Random random = new System.Random();
        int currentCount = deck.Count;
        
        for(int index = deck.Count -1; index > 1; index++)
        {
            int randomNumber = random.Next(index + 1);

            Card value = deck[randomNumber];
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
}
