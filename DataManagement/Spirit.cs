using UnityEngine;

public enum SpiritType
{
    none,
    basic,
    fire,
    water,
    flora,
    electric,
    earth,
    wind
}


public enum Spiritgender
{
    Male,
    Female,
    NA
}

[CreateAssetMenu(menuName = "Spirit/ Spirit/ Generic")]    
public class Spirit : ScriptableObject
{
    public void Awake()
    {
        InitEffectiveStats();
    }


    //3d model
    public GameObject body;
    
    //pixel sprite
    public Texture2D sprite;
    //more detailed portrait
    public Texture2D portrait;



    //name
    public string NickName; //nickname
    [HideInInspector]public string SpiritName;
    public Spiritgender gender; //true for male, false for female

    //level // multiplies effective base stats
    public int level;
    [HideInInspector] public int expCap;
    [HideInInspector] public int expHeld;

    //base stats //for Atk Def Spe max is 10, min is 1, total distribution is 15 // for hp max is 20
    public int baseMaxHp;
    public int baseAttack;
    public int baseDefense;
    public int baseSpeed;

    //effective stats
    public float effectiveMaxHp;
    public float currentHP;
    public float effectiveAttack;
    public float effectiveDefense;
    public float effectiveSpeed;


    //extra stats maxes out at 100

    //elemental type
    public SpiritType primaryType;
    public SpiritType secondaryType;

    
    //current moves //3 slots
    public SpiritAttacks[] attacks = new SpiritAttacks[3];

    [HideInInspector] public SpiritAttacks[] capableAttacks;

    public void MaxHeal()
    {
        currentHP = effectiveMaxHp;
    }

    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if(currentHP > effectiveMaxHp)
            currentHP = effectiveMaxHp;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if(currentHP < 0)
            currentHP = 0;
    }

    public void MaxDamage()
    {
        currentHP = 0;
    }

    public bool IsDead()
    {
        //returns true if spirit has been knocked out
        if(currentHP <= 0)
            return true;
        else
            return false;
    }

    public void InitEffectiveStats()//includes heal
    {
        effectiveMaxHp = baseMaxHp * ((level/5f)+1f);
        effectiveAttack = baseAttack * ((level/5f)+1f);
        effectiveDefense = baseDefense * ((level/5f)+1f);
        effectiveSpeed = baseSpeed * ((level/5f)+1f);

        effectiveMaxHp = Mathf.Floor(effectiveMaxHp);
        currentHP = effectiveMaxHp;
        effectiveAttack = Mathf.Floor(effectiveAttack);
        effectiveDefense = Mathf.Floor(effectiveDefense);
        effectiveSpeed = Mathf.Floor(effectiveSpeed);

    }

    public void updateEffectiveStats()//no heal
    {
        effectiveMaxHp = baseMaxHp * ((level/5f)+1f);
        effectiveAttack = baseAttack * ((level/5f)+1f);
        effectiveDefense = baseDefense * ((level/5f)+1f);
        effectiveSpeed = baseSpeed * ((level/5f)+1f);


        effectiveMaxHp = Mathf.Floor(effectiveMaxHp);
        effectiveAttack = Mathf.Floor(effectiveAttack);
        effectiveDefense = Mathf.Floor(effectiveDefense);
        effectiveSpeed = Mathf.Floor(effectiveSpeed);

    }
}


