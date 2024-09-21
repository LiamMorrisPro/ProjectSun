using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


    //////////////////////////////////////////////////////////////////////////////
    //
    //  This battle script will be used specificly for battles against NPC's
    //  It places the attached gameobject in a prepared spot in the world, so
    //  it's best used on pre-prepared battlefields with plenty of space
    //
    //////////////////////////////////////////////////////////////////////////////



public class Battle2_0 : MonoBehaviour
{
    public static Battle2_0 _iBattle;

    //references to other scipts
    public BattleCamera battleCamera;
    public GameObject battleHud;
    public battleHud playerHud;
    public battleHud enemyHud;
    public DamageCalculator damageCalculator;

    public GameObject Player;
    public GameObject NPC;

    public TMP_Text BattleDialogue;


    //BattleState
    [HideInInspector]public enum BattleState {  START, GOFIRST, GOSECOND, WON, LOST }
     [HideInInspector]public BattleState battleState;
    private bool canInput = false;

    //Obj positions
    public GameObject playerPos;
    public GameObject enemyPos;
    public GameObject playerSpiritPos;
    public GameObject enemySpiritPos;
    [HideInInspector] public GameObject playerSpiritObj;
    [HideInInspector] public GameObject enemySpiritObj;


    //spirit data
    public List<Spirit> playerParty = new List<Spirit>();
    public List<Spirit> enemyParty = new List<Spirit>();

    [HideInInspector]public Spirit currentPlayerSpirit;
    [HideInInspector]public Spirit currentEnemySpirit;


    //tools
    void ToolFullHeal() //tool for testing
    {
        for(int i = 0; i < playerParty.Count; i++)
        {
            if(playerParty[i] != null){playerParty[i].MaxHeal();}
        }
        for(int i = 0; i < enemyParty.Count; i++)
        {
            if(enemyParty[i] != null){enemyParty[i].MaxHeal();}
        }
    }
    void InitStats()
    {
        for(int i = 0; i < playerParty.Count; i++)
        {
            if(playerParty[i] != null){playerParty[i].updateEffectiveStats();}
        }
        for(int i = 0; i < enemyParty.Count; i++)
        {
            if(enemyParty[i] != null){enemyParty[i].updateEffectiveStats();}
        }  
    }

    private bool SpeedCheck(Spirit player, Spirit enemy)//compare speed and decide who moves first
    {
        if(player.effectiveSpeed > enemy.effectiveSpeed)
            {return true;}
        else if(player.effectiveSpeed < enemy.effectiveSpeed)
            {return false;}
        else //by default a speed tie goes in the players favor //or swap with 50/50
            {return true;}
    }
    private void SendOutSpirit(int spiritSlot, string playerOrEnemy)
    {
        if(playerOrEnemy == "Player")
        {
            //data
            currentPlayerSpirit = playerParty[spiritSlot];
            playerHud.setHud(currentPlayerSpirit);
            playerHud.setMoves(currentPlayerSpirit);
            //visuals
            if(playerSpiritObj != null)
                Destroy(playerSpiritObj);
            playerSpiritObj = Instantiate(currentPlayerSpirit.body, playerSpiritPos.transform);
            //dialogue
            //dialogueText.text = "player sent out " + currentPlayerSpirit.NickName;
        }
        else if(playerOrEnemy == "Enemy")
        {
            //data
            currentEnemySpirit = enemyParty[spiritSlot];
            enemyHud.setHud(currentEnemySpirit);
            //visuals
            if(enemySpiritObj != null)
                Destroy(enemySpiritObj);
            enemySpiritObj = Instantiate(currentEnemySpirit.body, enemySpiritPos.transform);
            //dialogue
            //dialogueText.text = "enemy sent out " + currentEnemySpirit.NickName;
        }
    }
    private void WinCheck()
    {
        int playerSpiritNum = 0;
        int enemySpiritNum = 0;

        for(int i = 0; i < playerParty.Count; i++)
            if(playerParty[i] != null && playerParty[i].currentHP > 0){playerSpiritNum += 1;}   
        for(int i = 0; i < enemyParty.Count; i++)
            if(enemyParty[i] != null && enemyParty[i].currentHP > 0){enemySpiritNum += 1;}

        if(playerSpiritNum <= 0){battleState = BattleState.LOST; StopAllCoroutines(); Endbattle();}
        else if(enemySpiritNum <= 0){battleState = BattleState.WON; StopAllCoroutines(); Endbattle();}
        
    }
    public void DynamicAnimation(GameObject Target, GameObject CamPos, String AnimTrigger)
    {
        //get animator
        if(Target.TryGetComponent<Animator>(out Animator animator))
        {
            //play animation
            animator.SetTrigger(AnimTrigger);            
        }
        //focus on target
        battleCamera.FocusTarget(Target, CamPos);

    }

