using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Player player;
    public DayTimeController timeController;
    public QuestManager questManager;
    public DialogueSystem dialogueSystem;
}
