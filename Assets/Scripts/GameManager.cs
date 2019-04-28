﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Slider BetSlider;
    [SerializeField]
    private Text BetText;
    [SerializeField]
    private PlayerController player;

    public static int DeckNumbers = 1;
    private bool playing = false;
    DeckOfCards deck = new DeckOfCards();

    // Start is called before the first frame update
    void Start()
    {
        deck.BuildDeck();
        deck.ShuffleDeck();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBetText();
    }

    public void StartGame()
    {
        playing = true;
    }

    private void UpdateBetText()
    {
        BetText.text = BetSlider.value.ToString();
    }

    public void Deal()
    {
        if(deck.GetDeckSize() <= (52*DeckNumbers/2))
        {
            deck.BuildDeck();
            deck.ShuffleDeck();
        }
        Card newCard = deck.Deal();

    }

}
