using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{



    public int DamageCalculation(SpiritAttacks attack, Spirit AttackingSpirit, Spirit DefendingSpirit, int moveIndex) //might have to rework if you want to have dialog change depending on hit effectiveness
    {
        int damage = 0;

        //get attack info
        float baseDamage = attack.getDamageAmount(moveIndex);
        string AttackType = attack.moveType.ToString();

        //get attacking spirits info
        string primaryAtkType =AttackingSpirit.primaryType.ToString();
        string secondaryAtkType =  AttackingSpirit.secondaryType.ToString();

        //get defending spirits info
        string primaryDefType = DefendingSpirit.primaryType.ToString();
        string secondaryDefType =  DefendingSpirit.secondaryType.ToString();

        //type effectivness
        float effectiveness = GetEffectiveness(attack.moveType, DefendingSpirit.primaryType, DefendingSpirit.secondaryType);

        //STAB
        float stab = 1;
        if(attack.moveType.ToString() == primaryAtkType || attack.moveType.ToString() == secondaryAtkType)
            {stab = 1.25f;}

        //calculate damage
        damage = (baseDamage * effectiveness * stab).ConvertTo<int>();
        
        //return as int
        return damage;
    }


    public float GetEffectiveness(MoveType atkType, SpiritType defTypePrimary, SpiritType defTypeSecondary)
    {
        float effectiveness;
        int EffectVar = 3;
        if(atkType == MoveType.basic)
        {

        }
        else if(atkType == MoveType.fire)
        {
            if(defTypePrimary == SpiritType.fire || defTypeSecondary == SpiritType.fire){EffectVar -= 1;}
            if(defTypePrimary == SpiritType.water || defTypeSecondary == SpiritType.water){EffectVar -= 1;}
            if(defTypePrimary == SpiritType.flora || defTypeSecondary == SpiritType.flora){EffectVar += 1;}
        }
        else if(atkType == MoveType.water)
        {
            if(defTypePrimary == SpiritType.water || defTypeSecondary == SpiritType.water){EffectVar -= 1;}
            if(defTypePrimary == SpiritType.flora || defTypeSecondary == SpiritType.flora){EffectVar -= 1;}
            if(defTypePrimary == SpiritType.fire || defTypeSecondary == SpiritType.fire){EffectVar += 1;}
        }
        else if(atkType == MoveType.flora)
        {
            if(defTypePrimary == SpiritType.flora || defTypeSecondary == SpiritType.flora){EffectVar -= 1;}
            if(defTypePrimary == SpiritType.fire || defTypeSecondary == SpiritType.fire){EffectVar -= 1;}
            if(defTypePrimary == SpiritType.water || defTypeSecondary == SpiritType.water){EffectVar += 1;}
        }
        else if(atkType == MoveType.earth)
        {
            if(defTypePrimary == SpiritType.earth || defTypeSecondary == SpiritType.earth){EffectVar -= 1;}
            if(defTypePrimary == SpiritType.fire || defTypeSecondary == SpiritType.fire){EffectVar += 1;}
            if(defTypePrimary == SpiritType.electric || defTypeSecondary == SpiritType.electric){EffectVar += 1;}
        }
        else if(atkType == MoveType.wind)
        {
            if(defTypePrimary == SpiritType.wind || defTypeSecondary == SpiritType.wind){EffectVar -= 1;}
            if(defTypePrimary == SpiritType.earth || defTypeSecondary == SpiritType.earth){EffectVar += 1;}
            if(defTypePrimary == SpiritType.flora || defTypeSecondary == SpiritType.flora){EffectVar += 1;}
        }
        else if(atkType == MoveType.electric)
        {
            if(defTypePrimary == SpiritType.electric || defTypeSecondary == SpiritType.electric){EffectVar -= 1;}
            if(defTypePrimary == SpiritType.water || defTypeSecondary == SpiritType.water){EffectVar += 1;}
            if(defTypePrimary == SpiritType.wind || defTypeSecondary == SpiritType.wind){EffectVar += 1;}
        }


        switch (EffectVar)
        {
            case 5:
                effectiveness = 4f;
                break;
            case 4:
                effectiveness = 2f;
                break;
            case 3:
                effectiveness = 1f;
                break;
            case 2:
                effectiveness = 0.5f;
                break;
            case 1:
                effectiveness = 0.25f;
                break;
            default:
                effectiveness = 1f;
                break;
        }
        return effectiveness;
    }
}
