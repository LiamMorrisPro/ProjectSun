using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpiritData
{
    public string Nickname;
    public string SpiritName;
    public int level;
    public Spiritgender gender;
    public float currentHealth;
    public List<string> currentAttacks;
}

 
public class SpiritSaveData : MonoBehaviour, IDataPersistance
{
    public SpiritGenerate spiritGenerator;     

    public static List<Spirit> PlayerPartySpirits = new List<Spirit>(); //stores party in game
    public static List<SpiritData> PlayerPartySpiritData = new List<SpiritData>(); //stores party data

    public static List<Spirit> SpiritBox1 = new List<Spirit>();
    public static List<SpiritData> SpiritBox1Data = new List<SpiritData>();

/////////////////////////////////////////

//LoadData and SaveData are called whenever the game is loaded or saved

/////////////////////////////////////////

    public void LoadData(GameData data)
    {
        //make sure data is properly stored
        SpiritDataIndex.i.FillAssetDictionary();
        SpiritAttackHolder.attackIndexInstance.FillAttackDictionary();

        PlayerPartySpiritData = data.PlayerPartySpirits;
        SpiritBox1Data = data.SpiritBox1;
        convertDataToSpirit();
    }
    public void SaveData(ref GameData data)
    {
        convertSpiritToData();
        data.PlayerPartySpirits = PlayerPartySpiritData;
        data.SpiritBox1 = SpiritBox1Data;
    }

    public SpiritData ToData(Spirit spirit)
    {
        List<string> attacks = new List<string>();

        for(int i = 0; i < 3; i++)
        {
            if(spirit.attacks[i] != null)
                attacks.Add(spirit.attacks[i].moveName);
        }
 
        SpiritData spiritData = new SpiritData
        {
            Nickname = spirit.NickName,
            SpiritName = spirit.SpiritName,
            level = spirit.level,
            gender = spirit.gender,
            currentHealth = spirit.currentHP,
            currentAttacks = attacks
        };

        return spiritData;
    }

    public Spirit ToSpirit(SpiritData data)
    {
        Spirit spirit;

        if(data.SpiritName != "")
        {
            spirit = spiritGenerator.generateSpirit(data.level, data.gender, data.SpiritName, data.Nickname, data.currentHealth, data.currentAttacks);
            return spirit;
        }
        else
        {
            spirit = spiritGenerator.generateEmptySpirit();
        }

        //returns null spirit if spirit has no name
        return spirit;

    }

    public void convertSpiritToData()
    {
        PlayerPartySpiritData = new List<SpiritData>();
        SpiritBox1Data = new List<SpiritData>();

        //for player party
        for(int i = 0; i < PlayerPartySpirits.Count(); i++)
        {
            if(PlayerPartySpirits[i] != null)
                PlayerPartySpiritData.Add(ToData(PlayerPartySpirits[i]));
        }

        //for box 1
        for(int i = 0; i < SpiritBox1.Count(); i++)
        {
            if(SpiritBox1[i] != null)
                SpiritBox1Data.Add(ToData(SpiritBox1[i]));
        }
    }

    public void convertDataToSpirit()
    {
        //for player party
        for(int i = 0; i < PlayerPartySpiritData.Count(); i++)
        {
            if(PlayerPartySpiritData[i] != null)
                PlayerPartySpirits.Add(ToSpirit(PlayerPartySpiritData[i]));
        }

        //for box 1
        for(int i = 0; i < SpiritBox1Data.Count(); i++)
        {
            if(SpiritBox1Data[i] != null)
                SpiritBox1.Add(ToSpirit(SpiritBox1Data[i]));
        }

        //handle no spirits in party
        if(PlayerPartySpirits.Count() < 1)
        {
            PlayerPartySpirits.Add(spiritGenerator.generateSpiritNew(5,"Dodomon"));
        }

    }

}
