using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class battleHud : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text levelText;
    public Slider hpSlider;
    public TMP_Text hpNumber;

    public Button AB1;
    public Button AB2;
    public Button AB3;

    public void setHud(Spirit spirit)
    {
        nameText.text = spirit.NickName;
        levelText.text = "Lv " + spirit.level;
        hpSlider.maxValue = spirit.effectiveMaxHp;
        hpSlider.value = spirit.currentHP;

        hpNumber.text = spirit.currentHP + " / " + spirit.effectiveMaxHp;
    }

    public void setHP(float hp, float maxhp)
    {
        if(hp < 0)
            hp = 0;

        hpSlider.value = hp;
        hpNumber.text = hp + " / " + maxhp;
    }

    public void setMoves(Spirit spirit)
    {
        if(AB1 != null && spirit.attacks[0] != null)
        {
            TMP_Text Text = AB1.GetComponentInChildren<TMP_Text>();
            Text.text = spirit.attacks[0].name;

            AB1.image.color = AttacktypeToColor(spirit.attacks[0]);
        }
        if(AB2 != null && spirit.attacks[1] != null)
        {
            TMP_Text Text = AB2.GetComponentInChildren<TMP_Text>();
            Text.text = spirit.attacks[1].name;

            AB2.image.color = AttacktypeToColor(spirit.attacks[1]);
        }
        if(AB3 != null && spirit.attacks[2] != null)
        {
            TMP_Text Text = AB3.GetComponentInChildren<TMP_Text>();
            Text.text = spirit.attacks[2].name;

            AB3.image.color = AttacktypeToColor(spirit.attacks[2]);
        }

        if(spirit.attacks[0] == null)
        {
            TMP_Text Text = AB1.GetComponentInChildren<TMP_Text>();
            Text.text = "";

            AB1.image.color = Color.gray;                
        }
        if(spirit.attacks[1] == null)
        {
            TMP_Text Text = AB2.GetComponentInChildren<TMP_Text>();
            Text.text = "";

            AB2.image.color = Color.gray;            
        }
        if(spirit.attacks[2] == null)
        {
            TMP_Text Text = AB3.GetComponentInChildren<TMP_Text>();
            Text.text = "";

            AB3.image.color = Color.gray;                           
        }
    }

    

    private Color AttacktypeToColor(SpiritAttacks attack)
    {
        if(attack.moveType == MoveType.basic)
        {
            return Color.white;
        }
        else if(attack.moveType == MoveType.fire)
        {
            return Color.red;
        }
        else if(attack.moveType == MoveType.water)
        {
            return Color.blue;
        }
        else if(attack.moveType == MoveType.flora)
        {
            return Color.green;
        }

        else
        {
            return Color.black;
        }
    }


    //attach attack button info and
}