    private void UpdateBattleText(string Tag, string Name)
    {
        if(Tag == "Null")
        {
            BattleDialogue.text = "";
        }
        if(Tag == "Attack Selected")
        {
            BattleDialogue.text = "use " + Name + "?";
        }
        if(Tag == "SendOut")
        {
            BattleDialogue.text = "Opponent sent out" + Name;
        }
        if(Tag == "Attack")
        {
            BattleDialogue.text = Name + " attacks";
        }
        if(Tag == "GetHit")
        {
            BattleDialogue.text = Name + " was hit";
        }
        if(Tag == "Faint")
        {
            BattleDialogue.text = Name + " got knocked out";
        }

    }

    //initialize battle    
    public void StartBattle()
    {
        UpdateBattleText("Null", "");
        GameState.instance.state = GameState.play_state.IN_BATTLE;
        //set player position to appropriate transform
            Player.transform.position = playerPos.transform.position;
            Player.transform.rotation = Quaternion.Euler(0,90,0);
        //set npc position to appropriate transform
            //NPC.transform.position = enemyPos.transform.position;
            //NPC.transform.rotation = Quaternion.Euler(0,-90,0);
        //activate battle camera
        battleCamera.activateBattleCam();

        //toggle battle ui
        battleHud.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //construct player party based on spirits in data
        playerParty = SpiritSaveData.PlayerPartySpirits;  

        enemyParty = EnemyData.currentEnemyparty.spirits;

        
        //update the effective stats for all spirits in battle
        InitStats();
        //for testing
        ToolFullHeal();
        
        //make sure both spirit parties are not empty
        WinCheck();

        StartCoroutine(BattleInit());
    }
    IEnumerator BattleInit()
    {
        
        //send out first spirit in enemy party
        for(int i = 0; i < 3; i++)
        {
            if(enemyParty[i] != null && enemyParty[i].currentHP > 0)
                {SendOutSpirit(i,"Enemy"); break; }

            if(i == 2) //if you reach this and there are no more spirits to check, then end the battle
                {battleState = BattleState.WON; Endbattle();}
        }
        //send out first spirit in player party
        for(int i = 0; i < 3; i++)
        {
            if(playerParty[i] != null && playerParty[i].currentHP > 0)
                {SendOutSpirit(i,"Player"); break;}

            if(i == 2) //if you reach this and there are no more spirits to check, then end the battle
                {battleState = BattleState.LOST; Endbattle();}
        }
        yield return new WaitForSeconds(1f);
        
        NewTurn();
    }
    
    //start new turn
    void NewTurn()
    {
        UpdateBattleText("Null", "");
        battleCamera.FocusTarget(battleCamera.arenaCentre, battleCamera.camPos1);
        bool goFirst = SpeedCheck(currentPlayerSpirit, currentEnemySpirit);

        if(goFirst == true)
            {battleState = BattleState.GOFIRST; PlayerTurn();}
        else
            {battleState = BattleState.GOSECOND; PlayerTurn();}
    }

