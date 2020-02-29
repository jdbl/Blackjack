using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    private bool showAds = true;
    private int credit = 0;
	private DateTime lastPlayed = DateTime.Now;
	private bool[] consecutiveDaysPlayed;

	public static SaveAndLoad control;
    private void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerdata.dat");

        PlayerData data = new PlayerData();
        data.ShowAds = showAds;
        data.Credit = credit;
		data.LastPlayed = DateTime.Now;
		data.ConsecutiveDaysPlayed = consecutiveDaysPlayed;
		bf.Serialize(file, data);
        file.Close();
    }

    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerdata.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerdata.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

			consecutiveDaysPlayed = data.ConsecutiveDaysPlayed;
			lastPlayed = data.LastPlayed;
            credit = data.Credit;
            showAds = data.ShowAds;
			return true;
        }
		return false;
    }

	private void UpdateDaysPlayed()
	{
		DateTime current = DateTime.Now;
		DateTime startOfWeek = current.AddDays(-(int)current.DayOfWeek);

		if (current-lastPlayed > current-startOfWeek && lastPlayed != current)
		{
			consecutiveDaysPlayed = new bool[7];
			lastPlayed = current;
		}
		else
		{
			consecutiveDaysPlayed[(int)current.DayOfWeek] = true;
			lastPlayed = current;
		}
	}

	public int Credit
	{
		get { return credit; }
		set { credit = value; }
	}
	public bool ShowAds
	{
		get { return showAds; }
		set { showAds = value; }
	}

	public bool[] ConsecutiveDaysPlayed
	{
		get { return consecutiveDaysPlayed; }
		set { consecutiveDaysPlayed = value; }
	}
}

[Serializable]
class PlayerData
{
    private bool showAds;
    private int credit;
	private DateTime lastPlayed;
	private bool[] consecutiveDaysPlayed = { false, false, false, false, false, false, false};

    public int Credit
    {
        get { return credit; }
        set { credit = value; }
    }

    public bool ShowAds
    {
        get { return showAds; }
        set { showAds = value; }
    }

	public DateTime LastPlayed
	{
		get { return lastPlayed; }
		set { lastPlayed = value; }
	}

	public bool[] ConsecutiveDaysPlayed
	{
		get { return consecutiveDaysPlayed; }
		set { consecutiveDaysPlayed = value; }
	}
}