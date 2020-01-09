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

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerdata.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            credit = data.Credit;
            showAds = data.ShowAds;

        }
    }
}

[Serializable]
class PlayerData
{
    private bool showAds;
    private int credit;

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
}