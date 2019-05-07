using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Individual cards to be dealt out.
/// </summary>
public class Card
{
	/*
		This class is the indiviual cards for play.

		Public Methods:
			Card(int _faceValue, int _suit, GameObject _prefab)
				_faceValue: Indiviual value of card
				_suit: Numerical representation of standard card suits
				_prefab: Prefab gameobject that is rendered in game
			ChangeAceFaceValue()
			GetSuit()
			GetFaceValue()
			GetPrefab()
	*/
	public const int SPADE = 1;
	public const int CLUB = 2;
	public const int HEART = 3;
	public const int DIAMOND = 4;
	private int faceValue = 0;
	private int suit = 0;
	private GameObject prefab;

	public Card(int _faceValue, int _suit, GameObject _prefab)
	{
		this.faceValue = _faceValue;
		this.suit = _suit;
		this.prefab = _prefab;
	}
	/// <summary>
	/// Change the value of an ace from depending on need
	/// </summary>
	public void ChangeAceFaceValue()
	{
        
		if (faceValue == 1)
		{
			faceValue = 11;
		}
		else if(faceValue == 11)
		{
			faceValue = 1;
		}
	}
    
	/// <summary>
	/// Return Card's suit
	/// </summary>
	/// <returns>int</returns>
	public int GetSuit()
	{
		return suit;
	}

	/// <summary>
	/// Return Card's Face Value
	/// </summary>
	/// <returns>int</returns>
	public int GetFaceValue()
	{
		return faceValue;
	}

	/// <summary>
	/// Return Card's Prefab
	/// </summary>
	/// <returns>GameObject</returns>
	public GameObject GetPrefab()
	{
		return prefab;
	}
}