    //Make a move   
    void PlayerTurn() //Alows the player to input a move
    {

        //BattleDialogue.GetComponent<TextMeshPro>().text = "make a move";
        canInput = true;
    }
    private int EnemyTurn()
    {
        //decide attacking move

        //execute attack
        return 0;
    }



    int PlayerAttackID = 0;
    public void OnAttackButton(int AttackID)
    {
        
        PlayerAttackID = AttackID;

        UpdateBattleText("Attack Selected", currentPlayerSpirit.attacks[PlayerAttackID].moveName);
        //highlight selected attack button
        //higlight confirm button
    }

    public void OnTurnFinish() 
    {
        
        int EnemyAttackID = EnemyTurn();

        if(currentPlayerSpirit.attacks[PlayerAttackID] == null)
            {return;}    
        else if(battleState == BattleState.GOFIRST && canInput)
            {canInput = false; StartCoroutine(AttackProcess("Player", currentPlayerSpirit.attacks[PlayerAttackID], currentPlayerSpirit, currentEnemySpirit,EnemyAttackID));}
        else if(battleState == BattleState.GOSECOND && canInput)
            {canInput = false; StartCoroutine(AttackProcess("Enemy",currentEnemySpirit.attacks[EnemyAttackID], currentEnemySpirit, currentPlayerSpirit, PlayerAttackID));}
        else {return;}
        
    }

