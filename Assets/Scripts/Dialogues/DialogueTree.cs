using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dialogue/Tree")]
public class DialogueTree : ScriptableObject
{
    public List<DialogueContainer> dialogueContainers;
    public Actor actor;

    private int currentPlayerLevel;
    
    // Theoretically goes through dialogue list and returns dialogue matching player's level
    public DialogueContainer GetCurrentDialogue()
    {
        currentPlayerLevel = GameManager.instance.player.GetComponent<Character>().level;

        foreach (DialogueContainer dialogue in dialogueContainers)
        {
            if(dialogue.levelRequirement == currentPlayerLevel)
            {
                return dialogue;
            }
        }
        return null;
    }

    // return a generic task dialogue if player chooses task


    // returns a generic talk dialogue if player chooses talk
}
