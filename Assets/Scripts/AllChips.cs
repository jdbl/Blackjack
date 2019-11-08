using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChips : MonoBehaviour
{
	[SerializeField]
	private GameObject[] chipPrefabs = null;
	[SerializeField]
	private int[] chipValues = null;

	private Chip[] chips = null;

	public void MakeChips()
	{
		chips = new Chip[chipPrefabs.Length];
		for (int i = 0; i < chipPrefabs.Length; i++)
		{
			chips[i] = new Chip(chipPrefabs[i], chipValues[i]); 
			
		}
	}

	public Chip FindChipByName(string name)
	{
		foreach(Chip testChip in chips)
		{
			if(testChip.ChipName == name)
			{
				return testChip;
			}
		}
		return null;
	}

	public Chip CreateChip(Chip chip)
	{
		return chip;
	}
}
