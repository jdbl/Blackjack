﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Control for dealer play.
/// </summary>
public class DealerController : MonoBehaviour
{

	/*
		This class controls all dealer actions and monitors dealers cards.

		Public Methods:
			GetHand()
			GetHandValue()
			AddToHand()
			FlipCard()
			ResetHand()
			GetHandFinished()
			CardCount()
			FinishHand()
			GetHandPrefabs()

		Private Methods:
			DisplayCard(Card newCard)
				newCard: Card to instantiate on table
			DisplayCard(Card newCard, bool second)
				newCard: Card to instantiate on table
				second: bool determining if new card should be face down
	*/

	[SerializeField]
	private List<Card> hand = new List<Card>();


	private int cardCount = 0;
	private int handValue = 0;
	private bool handFinished = false;
	private List<GameObject> handPrefabs = new List<GameObject>();
    private Vector3 initialPosition = new Vector3();
	private GameObject scoreText = null;
	private GameObject handController = null;

	private void Start()
    {
        initialPosition = transform.position;
    }

    /// <summary>
    /// Returns current dealer hand.
    /// </summary>
    /// <returns>List(Card)</returns>
    public List<Card> GetHand()
	{
		return hand;
	}

	/// <summary>
	/// Returns current value of dealers current hand.
	/// </summary>
	/// <returns>int</returns>
	public int GetHandValue()
	{
		return handValue;
	}

	/// <summary>
	/// Add Card to dealer hand.
	/// </summary>
	/// <param name="newCard">New card to be added to the hand.</param>
	public void AddToHand(Card newCard)
	{//Add new card dealt to players hand
        
		if (handValue + newCard.GetFaceValue() > 21 && (newCard.GetFaceValue() == 11))
		{
			newCard.ChangeAceFaceValue();
		}
		else if (handValue + newCard.GetFaceValue() > 21)
		{
			for (int index = 0; index < hand.Count; index++)
			{
				if (hand[index].GetFaceValue() == 11)
				{
					hand[index].ChangeAceFaceValue();
					handValue -= 10;
					if (handValue + newCard.GetFaceValue() <= 21)
					{
						index = hand.Count;
					}
				}
			}
		}
		handValue += newCard.GetFaceValue();
		hand.Add(newCard);
		if(cardCount == 1)
		{
			DisplayCard(newCard, true);
		}
		else
		{
			DisplayCard(newCard);
			if(hand.Count > 2)
			{
				this.transform.Translate(new Vector3(-0.2f, 0.0f, 0.0f));
			}
		}

		cardCount++;
		if (handValue >= 17)
		{
			handFinished = true;
		}
	}

