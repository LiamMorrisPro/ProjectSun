using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SpiritGenerate : MonoBehaviour
{
    public Spirit generateSpiritNew(int _level, string _spiritName)
    {
        Spirit spirit = ScriptableObject.CreateInstance<Spirit>();
        
        //visuals
        spirit.body = SpiritDataIndex.i.SpiritObjDictionary[_spiritName];
        spirit.sprite = SpiritDataIndex.i.SpiritSpriteDictionary[_spiritName];
        spirit.portrait = SpiritDataIndex.i.SpiritPortraitDictionary[_spiritName];

        //identifier data
        spirit.SpiritName = _spiritName;
        spirit.NickName = _spiritName; // set as spiritname
        spirit.level = _level;

        //set gender based on random generation // 
        if(Random.Range(0f, 1f) > SpiritDataIndex.i.genderRatioDictionary[_spiritName])
        {spirit.gender = Spiritgender.Male;}
        else
        {spirit.gender = Spiritgender.Female;}
        
        
        //stats //set currentHP as Max
        spirit.baseMaxHp = SpiritDataIndex.i.statDictionary[_spiritName + "HP"];
        spirit.baseAttack = SpiritDataIndex.i.statDictionary[_spiritName + "Str"];
        spirit.baseDefense = SpiritDataIndex.i.statDictionary[_spiritName + "Def"];
        spirit.baseSpeed = SpiritDataIndex.i.statDictionary[_spiritName + "Spe"];
        spirit.currentHP = spirit.baseMaxHp;
        //types
        spirit.primaryType = SpiritDataIndex.i.spiritTypeDictionary[_spiritName + "Primary"];
        spirit.secondaryType = SpiritDataIndex.i.spiritTypeDictionary[_spiritName + "Secondary"];

        //moves - get possible moves, compare to level and build from there from highest level cost
        spirit.capableAttacks = SpiritDataIndex.i.spiritAttackDictionary[_spiritName];

        for(int i = 0; i < spirit.capableAttacks.Count(); i++) //currently gives the attacks in the capableattacks list in order
        {
            if(i < 3)
                spirit.attacks[i] = spirit.capableAttacks[i];
        }


        spirit.InitEffectiveStats();
        return spirit;
    }

    public Spirit generateSpirit(int _level, Spiritgender _gender, string _spiritName, string _nickName, float _currentHealth, List<string> currentAttacks)
    {
        Spirit spirit = ScriptableObject.CreateInstance<Spirit>();

        //visuals
        spirit.body = SpiritDataIndex.i.SpiritObjDictionary[_spiritName];
        spirit.sprite = SpiritDataIndex.i.SpiritSpriteDictionary[_spiritName];
        spirit.portrait = SpiritDataIndex.i.SpiritPortraitDictionary[_spiritName];
        //identifier data
        spirit.SpiritName = _spiritName;
        spirit.NickName = _nickName;
        spirit.level = _level;
        spirit.gender = _gender;
        
        //stats //set currentHP from parameter
        spirit.baseMaxHp = SpiritDataIndex.i.statDictionary[_spiritName + "HP"];
        spirit.baseAttack = SpiritDataIndex.i.statDictionary[_spiritName + "Str"];
        spirit.baseDefense = SpiritDataIndex.i.statDictionary[_spiritName + "Def"];
        spirit.baseSpeed = SpiritDataIndex.i.statDictionary[_spiritName + "Spe"];
        spirit.currentHP = _currentHealth;
        //types
        spirit.primaryType = SpiritDataIndex.i.spiritTypeDictionary[_spiritName + "Primary"];
        spirit.secondaryType = SpiritDataIndex.i.spiritTypeDictionary[_spiritName + "Secondary"];
        //moves
        spirit.capableAttacks = SpiritDataIndex.i.spiritAttackDictionary[_spiritName];
        
        for(int i = 0; i < currentAttacks.Count(); i++)
        {
            spirit.attacks[i] = SpiritAttackHolder.attackIndexInstance.TotalAttackDictionary[currentAttacks[i]];
        }

        
        
        spirit.updateEffectiveStats();
        return spirit;
    }

    public Spirit generateEmptySpirit()
    {
        Spirit spirit = ScriptableObject.CreateInstance<Spirit>();

        spirit.body = null;
        spirit.sprite = null;     

        spirit.SpiritName = "";
        spirit.NickName = "";
        spirit.level = 0;
        spirit.gender = Spiritgender.NA;

        spirit.baseMaxHp = 0;
        spirit.baseAttack = 0;
        spirit.baseDefense = 0;
        spirit.baseSpeed = 0;
        spirit.currentHP = 0;

        spirit.primaryType = SpiritType.none;
        spirit.secondaryType = SpiritType.none;

        return spirit;
    }

}
