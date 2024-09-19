using System.Collections.Generic;
using UnityEngine;

public class SpiritDataIndex : MonoBehaviour
{
    private static SpiritDataIndex _i;

    public static SpiritDataIndex i {
        get{
            if(_i == null) _i = (Instantiate(Resources.Load("SpiritDataIndex")) as GameObject).GetComponent<SpiritDataIndex>();
            DontDestroyOnLoad(_i);
            return _i;
        }
    }

    //stats
    public Dictionary <string, int> statDictionary = new Dictionary<string, int>
    {
    //Dodomon
    {"DodomonHP", 10},
    {"DodomonStr", 1},
    {"DodomonDef", 1},
    {"DodomonSpe", 1},
    //Crabult
    {"CrabultHP", 10},
    {"CrabultStr", 5},
    {"CrabultDef", 5},
    {"CrabultSpe", 5},
    //Polpuff
    {"PolpuffHP", 10},
    {"PolpuffStr", 5},
    {"PolpuffDef", 5},
    {"PolpuffSpe", 5},
    //Salasear
    {"SalasearHP", 10},
    {"SalasearStr", 5},
    {"SalasearDef", 5},
    {"SalasearSpe", 5}
    };
    
    //gender ratios // higher means more likely to be female
    public Dictionary <string, float> genderRatioDictionary = new Dictionary<string, float>
    {
        {"Dodomon", 0.5f},
        {"Crabult", 0.5f},
        {"Polpuff", 0.5f},
        {"Salasear", 0.5f}
    };

    //types
    public Dictionary <string, SpiritType> spiritTypeDictionary = new Dictionary<string, SpiritType>
    {
        {"DodomonPrimary", SpiritType.basic},{"DodomonSecondary", SpiritType.none},
        {"CrabultPrimary", SpiritType.water},{"CrabultSecondary", SpiritType.none},
        {"PolpuffPrimary", SpiritType.flora},{"PolpuffSecondary", SpiritType.none},
        {"SalasearPrimary", SpiritType.fire},{"SalasearSecondary", SpiritType.none}
    };

    //asset references

    [Header("Attacks")]
    public SpiritAttackHolder attackHolder;

    [Header("Dodomon")]
    public Texture2D DodomonSprite;
    public Texture2D DodomonPortrait;
    public GameObject DodomonObj;

    [Header("Crabult")]
    public Texture2D CrabultSprite;
    public Texture2D CrabultPortrait;
    public GameObject CrabultObj;

    [Header("Polpuff")]
    public Texture2D PolpuffSprite;
    public Texture2D PolpuffPortrait;
    public GameObject PolpuffObj;

    [Header("Salasear")]
    public Texture2D SalasearSprite;
    public Texture2D SalasearPortrait;
    public GameObject SalasearObj;

    //asset dictionaries
    public Dictionary<string, GameObject> SpiritObjDictionary = new Dictionary<string, GameObject>();
    public Dictionary<string, Texture2D> SpiritSpriteDictionary = new Dictionary<string, Texture2D>();
    public Dictionary<string, Texture2D> SpiritPortraitDictionary = new Dictionary<string, Texture2D>();
    public Dictionary <string, SpiritAttacks[]> spiritAttackDictionary = new Dictionary<string, SpiritAttacks[]>();

    private bool DictionaryFilled = false;

    public void FillAssetDictionary() //gets called when game data is loaded
    {
        if(DictionaryFilled == false)
        {
            DictionaryFilled = true;

            //objects
            SpiritObjDictionary.Add("Dodomon", DodomonObj);
            SpiritObjDictionary.Add("Crabult", CrabultObj);
            SpiritObjDictionary.Add("Polpuff", PolpuffObj);
            SpiritObjDictionary.Add("Salasear", SalasearObj);

            //sprites
            SpiritSpriteDictionary.Add("Dodomon", DodomonSprite);
            SpiritSpriteDictionary.Add("Crabult", CrabultSprite);
            SpiritSpriteDictionary.Add("Polpuff", PolpuffSprite);
            SpiritSpriteDictionary.Add("Salasear", SalasearSprite);

            //portraits
            SpiritPortraitDictionary.Add("Dodomon", DodomonPortrait);
            SpiritPortraitDictionary.Add("Crabult", CrabultPortrait);
            SpiritPortraitDictionary.Add("Polpuff", PolpuffPortrait);
            SpiritPortraitDictionary.Add("Salasear", SalasearPortrait);

            //spiritAttacks
            spiritAttackDictionary.Add("Dodomon", new []{SpiritAttackHolder.attackIndexInstance.Dodopeck, SpiritAttackHolder.attackIndexInstance.Struggle, SpiritAttackHolder.attackIndexInstance.Obliterate});
            spiritAttackDictionary.Add("Crabult", new []{SpiritAttackHolder.attackIndexInstance.CrabSlash, SpiritAttackHolder.attackIndexInstance.Struggle});
            spiritAttackDictionary.Add("Polpuff", new []{SpiritAttackHolder.attackIndexInstance.CrabSlash, SpiritAttackHolder.attackIndexInstance.Struggle});
            spiritAttackDictionary.Add("Salasear", new []{SpiritAttackHolder.attackIndexInstance.CrabSlash, SpiritAttackHolder.attackIndexInstance.Struggle});  
        }

    }






    
}
