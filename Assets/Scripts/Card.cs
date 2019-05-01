using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
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
    
    public void ChangeAceFaceValue()
    {
        if(faceValue == 1)
        {
            faceValue = 11;
        }
        else if(faceValue == 11)
        {
            faceValue = 1;
        }
    }
    
    public int GetSuit()
    {
        return suit;
    }
    public int GetFaceValue()
    {
        return faceValue;
    }
    public GameObject GetPrefab()
    {
        return prefab;
    }
}
