using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveAndLoad
{
    private bool showAds = true;
    private int credit = 0;
	public DateTime lastPlayed = DateTime.Now;
	private bool[] consecutiveDaysPlayed;
	private bool playedFullWeek = false;
	private bool newDay = false;
	public static SaveAndLoad control;
    private void Awake()
    {
		/*if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }*/

		control = this;
	}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerdata.dat");

        PlayerData data = new PlayerData();
        data.ShowAds = showAds;
        data.Credit = credit;
		data.LastPlayed = lastPlayed;
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
			UpdateDaysPlayed();
			return true;
        }
		return false;
    }

	private void UpdateDaysPlayed()
	{
		DateTime current = DateTime.Now;
		DateTime startOfWeek = current.AddDays(-(int)current.DayOfWeek);

		if((current.DayOfYear - lastPlayed.DayOfYear) > 1)
		{
			consecutiveDaysPlayed = new bool[7];
			consecutiveDaysPlayed[0] = true;
			lastPlayed = current;
		}
		else if((current.DayOfYear - lastPlayed.DayOfYear) == 1)
		{
			consecutiveDaysPlayed[Array.IndexOf(consecutiveDaysPlayed, false)] = true;
			lastPlayed = current;
			credit += 10;
			newDay = true;
			if (Array.IndexOf(consecutiveDaysPlayed, false) == -1)
			{
				consecutiveDaysPlayed = new bool[7];
				consecutiveDaysPlayed[0] = true;
				credit += 40;
			}
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

	public bool NewDay
	{
		get { return newDay; }
		set { newDay = value; }
	}
	public bool PlayedFullWeek
	{
		get { return playedFullWeek; }
		set { playedFullWeek = value; }
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