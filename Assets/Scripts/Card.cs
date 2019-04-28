using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public const int SPADE = 1;
    public const int CLUB = 2;
    public const int HEART = 3;
    public const int DIAMOND = 4;
    public int faceValue = 0;
    public int suit = 0;

    public Card(int _faceValue, int _suit)
    {
        this.faceValue = _faceValue;
        this.suit = _suit;
    }
    
    
}
