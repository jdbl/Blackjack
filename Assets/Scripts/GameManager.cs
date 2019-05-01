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
    private Canvas mainCanvas;
    [SerializeField]
    private DeckOfCards deck;

    private List<Text> playerHandValuesText;
    public static int deckNumbers = 1;
    private bool playing = false;
    
    

    // Start is called before the first frame update
    void Start()
    {
        deck.BuildDeck();
        deck.ShuffleDeck();
        player.ResetHand();
        playerHandValuesText = new List<Text>();
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
        betText.text = betSlider.value.ToString();
    }
    
    public void Deal()
    {
        if(deck.GetDeckSize() <= (52*deckNumbers/2))
        {
            deck.BuildDeck();
            deck.ShuffleDeck();
        }
        Card newCard = deck.Deal();
        player.AddToHand(newCard);
        UpdateCardValues();
    }
    private void UpdateCardValues()
    {
        foreach(Text tempText in playerHandValuesText)
        {
            Destroy(tempText.gameObject);
        }
        playerHandValuesText.Clear();
        int loopCounter = 0;
        foreach(int value in player.GetHandValues())
        {
            GameObject tempObject = new GameObject("HandValue");
            tempObject.transform.SetParent(mainCanvas.transform);
            tempObject.layer = mainCanvas.gameObject.layer;
            tempObject.transform.position = new Vector3(400.0f, 200.0f, 0.0f);
            Text tempText = tempObject.AddComponent<Text>();

            tempText.text = value.ToString();
            tempText.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");


            playerHandValuesText.Add(tempText);
            loopCounter++;
        }
    }

}
