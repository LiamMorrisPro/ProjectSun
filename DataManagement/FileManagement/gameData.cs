using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //player data
    public Vector3 playerPosition;
    //list of generated spirits and all of their data
 
    public List<SpiritData> PlayerPartySpirits;
    public List<SpiritData> SpiritBox1;


    //values defined in this constructor will be our default values
    //that the game starts with when there is no save data

    public GameData()
    {
        this.playerPosition = new Vector3(0,16,-3);
        this.PlayerPartySpirits = new List<SpiritData>();
        this.SpiritBox1 = new List<SpiritData>();
    }


}
