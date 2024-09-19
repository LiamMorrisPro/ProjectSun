using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UI;

//I have misspelled Dialogue, forgive me

[CreateAssetMenu(menuName = "Dialogue/Dialogue Tree")]
public class DialogTree : ScriptableObject
{
    [SerializeReference] public List<DialogOption> TreeBehaviour;

    #region Menu Items

    [ContextMenu(nameof(AddCondition))] void AddCondition(){ TreeBehaviour.Add(new _Condition());}
    [ContextMenu(nameof(AddSequence))] void AddSequence(){ TreeBehaviour.Add(new _Sequence());}
    [ContextMenu(nameof(AddChoice))] void AddChoice(){ TreeBehaviour.Add(new _Choice());}
    [ContextMenu(nameof(AddBattle))] void AddBattle(){TreeBehaviour.Add(new _Battle());}
    [ContextMenu(nameof(AddEnd))] void AddEnd(){TreeBehaviour.Add(new _End());}

    #endregion




    
    /// public methods 

    public List<Paths> GetConditions(int index)
    {
        List<Paths> checks = new List<Paths>();

        if(TreeBehaviour[index] is _Condition condition)
        {
            foreach(Paths path in condition.Conditions)
                checks.Add(path);
        }
        return checks;
    }

    public List<string> GetSequenceDialogs(int index)
    {
        List<string> dialogs = new List<string>();

        if(TreeBehaviour[index] is _Sequence sequence)
        {
            foreach(var dialog in sequence.Sequence)
                dialogs.Add(dialog.text);
        }
        return dialogs;
    }

    public DialogTree GetNewTree(int Bindex, int Cindex)
    {
        DialogTree newTree  = new DialogTree();

        if(TreeBehaviour[Bindex] is _Choice choice)
        {
            newTree = choice.Choices[Cindex].branch;
        }

        return newTree;
    }

    
    public List<string> GetChoices(int index)
    {
        List<string> choicetext = new List<string>();
        if(TreeBehaviour[index] is _Choice choice)
        {
            foreach(var Choices in choice.Choices)
                choicetext.Add(choice.Choices[index].text);
        }
        return choicetext;
    }
    
}

///
/// Dialogue Options
///

[Serializable] public class DialogOption
{
}
///////////////////////////////////////////////////////////////////////////////
public class _Condition : DialogOption
{
    public List<Paths> Conditions = new List<Paths>();
}
[Serializable] public class Paths
{
    public string Check;
    public bool istrue;
    public DialogTree branch;
}

///////////////////////////////////////////////////////////////////////////////
public class _Sequence : DialogOption
{
    public string Name;
    public List<Dialog> Sequence = new List<Dialog>();
}
[Serializable] public class Dialog
{
    [TextArea(3,10)]
    public string text;
}
///////////////////////////////////////////////////////////////////////////////
public class _Choice : DialogOption //Not currently implemented through the UI
{
    public List<Choices> Choices = new List<Choices>();
}
[Serializable] public class Choices
{
    public string text;
    
    public DialogTree branch;
}
///////////////////////////////////////////////////////////////////////////////
public class _Battle : DialogOption //doesn't need to carry any data, just check for the behaviour type
{
    
}
///////////////////////////////////////////////////////////////////////////////
public class _End : DialogOption //doesn't need to carry any data, just check for the behaviour type
{
    
}

