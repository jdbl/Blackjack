using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

	private int chipValue;
	private string chipName;

	public Chip(GameObject _prefab, int _chipValue)
	{
		prefab = _prefab;
		chipValue = _chipValue;
	}

	public GameObject Prefab
	{
		get { return prefab; }
		set { prefab = value; }
	}

	public int ChipValue
	{
		get { return chipValue; }
		set { chipValue = value; }
	}

	public string ChipName
	{
		get { return chipName; }
		set { chipName = value; }
	}
}
