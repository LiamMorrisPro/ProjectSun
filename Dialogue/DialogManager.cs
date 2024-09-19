using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Events;

/////////////////////////////////////////////

//The Dialogue tree is received by a script attached to the player 
//that accesses the held dialogue tree of an NPC and sends it to the dialogue manager,
//thus triggering the dialogue gamestate stopping the players movement and camera's movement
//until the Dialogue managers coroutine resolves

/////////////////////////////////////////////

namespace TMPro
{
    public enum Emotion { happy, sad, suprised, angry };

    [System.Serializable] public class ActionEvent : UnityEvent<string> { }

    [System.Serializable] public class TextRevealEvent : UnityEvent<char> { }

    [System.Serializable] public class DialogueEvent : UnityEvent { }

}
public class DialogManager : MonoBehaviour
{
    [Header("DialogTree")]
    public DialogTree dialogTree;
    private DialogTree currentTree;

    [Header("Dialog")]
    [SerializeField] private TMP_Text dialogtext;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private thirdPersonCam playerCamera;


    ///coroutines
    bool canContinueDialogue = false;

    [SerializeField] private float speed = 10;
    
    public ActionEvent onAction;
    public TextRevealEvent onTextReveal;
    public DialogueEvent onDialogueFinish;


    private void Start()
    {
        dialogPanel = this.GameObject();
    }

    public void StartDialog()
    {
        if(dialogTree != null)
        {
            playerCamera.SetCameraStyle(CameraStyle.Dialogue);
            GameState.instance.state = GameState.play_state.IN_MENU;
            dialogPanel.SetActive(true);
            StartCoroutine(ParseTree(dialogTree));
        } 
        
    }
 
    public IEnumerator ParseTree(DialogTree currentTree) //go through the tree element by element
    {

        for (int i = 0; i < currentTree.TreeBehaviour.Count; i++)
        {
            //CHECK TYPE
            var type = currentTree.TreeBehaviour[i].GetType();


            //CONDITION
            if(type == typeof(_Condition)) 
            {
                
                List<Paths> data = currentTree.GetConditions(i);
                for(int j = 0; j < data.Count; j++)
                {
                  print("hi");  
                    if(data[j].istrue == true)
                    {
                        //reset the index and change the tree (-1 is necessary)
                        i = -1;
                        currentTree = data[j].branch;
                        
                        
                    }
                }
            }

            //SEQUENCE
            if(type == typeof(_Sequence)) 
            {
                List<string> data = currentTree.GetSequenceDialogs(i);
                foreach (string dialog in data)
                {
                    canContinueDialogue = false;
                    ReadText(dialog);
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) && canContinueDialogue);
                   
                }
            }
            //CHOICE
            else if(type == typeof(_Choice))
            {
                //Haven't implement UI interface for choices
            }
            else if(type == typeof(_Battle))
            {
                if(Battle2_0._iBattle != null)
                {
                    Battle2_0._iBattle.StartBattle();
                    EndDialogue();
                }
            }
            else if(type == typeof(Paths))
            {
                 
            }
        }
        //FORCE END
        EndDialogue(); 
    }

    public void BranchTree(DialogTree dialogTree) //change the current active tree
    {
        currentTree = dialogTree;
    }

    public void EndDialogue() //force quit the dialog and stop all coroutines
    {
        playerCamera.SetCameraStyle(CameraStyle.Basic);
        if(GameState.instance.state == GameState.play_state.IN_MENU)
            GameState.instance.state = GameState.play_state.IN_PLAY;
        StopCoroutine(ParseTree(dialogTree));
        dialogPanel.SetActive(false);
        //make dialog box inactive as well
        return;
    }


    public void ReadText(string newtext)
    {
        dialogtext.text = string.Empty;
        continueIcon.SetActive(false);


        //split whole text into parts based on <> tags
        //even numbers in the array are text, odd ones are tags
        string[] subTexts = newtext.Split('<', '>');

        //textmeshpro will parse its own tags, so only include non custom tags
        string displayText = "";
        for(int i = 0; i < subTexts.Length; i++)
        {
            if(i % 2 == 0)
                displayText += subTexts[i];
            else if (!isCustomTag(subTexts[i].Replace(" ", "")))
                displayText += $"<{subTexts[i]}>";
        }
        bool isCustomTag(string tag)
        {
            return tag.StartsWith("speed=") || tag.StartsWith("pause=") || tag.StartsWith("emotion=") || tag.StartsWith("action");
        }

        //send that text to textmeshpro and hide it the start reading
        dialogtext.text = displayText;
        dialogtext.maxVisibleCharacters = 0;
        StartCoroutine(Read());

        IEnumerator Read()
        {
            int subCounter = 0;
            int visibleCounter = 0;
            while(subCounter < subTexts.Length)
            {
                if(subCounter % 2 == 1)
                {
                    yield return EvaluateTag(subTexts[subCounter].Replace(" ", ""));
                }
                else
                {
                    while(visibleCounter < subTexts[subCounter].Length)
                    {
                        onTextReveal.Invoke(subTexts[subCounter][visibleCounter]);
                        visibleCounter++;
                        dialogtext.maxVisibleCharacters++;
                        yield return new WaitForSeconds(1f / speed);
                    }
                    visibleCounter = 0;
                }
                subCounter++;
            }
            continueIcon.SetActive(true);
            canContinueDialogue = true;


            WaitForSeconds EvaluateTag(string tag)
            {
                    if (tag.Length > 0)
                    {
                        if (tag.StartsWith("speed="))
                        {
                            speed = float.Parse(tag.Split('=')[1]);
                        }
                        else if (tag.StartsWith("pause="))
                        {
                            return new WaitForSeconds(float.Parse(tag.Split('=')[1]));
                        }
                        else if (tag.StartsWith("action="))
                        {
                            onAction.Invoke(tag.Split('=')[1]);
                        }  
                                  
                    }
                    return null;
            }
            onDialogueFinish.Invoke();
        }
    }
}
