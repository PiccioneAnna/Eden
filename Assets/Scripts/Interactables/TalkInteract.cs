using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkInteract : Interactable
{
    [SerializeField] DialogueTree dialogue;

    public override void Interact(Player player)
    {
        GameManager.instance.dialogueSystem.Initialize(dialogue, player);
    }
}