    //attack Process
    IEnumerator AttackProcess(string playerOrEnemy, SpiritAttacks attack, Spirit self, Spirit opponent, int opponentsAttack)
    {
        //some useful variables
        bool targetKO = false;
        bool selfKO = false;

        if(playerOrEnemy == "Player")
        {
            UpdateBattleText("Attack", currentPlayerSpirit.NickName);
            DynamicAnimation(playerSpiritObj, battleCamera.camPos2, "Attack");
        }
        if(playerOrEnemy == "Enemy")
        {
            UpdateBattleText("Attack", currentEnemySpirit.NickName);
            DynamicAnimation(enemySpiritObj, battleCamera.camPos3, "Attack");
        }
        
        yield return new WaitForSeconds(2f); //wait for time needed for attack // ~ get from animation length
        
        //iterate through attacks movebehaviours
        for(int i = 0; i < attack.moveBehaviour.Count; i++)
        {
            var moveType = attack.moveBehaviour[i].GetType();

            if(moveType == typeof(_DamageEnemy))
            {
                opponent.currentHP = opponent.currentHP - damageCalculator.DamageCalculation(attack, self, opponent, i);
                if(opponent.currentHP <= 0){targetKO = true;}
                    
                if(playerOrEnemy == "Player")
                {enemyHud.setHP(opponent.currentHP, opponent.effectiveMaxHp);
                UpdateBattleText("GetHit", currentEnemySpirit.NickName);
                DynamicAnimation(enemySpiritObj, battleCamera.camPos3, "GetHit");}

                if(playerOrEnemy == "Enemy")
                {playerHud.setHP(opponent.currentHP, opponent.effectiveMaxHp);
                UpdateBattleText("GetHit", currentPlayerSpirit.NickName);
                DynamicAnimation(playerSpiritObj, battleCamera.camPos2, "GetHit");}

                yield return new WaitForSeconds(2f);
         
                


            }
            else if(moveType == typeof(_DamageSelf))
            {
                self.currentHP -= 5;
                if(self.currentHP <= 0)
                    {selfKO = true;}
                
                //setHud
                if(playerOrEnemy == "Player")
                {playerHud.setHP(self.currentHP, self.effectiveMaxHp);}
                if(playerOrEnemy == "Enemy")
                {playerHud.setHP(self.currentHP, self.effectiveMaxHp);} 
                
                //dialogue
            }
            else if(moveType == typeof(_HealEnemy))
            {
                opponent.currentHP += 1;
                //set hud //dialogue
            }
            else if(moveType == typeof(_HealSelf))
            {
                self.currentHP += 1;
                //set hud //dialogue
            }
        }
        
        if(targetKO)//handle if target knocked out (just remove current spirit, we'll add one later)
        {
            if(playerOrEnemy == "Player")
            {
                UpdateBattleText("Faint", currentEnemySpirit.NickName);
                DynamicAnimation(enemySpiritObj, battleCamera.camPos3, "Die");
                yield return new WaitForSeconds(2f);
            }
            else if(playerOrEnemy == "Enemy")
            {
                UpdateBattleText("Faint", currentPlayerSpirit.NickName);
                DynamicAnimation(playerSpiritObj, battleCamera.camPos2, "Die");
                yield return new WaitForSeconds(2f);
            }

        }
        
        if(selfKO)//handle if self knocked out // immedietly send out new spirit
        {
            if(playerOrEnemy == "Player")
            {
                UpdateBattleText("Faint", currentPlayerSpirit.NickName);
                DynamicAnimation(playerSpiritObj, battleCamera.camPos2, "Die");
                yield return new WaitForSeconds(2f);
                //dialogue
                for(int i = 0; i < playerParty.Count(); i++)
                {
                    if(playerParty[i] != null && playerParty[i].currentHP > 0){SendOutSpirit(i, "Player"); break;} 
                }
                WinCheck();
            }
            else if(playerOrEnemy == "Enemy")
            {
                UpdateBattleText("Faint", currentEnemySpirit.NickName);
                DynamicAnimation(enemySpiritObj, battleCamera.camPos3, "Die");
                for(int i = 0; i < playerParty.Count(); i++)
                {
                    if(enemyParty[i] != null && enemyParty[i].currentHP > 0){SendOutSpirit(i, "Enemy"); break;} 
                }
                WinCheck();
            }

        }
        
        if(targetKO)//now send out the next spirit in opponents party
        {
            if(playerOrEnemy == "Player")
            {
                for(int i = 0; i < enemyParty.Count(); i++)
                {
                    if(enemyParty[i] != null && enemyParty[i].currentHP > 0){SendOutSpirit(i, "Enemy"); break;} 
                }
                WinCheck();
                NewTurn();
            }
            else if(playerOrEnemy == "Enemy")
            {
                for(int i = 0; i < playerParty.Count(); i++)
                {
                    if(playerParty[i] != null && playerParty[i].currentHP > 0){SendOutSpirit(i, "Player"); break;} 
                    if(i == playerParty.Count() - 1){; Endbattle();}

                }
                WinCheck();
                NewTurn();
                
            }
        }



        //decide next turn logic
        if(playerOrEnemy == "Player" && battleState == BattleState.GOFIRST && targetKO == false)//if player turn done and went first, the its enemy's turn
            {StartCoroutine(AttackProcess("Enemy", currentEnemySpirit.attacks[opponentsAttack], currentEnemySpirit, currentPlayerSpirit, 0));}
        else if(playerOrEnemy == "Player" && battleState == BattleState.GOSECOND)//if player turn done and went second starts new turn
            {NewTurn();}
        else if(playerOrEnemy == "Enemy" && battleState == BattleState.GOFIRST)//if enemy done and player went first the new turn
            {NewTurn();}
        else if(playerOrEnemy == "Enemy" && battleState == BattleState.GOSECOND && targetKO == false)//if enemy done and player to go second, player's turn
            {StartCoroutine(AttackProcess("Player", currentPlayerSpirit.attacks[opponentsAttack], currentPlayerSpirit, currentEnemySpirit, 0));}


    }


    //end battle
    private void Endbattle()
    {
        if(playerSpiritObj != null)
                Destroy(playerSpiritObj);

        if(enemySpiritObj != null)
                Destroy(enemySpiritObj);

        battleCamera.deactivateBattleCam();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameState.instance.state = GameState.play_state.IN_PLAY;
        battleHud.SetActive(false);
        
    }

}