	/// <summary>
	/// Instantiate card and render gameobject.
	/// </summary>
	/// <param name="newCard">New card to be rendered.</param>
	private void DisplayCard(Card newCard)
	{
		if (handPrefabs.Count == 0)
		{
			handController = new GameObject("dealerHandController");
			handController.transform.SetParent(this.transform);
			//handController.transform.position = -(new Vector3(0.0f, 0.0f, Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, -(Screen.height - GetBannerHeight()))).z));
			handController.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
			Vector3 bannerHeight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 7.74255f)) + Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height - GetBannerHeight(), 7.74255f));
			handController.transform.Translate(0.0f, 0.0f, -bannerHeight.normalized.y);
			CreateScoreText();
		}

		//handPrefabs.Add(Instantiate(newCard.GetPrefab(), this.transform));
		handPrefabs.Add(Instantiate(newCard.GetPrefab(), this.transform.GetChild(0)));
		handPrefabs[handPrefabs.Count - 1].transform.localPosition = new Vector3((hand.Count - 1) * 0.4f, (hand.Count -1) * 0.001f, 0.0f);
		//handPrefabs[handPrefabs.Count-1].transform.SetPositionAndRotation(new Vector3((float)hand.Count, 3.5f, -1.0f), new Quaternion(180.0f, 0.0f, 0.0f, 0.0f));
		handPrefabs[handPrefabs.Count - 1].transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
		/*if(handPrefabs.Count > 2)
        {
            this.transform.position = this.transform.position;
            this.transform.localPosition = this.transform.localPosition;
        }*/

		UpdateScoreTextPosition();
	}

	/// <summary>
	/// Instantiate card and render gameobject face down.
	/// </summary>
	/// <param name="newCard">New card to be rendered.</param>
	/// <param name="second">Render card face down</param>
	private void DisplayCard(Card newCard, bool second)
	{
		handPrefabs.Add(Instantiate(newCard.GetPrefab(), this.transform.GetChild(0)));
		handPrefabs[handPrefabs.Count - 1].transform.localPosition = new Vector3((hand.Count - 1) * 0.4f, (hand.Count - 1) * 0.001f, 0.0f);
		handPrefabs[handPrefabs.Count - 1].transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
		//handPrefabs[1].transform.SetPositionAndRotation(new Vector3((float)hand.Count, 3.5f, -1.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		handPrefabs[1].transform.localScale = new Vector3(15.0f, 15.0f, 1.0f);
		UpdateScoreTextPosition();
	}
	private void UpdateScoreTextPosition()
	{
		int textOverMesh = handPrefabs.Count / 2;
		MeshRenderer cardMesh = handPrefabs[textOverMesh].GetComponent<MeshRenderer>();
		Vector3 topOfHand = new Vector3(this.transform.GetChild(0).position.x,
			cardMesh.bounds.max.y, cardMesh.bounds.max.z);
		scoreText.transform.position = Camera.main.WorldToScreenPoint(topOfHand);
	}

	private void CreateScoreText()
	{
		scoreText = new GameObject("dealerScoreText");
		scoreText.transform.SetParent(this.transform.parent.Find("Canvas"));

		Text text = scoreText.AddComponent<Text>();
		text.rectTransform.anchorMax = new Vector2(0.0f, 0.0f);
		text.rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
		text.rectTransform.pivot = new Vector2(0.0f, 0.0f);
		text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
		text.text = "";
		text.fontSize = 80;
		text.color = new Color(0.0f, 0.0f, 0.0f, 255.0f);
	}
	/// <summary>
	/// Flip second dealer card facing up.
	/// </summary>
	public void FlipCard()
	{
		handPrefabs[handPrefabs.Count - 1].transform.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
	}

	/// <summary>
	/// Reset hand to empty default.
	/// </summary>
	public void ResetHand()
	{
		hand.Clear();
		handFinished = false;
		handValue = 0;
		cardCount = 0;
        transform.position = initialPosition;
		
		try
		{
			Destroy(scoreText);
			foreach (GameObject temp in handPrefabs)
			{
				Destroy(temp);
			}
			
			handPrefabs.Clear();
			foreach (Transform child in this.transform)
			{
				Destroy(child.gameObject);
			}
		}
		catch { }

		this.transform.DetachChildren();
	}

	/// <summary>
	/// Return dealers current hand status.
	/// </summary>
	/// <returns>bool</returns>
	public bool GetHandFinished()
	{
		if(handFinished)
		{
			return handFinished;
		}
		return handFinished;
	}

	/// <summary>
	/// Current number of cards in hand
	/// </summary>
	/// <returns></returns>
	public int CardCount()
	{
		return cardCount;
	}
    
	/// <summary>
	/// Designate dealer turn is finished.
	/// </summary>
	public void FinishHand()
	{
		handFinished = true;
	}

	/// <summary>
	/// Returns prefabs of all cards in hand.
	/// </summary>
	/// <returns></returns>
	public List<GameObject> GetHandPrefabs()
	{
		return handPrefabs;
	}

	public GameObject ScoreText
	{
		get { return scoreText; }
	}

	private float GetBannerHeight()
	{
		return Mathf.RoundToInt(50 * Screen.dpi / 160);
	}
}
