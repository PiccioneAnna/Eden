using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageable
{
    [SerializeField] int hp = 10;
    QuestManager questManager;
    public Mob mob;

    public void Awake()
    {
        questManager = GameManager.instance.GetComponent<GameManager>().questManager;
    }

    public void ApplyDamage(int damage)
    {
        hp -= damage;
    }

    public void CalculateDamage(ref int damage)
    {
        damage /= 2;
    }

    public void CheckState()
    {
        if(hp <= 0) 
        {
            Debug.Log(mob);
            questManager.KillMob(mob);
            Destroy(gameObject); 
        }
    }
}
